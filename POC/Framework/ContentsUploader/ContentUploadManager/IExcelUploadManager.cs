using ContentUploader.Model;
using System.Collections.Generic;
using System.Web;

namespace ContentsUploader.ContentUploadManager
{
    public interface IExcelUploadManager
    {
        // void PersistUploadedCsvContent(string fileBaseFileName, byte[] contentBytes);
        //CsvFile PersistUploadedCsvContent(string fileBaseFileName, byte[] contentBytes);
        // object PersistUploadedCsvContent(string fileBaseFileName, byte[] contentBytes);
        // CsvFile PersistUploadedCsvContent(HttpPostedFileBase fileBase);
        // object PersistUploadedCsvContent(IFormFile fileBase);
        //object PersistUploadedExcellContent(IFormFile fileBase);
        //ExcelFile PersistUploadedCsvContent(HttpPostedFileBase fileBase);
        int PersistUploadedExcelContentForCuraredExperience(HttpPostedFileBase fileBase, List<string> errorMessages);
        int PersistUploadedExcelContentForVideo(HttpPostedFileBase fileBase, List<string> errorMessages);
        int PersistUploadedExcelContentForQA(HttpPostedFileBase fileBase, List<string> errorMessages);
        bool RollbackMigration(int migrationId, string connectionString);

        // byte[] GetCsvContentBytes();
        //List<CsvFile> GetFiles();
        // List<object> GetFiles();
        //CsvFile GetFile(int id);
        // object GetFile(int id);

    }
}