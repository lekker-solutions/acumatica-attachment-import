
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32.SafeHandles;
using TinyCsvParser.Mapping;

namespace AcumaticaFilesImport.Files.Csv
{
    public class UploadItem 
    {
        public DocType DocType { get; set; }
        public string FilePath { get; set; }

        public string FileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fileName))
                {
                    return Path.GetFileName(FilePath);
                }

                else return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Key3 { get; set; }
        public string Key4 { get; set; }
        public string Key5 { get; set; }

        public FileStream Data
        {
            get
            {
                if (File.Exists(FilePath))
                {
                    return File.Open(FilePath, FileMode.Open);
                }
                
                throw new FileNotFoundException("Filepath defined in CSV not valid");
            }
        }

        public IEnumerable<string> KeyValues
        {
            get
            {
                List<string> values = new List<string>();

                if (!string.IsNullOrWhiteSpace(Key1)) values.Add(Key1);
                if (!string.IsNullOrWhiteSpace(Key2)) values.Add(Key2);
                if (!string.IsNullOrWhiteSpace(Key3)) values.Add(Key3);
                if (!string.IsNullOrWhiteSpace(Key4)) values.Add(Key4);
                if (!string.IsNullOrWhiteSpace(Key5)) values.Add(Key5);

                return values;
            }
        }

        private string _fileName;
    }
}