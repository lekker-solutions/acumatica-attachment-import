
using System;
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
        public FileImporter(AcumaticaWorker acWorker, CsvWorker csvWorker)
        {
            _acWorker = acWorker;
            _csvWorker = csvWorker;
        }

        public void FetchItemsFromCsv(string filePath)
        {
            _items = _csvWorker.ReadFromFile(filePath);
        }

        public void UploadFiles(string siteUrl, Credentials credentials, 
            Endpoint endpoint)
        {
            foreach (var item in _items)
            {
                _acWorker.ImportFile(siteUrl, credentials, endpoint, item);
            }
        }

        public Endpoint GetEndpoint()
        {
            if (_items == null ||
                !_items.Any())
            {
                throw new InvalidOperationException("No items are available in the CSV");
            }

            return _items.First().Endpoint;
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
        private AcumaticaWorker _acWorker;
        private CsvWorker _csvWorker;
    }
}