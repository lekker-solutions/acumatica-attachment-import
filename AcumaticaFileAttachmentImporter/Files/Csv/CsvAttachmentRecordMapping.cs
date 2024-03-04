
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace AcumaticaFilesImport.Files.Csv
{
    public class CsvAttachmentRecordMapping : CsvMapping<UploadItem>
    {
        public CsvAttachmentRecordMapping() : base()
        {
            MapProperty(0, x => x.Endpoint, new EnumConverter<Endpoint>(true));
            MapProperty(1, x => x.FilePath);
            MapProperty(2, x => x.FileName);

            MapProperty(3, x => x.Key1);
            MapProperty(4, x => x.Key2);
            MapProperty(5, x => x.Key3);
            MapProperty(6, x => x.Key4);
            MapProperty(7, x => x.Key5);
        }
    }
}