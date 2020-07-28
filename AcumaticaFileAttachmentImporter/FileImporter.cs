
using System.Collections.Generic;
using System.Linq;
using Acumatica.Auth.Model;
using AcumaticaFilesImport.Acumatica;
using AcumaticaFilesImport.Files;
using AcumaticaFilesImport.Files.Csv;
using Microsoft.Extensions.Logging;

namespace AcumaticaFilesImport
{
    public class FileImporter
    {
        public FileImporter(string siteUrl, ILogger logger)
        {
            _acumaticaWorker = new AcumaticaWorker(siteUrl, logger);
        }

        public void Initialize(Credentials credentials)
        {
            _acumaticaWorker.Initialize(credentials);
        }

        public void FetchItemsFromCsv(string filePath)
        {
            CsvWorker worker = new CsvWorker();
            _items = worker.ReadFromFile(filePath);
        }

        public void UploadFiles()
        {
            _acumaticaWorker.AttachToRecord(_items);
        }

        public void Close()
        {
            _acumaticaWorker.Dispose();
        }

        public bool HasItems
        {
            get => _items.Any();
        }

        public int ItemCount
        {
            get => _items.Count();
        }

        private IEnumerable<UploadItem> _items;
        private AcumaticaWorker _acumaticaWorker;
    }
}