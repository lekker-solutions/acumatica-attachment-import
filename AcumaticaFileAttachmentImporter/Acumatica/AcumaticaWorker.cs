
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
    public class AcumaticaWorker
    {
        public void ImportFile(string siteUrl, Credentials credentials, Endpoint endpoint, UploadItem item)
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

                byte[] initialData = File.ReadAllBytes(item.FilePath);
                IEnumerable<string> fileId;
                string fileName;

                switch (endpoint)
                {
                    case Endpoint.SalesOrder:

                        string orderNbr = item.Key1;
                        string orderType = item.Key2;

                        fileId = new List<string> { orderType, orderNbr };

                        var salesOrderApi = new SalesOrderApi(authApi.ApiClient);
                        var order = salesOrderApi.GetByKeys(fileId, expand: "files");

                        salesOrderApi.PutFile(fileId, Path.GetFileName(item.FilePath), initialData);

                        break;

                    case Endpoint.ARInvoice:

                        string docType = item.Key1;
                        string refNbr = item.Key2;

                        fileId = new List<string> { docType, refNbr };

                        var arInvoiceApi = new SalesInvoiceApi(authApi.ApiClient);
                        var invoice = arInvoiceApi.GetByKeys(fileId, expand: "files");

                        arInvoiceApi.PutFile(fileId, Path.GetFileName(item.FilePath), initialData);

                        break;

                    case Endpoint.BillOfMaterial:

                        string bomId = item.Key1;
                        string revisionId = item.Key2;

                        fileId = new List<string> { bomId, revisionId };

                        var bomApi = new BillOfMaterialApi(authApi.ApiClient);
                        var bom = bomApi.GetByKeys(fileId, expand: "files");

                        bomApi.PutFile(fileId, Path.GetFileName(item.FilePath), initialData);

                        break;

                    case Endpoint.InventoryItem:

                        string inventoryId = item.Key1;

                        fileId = new List<string> () { inventoryId };  

                        var inventoryItemApi = new StockItemApi(authApi.ApiClient);
                        var inventoryItem = inventoryItemApi.GetByKeys(fileId, expand: "files");

                        inventoryItemApi.PutFile(fileId, Path.GetFileName(item.FilePath), initialData);

                        break;

                    default:
                        throw new DocTypeUnrecognizedException();

                }
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
    }
}