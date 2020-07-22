using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acumatica.Auth.Model;
using Acumatica.Default_18_200_001.Model;

namespace AcumaticaFilesImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Acumatica File Import");
            Console.WriteLine("---------------------");
            
            // find CSV File
            string csvLocation = GetFilePath();

            Console.WriteLine();
            Console.WriteLine();

            // find Acumatica Url
            string url = GetUrl();

            Console.WriteLine();
            Console.WriteLine();

            // Attempt to parse Csv
            Console.WriteLine("Parsing CSV...");

            var importer = new AcumaticaFileImporter(url, null);
            importer.FetchItemsFromCsv(csvLocation);

            do
            {
                Console.WriteLine("No items detected in csv, try again?");
                if (YNToBool())
                {
                    csvLocation = GetFilePath();
                    importer.FetchItemsFromCsv(csvLocation);
                }
                else
                {
                    return;
                }
            } 
            while (!importer.HasItems);

            Console.WriteLine();
            Console.WriteLine();

            importer.Initialize(GetCredentials());

            




        }

        private static string GetFilePath()
        {
            Console.WriteLine("Enter Csv File Location: ");
            string csvLocation;
            while(true)
            {
                csvLocation = Console.ReadLine();
                if (File.Exists(csvLocation))
                {
                    return csvLocation;
                }

                Console.WriteLine();
                Console.WriteLine($"No file detected at {csvLocation}");
            }
        }

        private static string GetUrl()
        {
            Console.WriteLine("Enter Acumatica Url: ");
            string url;
            Uri uri;
            while(true)
            {
                url = Console.ReadLine();
                if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
                {
                    return url;
                }

                Console.WriteLine($"Url {url} is not valid");
            }
        }

        private static bool YNToBool()
        {
            while (true)
            {
                string response = Console.ReadLine();
                if (string.Equals(response.ToUpper(), "Y"))
                {
                    return true;
                }
                else if (string.Equals(response.ToUpper(), "N"))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Response not valid");
                }
            }
        }

        private static Credentials GetCredentials()
        {
            Console.WriteLine("Enter Acumatica Credentials");
            Console.WriteLine("---------------------------");
            Console.WriteLine("Username:");
            string userName = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            Console.WriteLine("Company:");
            string company = Console.ReadLine();

            return new Credentials(userName, password, company);
        }
    }
}
