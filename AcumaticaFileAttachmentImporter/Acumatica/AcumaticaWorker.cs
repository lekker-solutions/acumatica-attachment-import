
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.Default_22_200_001;
using Acumatica.Default_22_200_001.Api;
using Acumatica.Default_22_200_001.Model;
using Acumatica.Default_18_200_001.Api;
using Acumatica.Default_18_200_001.Model;
using Acumatica.RESTClient.Api;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.Model;
using AcumaticaFilesImport.Files;
using AcumaticaFilesImport.Files.Csv;
using AcumaticaFilesImport.Logging;
using Microsoft.Extensions.Logging;

namespace AcumaticaFilesImport.Acumatica
{
    public class AcumaticaWorker : IDisposable
    {
        public const string DefaultEndpoint = "/entity/Default/18.200.001/";
        
        public AcumaticaWorker(ILogger<AcumaticaWorker> logger)
        {
            _logger = logger;
            _apis = new List<object>();
        }

        private List<object> _apis;
        private AuthApi _auth;
        private ILogger<AcumaticaWorker> _logger;
        private Configuration _config;

        public void Initialize(string siteUrl, Credentials credentials)
        {
            try
            {
                _auth = new AuthApi(siteUrl);
                var cookieContainer = new CookieContainer();
                _auth.Configuration.ApiClient.RestClient.CookieContainer = cookieContainer;
                _auth.

                _config = new Configuration(siteUrl + DefaultEndpoint);
                _config.ApiClient.RestClient.CookieContainer = cookieContainer;

                _auth.AuthLogin(credentials);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was an error when attempting to log on");
                Dispose();
                throw;
            }
        }

        public void AttachToRecord(IEnumerable<UploadItem> items)
        {
            foreach (UploadItem item in items)
            {
                AttachToRecord(item);
            }
        }

        public void AttachToRecord(UploadItem item)
        {
            try
            {
                Action<IEnumerable<string>, string, byte[]> attachFileMethod = GetAttachFunction(item.DocType);

                using (MemoryStream stream = new MemoryStream())
                {
                    item.Data.CopyTo(stream);
                    attachFileMethod.Invoke(item.KeyValues, item.FileName, stream.ToArray());
                }

                _logger.LogFileAttached(item);
            }
            catch (DocTypeUnrecognizedException)
            {
                _logger.LogDocTypeUnsupported(item);
            }
            catch (ApiException a)
            {
                if (a.ServerException.ExceptionType.Contains("NoEntitySatisfiesTheConditionException"))
                {
                    _logger.LogRecordMissing(item);
                }
                else
                {
                    _logger.LogError(a, "There was an unhandled exception");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "There was an unhandled exception");
            }
        }

        private Action<IEnumerable<string>, string, byte[]> GetAttachFunction(DocType docType)
        {
            Action<IEnumerable<string>, string, byte[]> action;
            switch (docType)
            {
                case DocType.SalesOrder:
                    action = GetApi<SalesOrderApi, SalesOrder>().PutFile;
                    break;
                case DocType.ARInvoice:
                    action = GetApi<InvoiceApi, Invoice>().PutFile;
                    break;
                default:
                    throw new DocTypeUnrecognizedException();
            }

            return action;
        }

        private EntityAPI<W> GetApi<T, W>() where T : EntityAPI<W>
        where W : Entity
        {
            InvoiceApi api = new InvoiceApi(_config);
            var existingApi = _apis.Find(a => a.GetType() == typeof(T));
            return (EntityAPI<W>)(existingApi ?? typeof(T)
               .GetConstructor(
                new Type[]
                {
                    typeof(Configuration)
                })
               .Invoke(new[]
                {
                    _config
                }));
        }

        public void Dispose()
        {
            _auth.AuthLogout();
        }
    }
}