
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcumaticaFilesImport.Files.Csv;
using TinyCsvParser;
using TinyCsvParser.Mapping;

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
            foreach (CsvMappingResult<UploadItem> result in _parser.ReadFromFile(csvPath, Encoding.UTF8))
            {
                if (result.IsValid) yield return result.Result;
            }
        }

        private CsvParser<UploadItem> _parser;
    }
}