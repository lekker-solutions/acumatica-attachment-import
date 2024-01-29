
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.Default_22_200_001;
using Acumatica.Default_22_200_001.Api;
using Acumatica.Default_22_200_001.Model;
using Acumatica.Manufacturing_23_100_001.Api;
using Acumatica.RESTClient.Api;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;
using Acumatica.RESTClient.RootApi.Model;
using AcumaticaFilesImport.Files;
using AcumaticaFilesImport.Files.Csv;
using AcumaticaFilesImport.Logging;
using AcumaticaRestApiExample;
using Microsoft.Extensions.Logging;

namespace AcumaticaFilesImport.Acumatica
{
    // Refer to RestExample in AcumaticaRESTAPIClientForCSharp

    // TODO:
    // 1. Add method to this class to differentiate between api call type (i.e. SO, IN, etc.)
    // 2. Create endpoint for drawings under (Default or Manufacturing??)
    public class AcumaticaWorker
    {
        public void ImportFile(string siteUrl, Credentials credentials, Endpoint endpoint)
        {
            var authApi = new AuthApi(siteUrl, requestInterceptor: ApiExchangeLogger.LogRequest,
                responseInterceptor: ApiExchangeLogger.LogResponse);

            var userName = credentials.Name;
            var password = credentials.Password;
            var tenant = credentials.Tenant = null;
            var branch = credentials.Branch = null;
            var locale = credentials.Locale = null;

            try
            {
                authApi.LogIn(userName, password, tenant, branch, locale);

                // TODO: Add a method that switches case based on the Endpoint (DocType) and calls the PutDefaultEndpointFiles method with the correct Endpoint (DocType)

                #region Sample Sales Order Api Call

                var salesOrderApi = new SalesOrderApi(authApi.ApiClient);
                var order = salesOrderApi.GetByKeys(new List<string> { "SO", "SO005207" }, expand: "files");

                byte[] initialData = Encoding.UTF8.GetBytes("File");
                string fileName = "TestFile.txt";
                salesOrderApi.PutFile("SO/SO005207", fileName, initialData);

                #endregion


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
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

        private void PutDefaultEndpointFiles<T, U>(AuthApi authApi)
            where T: EntityAPI<U>
            where U: Entity, new()
        {
            //Use Refelection to create instance T
            Type type = typeof(T);

            // Get the constructor information
            ConstructorInfo constructorInfo = type.GetConstructor(new[] { authApi.GetType() });
            if (constructorInfo == null)
            {
                throw new InvalidOperationException("No matching constructor found");
            }
            // Create an instance of T with the specified constructor parameter
            T instance = (T)constructorInfo.Invoke(new[] { authApi });

            // Get keys and put into first string
            // throw error if first key doesn't exist
            // get keys from csv
            instance.PutFile("SO/SO005207", fileName, initialData);
        }

        // TODO: Pick up here.  Need method to set endpoint in import file
        private (EntityAPI<Entity>, Entity) SetEndpoint(Endpoint endpoint)
        {

            switch (endpoint)
            {
                case Endpoint.SalesOrder:
                    break;
                case Endpoint.ARInvoice: 
                    break;
                case Endpoint.BillOfMaterial:
                    break;
                default:
                    throw new DocTypeUnrecognizedException();

            }
        }


        //private Action<IEnumerable<string>, string, byte[]> GetAttachFunction(DocType docType)
        //{
        //    Action<IEnumerable<string>, string, byte[]> action;
        //    switch (docType)
        //    {
        //        case Endpoint.SalesOrder:
        //            action = GetApi<SalesOrderApi, SalesOrder>().PutFile;
        //            break;
        //        case Endpoint.ARInvoice:
        //            action = GetApi<InvoiceApi, Invoice>().PutFile;
        //            break;
        //        case Endpoint.BillOfMaterial:
        //            action = GetApi<DrawingApi, Drawing>().PutFile;
        //            break;
        //        default:
        //            throw new DocTypeUnrecognizedException();
        //    }

        //    return action;
        //}

    }
}