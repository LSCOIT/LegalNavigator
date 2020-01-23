﻿using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.DataFixers;
using CommandLine;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Access2Justice.DataFixes
{
    class Program
    {
        private static Dictionary<string, Func<CommandLineOptions, IDataFixer>> DataFixersRegistry =
            new Dictionary<string, Func<CommandLineOptions, IDataFixer>>
            {
                { "#829", x => new Issue829Fixer() },
                { "#350", x => new Issue350DataFixer() },
                { "#875", x => new Issue875Fixer() },
                { "#__duplicates", x => new DuplicatesRemover() },
                { "#__getData", x => new DataLoader(x.AdditionalPram) },
				{ "#953", x => new Issue953Fixer() },
                { "#__removeResources", x => new ResourcesRemoval() },
                { "#__updateResources", x => new UpdateResources() }
            };


        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory()).
                AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            var configuration = builder.Build();

            var parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            var parsedOptions = parserResult as Parsed<CommandLineOptions>;

            if (parsedOptions != null)
            {
                await ApplyFix(parsedOptions.Value, configuration);
            }

            Console.WriteLine("DONE");
            Console.ReadKey();
        }

        private static async Task ApplyFix(
            CommandLineOptions commandLineOptions,
            IConfiguration appConfiguration)
        {
            var issueId = commandLineOptions.IssueId;

            if (!ValidateIssueId(issueId))
            {
                Console.WriteLine(
                    $"ERROR: invalid issue id, fixes available for: {string.Join(", ", DataFixersRegistry.Keys)}");
                return;
            }

            try
            {
                var cosmosDbSettings = new CosmosDbSettings(
                    appConfiguration.GetSection("CosmosDb"),
                    commandLineOptions.CosmosDbDatabaseId);

                var documentClient = new DocumentClient(
                    new Uri(commandLineOptions.CosmosDbEndpoint),
                    commandLineOptions.CosmosDbApiKey);

                var cosmosDbService = new CosmosDbService(documentClient, cosmosDbSettings);

                var dataFixer = DataFixersRegistry[issueId](commandLineOptions);

                await dataFixer.ApplyFixAsync(cosmosDbSettings, cosmosDbService);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }
        }

        private static bool ValidateIssueId(string issueId)
        {
            if (!issueId.StartsWith("#"))
            {
                return false;
            }

            if (!DataFixersRegistry.ContainsKey(issueId))
            {
                return false;
            }

            return true;
        }
    }
}
