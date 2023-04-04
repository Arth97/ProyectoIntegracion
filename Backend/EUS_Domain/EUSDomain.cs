namespace EUS_Domain
{
    public class EUSDomain
    {
        public string GetJson()
        {
            string path = @"C:\Users\WIMTRUCK 2\Desktop\IEI\DataDemo\JsonFile.json";

            //Initialize json
            var json = "";

            //Use StramReader to read Json file
            using (StreamReader jsonStream = File.OpenText(path))
            {
                json = jsonStream.ReadToEnd();
            }

            return json;
        }
    }
}