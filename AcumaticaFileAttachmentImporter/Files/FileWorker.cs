
using System.Collections.Generic;
using System.Text;
using AcumaticaFilesImport.Files.Csv;
using TinyCsvParser;

namespace AcumaticaFilesImport.Files
{
    public class CsvWorker
    {
        public CsvWorker()
        {
            CsvParserOptions options = new CsvParserOptions(false,',');
            _parser = new CsvParser<UploadItem>(options, new CsvAttachmentRecordMapping());
        }

        public IEnumerable<UploadItem> ReadFromFile(string csvPath)
        {
            return (IEnumerable<UploadItem>)_parser.ReadFromFile(csvPath, Encoding.UTF8);
        }

        private CsvParser<UploadItem> _parser;
    }
}