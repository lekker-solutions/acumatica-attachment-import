
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

        public void Initialize(string url, Credentials credentials)
        {
            _acWorker.Initialize(url, credentials);
        }

        public void FetchItemsFromCsv(string filePath)
        {
            _items = _csvWorker.ReadFromFile(filePath);
        }

        public void UploadFiles()
        {
            _acWorker.AttachToRecord(_items);
        }

        public void Close()
        {
            _acWorker.Dispose();
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