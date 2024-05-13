using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IECS_CRUD.Helpers
{
    public static class LoggerHelper
    {
        public static void LogError(Exception ex)
        {
            string logFilePath = "error.log";

            try
            {
                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine($"{DateTime.Now}: {ex.Message}");
                    writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                    writer.WriteLine();
                }
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"Error occurred while logging: {logEx.Message}");
            }
        }
    }
}
