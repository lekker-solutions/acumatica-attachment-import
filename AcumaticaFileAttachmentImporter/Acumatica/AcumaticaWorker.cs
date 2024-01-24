
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.Default_22_200_001;
using Acumatica.Default_22_200_001.Api;
using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Api;
using Acumatica.RESTClient.Client;
using AcumaticaFilesImport.Files;
using AcumaticaFilesImport.Files.Csv;
using AcumaticaFilesImport.Logging;
using AcumaticaRestApiExample;
using Microsoft.Extensions.Logging;

namespace AcumaticaFilesImport.Acumatica
{
    // TODO: Refer to RestExample in AcumaticaRESTAPIClientForCSharp
    public class AcumaticaWorker
    {
        // TODO: Make Response and Request Logger classes
        public static void ImportFile(string siteUrl,
                                      string userName,
                                      string passWord,
                                      string tenant = null,
                                      string branch = null,
                                      string locale = null)
        {
            var authApi = new AuthApi(siteUrl, requestInterceptor: ApiExchangeLogger.LogRequest,
                responseInterceptor: ApiExchangeLogger.LogResponse);

            try
            {
                authApi.LogIn(userName, passWord, tenant, branch, locale);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //we use logout in finally block because we need to always logout, even if the request failed for some reason
                if (authApi.TryLogout())
                {
                    Console.WriteLine("Logged out successfully.");
                }
                else
                {
                    Console.WriteLine("An error occured during logout.");
                }
            }
        }

    }
}