using System;
using AcumaticaFilesImport.Files;
using AcumaticaFilesImport.Files.Csv;
using Microsoft.Extensions.Logging;

namespace AcumaticaFilesImport.Logging
{
    public static class ILoggerExt
    {
        public static void LogFileAttached(this ILogger logger, UploadItem item)
        {
            string message = GenerateRecordPrefix(item);
            message += " - Success";

            logger.LogInformation(message);
        }

        public static void LogRecordMissing(this ILogger logger, UploadItem item)
        {
            logger.LogError("ERROR: " + GenerateRecordPrefix(item) + " RECORD MISSING");
        }

        public static void LogDocTypeUnsupported(this ILogger logger, UploadItem item)
        {
            logger.LogError($"ERROR: {GenerateRecordPrefix(item)} - Document Type {item.Endpoint} Unsupported");
        }

        private static string GenerateRecordPrefix(UploadItem item)
        {
            string seperator = " / ";
            string message = item.Endpoint + seperator;

            foreach (string keyValue in item.KeyValues)
            {
                message += keyValue + seperator;
            }
            return message.TrimEnd(seperator.ToCharArray());
        }
    }
}