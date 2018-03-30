using ContentDataAccess;
using ContentDataAccess.DataContextFactory;
using System;
using System.Collections.Generic;
using System.Configuration;
using Serilog;
using Serilog.Core;

namespace ContentsTranslator
{
    class Program
    {
        private static ContentDataRepository contentDataRepository = new ContentDataRepository(new CrowledContentDataContextFactory());
        private string dbConnectionStringName = ConfigurationManager.AppSettings["ConnectionString"];
        private string[] separators = { ",", ".", "!", "?", ";", ":", " " };
        private  static Logger _logger = CreateLogger();

        static void Main(string[] args)
        {
            try
            {
                var batchId = Int32.Parse(args[0]);
                var state = args[1];
                var targetLanguageCode = args[2];
                _logger.Information("Started Content Translator");
                TranslationManager translationManager = new TranslationManager(contentDataRepository, _logger);
                var valid = translationManager.CountCharactersToBeTranslatedWithInLimit(batchId,targetLanguageCode);
                if (valid)
                {
                    translationManager.TranslateSentences(batchId, state, targetLanguageCode);
                }
                Console.ReadKey();
            }
            catch(AggregateException ae)
            {
                if (ae != null)
                {
                    if (ae.InnerException != null && ae.InnerExceptions.Count >= 1)
                    {
                        var count = 1;
                        foreach (var innerException in ae.InnerExceptions)
                        {
                            _logger.Error($"Inner Exception {count++} Error occured: with exception message = {innerException.Message}   and stackTrace = {innerException.StackTrace} ");

                        }
                    }
                    else
                    {
                        _logger.Error($"Error occured: with exception message = {ae.Message}  and stackTrace = {ae.StackTrace} ");

                    }
                }
                    
            }
            catch (Exception e)
            {
                if (e != null)
                {
                    _logger.Error($"Error occured: with exception message = {e.Message}  and stackTrace = {e.StackTrace}");
                }
            }
        }
        void TranslateCaregories(int batchId, string languageCode)
        {
            var categories = contentDataRepository
                               .GetLawCategories(dbConnectionStringName)
                               .FindAll(cat => cat.Batch_Id == batchId);

            List<string> listOfWords = new List<string>();
            categories.ForEach((x) => {
                                       listOfWords.AddRange(
                                               x.Description.Split(separators, StringSplitOptions.RemoveEmptyEntries));
                                       listOfWords.AddRange(
                                               x.StateDeviation.Split(separators, StringSplitOptions.RemoveEmptyEntries));
                                      });
                


        }

        public static Logger CreateLogger()
        {
            return  new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.RollingFile(@"..\..\logs\A2J\Log-{Date}.txt")
                .CreateLogger();
        }
    }
}
