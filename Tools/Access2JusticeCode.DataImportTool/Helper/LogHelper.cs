using System;
using System.IO;

namespace Access2Justice.DataImportTool.Helper
{
    public class LogHelper
    {
        public static void DataLogging(string data, string fileName)
        {

            string strPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\SampleFiles\\" + fileName);
            using (StreamWriter bw = new StreamWriter(File.Create(strPath)))
            {
                bw.Write(data);
                bw.Close();
            }
        }
    }
}
