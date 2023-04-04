using Newtonsoft.Json;
using System.Text;

namespace CV_Domain
{
    public class CVDomain
    {
        public string ConvertCsvToJson()
        {
            var csv = new List<string[]>();
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            var lines = System.IO.File.ReadAllLines(@"C:\Users\WIMTRUCK 2\Desktop\UPV\IEI\DataDemo\CsvFile.csv", iso);

            // Split string to get first line, header line as JSON properties
            var properties = lines[0].Split(';');

            lines = lines.Skip(1).ToArray();
            // Loop through all lines and add it in list as string
            foreach (string line in lines)
            {
                csv.Add(line.Split(';'));
            }

            var listObjResult = new List<Dictionary<string, string>>();

            // Loop all remaining lines, except header so starting it from 1 instead of 0
            for (int i = 1; i < lines.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            // Convert dictionary into JSON
            var json = JsonConvert.SerializeObject(listObjResult);
            return json;
        }
    }
}