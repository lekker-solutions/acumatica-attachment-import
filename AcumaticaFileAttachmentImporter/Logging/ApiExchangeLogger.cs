﻿
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;

namespace AcumaticaRestApiExample
{
    // TODO: Does this do response and request logging or do I need two seperate classes?
    public static class ApiExchangeLogger 
    {
        // TODO: What should this be for the generic code uploaded to github?
        private const string RequestsLogPath = "C:\\Repos\\acumatica-attachment-import\\RequestsLog.txt";

        /// <summary>
        /// Logs response to RequestsLog.txt file.
        /// </summary>
        public static void LogResponse(HttpResponseMessage responseMessage)
        {
            try
            {
                using (var writer = new StreamWriter(RequestsLogPath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine("Response");
                    writer.WriteLine("\tStatus code: " + responseMessage.StatusCode);
                    writer.WriteLine("\tContent: " + responseMessage?.Content.ReadAsStringAsync().Result);
                    writer.WriteLine("-----------------------------------------");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                }
            }
            catch { }
        }

        /// <summary>
        /// Logs request to RequestsLog.txt file.
        /// </summary>
        public static void LogRequest(HttpRequestMessage request)
        {
            try
            {
                using (var writer = new StreamWriter(RequestsLogPath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString());
                    writer.WriteLine("Request");
                    writer.WriteLine("\tMethod: " + request.Method);
                    string body = request.Content?.ReadAsStringAsync().Result;

                    writer.WriteLine("\tURL: " + request.RequestUri);
                    if (!String.IsNullOrEmpty(body))
                        writer.WriteLine("\tBody: " + body);
                    writer.WriteLine("-----------------------------------------");
                    writer.WriteLine();
                    writer.Flush();
                    writer.Close();
                }
            }
            catch { }

        }
    }
}