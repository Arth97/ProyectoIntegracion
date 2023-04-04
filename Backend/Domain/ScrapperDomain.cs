using Data.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using Repository;
using System.Text.RegularExpressions;

namespace Domain
{
    public class ScraperDomain
    {
        private readonly DataRepository dataRepository;

        public ScraperDomain(DataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        //Scrapper
        public string ScrapingData()
        {
            //Scraping for Lat & Lon
            List<EstablecimientoSanitario> establecimientosSanitarios = dataRepository.GetEstablecimientoSanitarioNoLatLon();
            if (establecimientosSanitarios.Count > 0)
            {
                IWebDriver driver = GetDriver();
                driver.Navigate().GoToUrl("https://www.coordenadas-gps.com/");
                foreach (EstablecimientoSanitario establecimientoSanitario in establecimientosSanitarios)
                {
                    var (lat, longi) = ScrapingForLatLon(driver, establecimientoSanitario);

                    establecimientoSanitario.Latitud = lat;
                    establecimientoSanitario.Longitud = longi;
                    dataRepository.UpdateEstablecimientoSanitario(establecimientoSanitario);
                }
                driver.Close();
            }
            List<EstablecimientoSanitario> establecimientosSanitariosError = new();

            //Scraping for CodPostal
            establecimientosSanitarios = dataRepository.GetEstablecimientoSanitarioNoCodPostal();
            if (establecimientosSanitarios.Count > 0)
            {
                IWebDriver driver = GetDriver();
                driver.Navigate().GoToUrl("https://www.coordenadas-gps.com/");
                foreach (EstablecimientoSanitario establecimientoSanitario in establecimientosSanitarios)
                {
                    var codigoPostal = ScrapingForCodigoPostal(driver, establecimientoSanitario);
                    var processedCodPostal = ProcessCodigoPostal(codigoPostal);
                    if (processedCodPostal == null)
                    {
                        establecimientosSanitarios.Add(establecimientoSanitario);
                    }
                    else
                    {
                        establecimientoSanitario.CodigoPostal = processedCodPostal;
                        establecimientoSanitario.Localidad.CodigoPostal = processedCodPostal;
                        establecimientoSanitario.Localidad.Provincia.CodigoPostal = processedCodPostal[..2];
                        dataRepository.UpdateEstablecimientoSanitario(establecimientoSanitario);
                        dataRepository.UpdateLocalidad(establecimientoSanitario.Localidad, establecimientoSanitario);
                        dataRepository.UpdateProvincia(establecimientoSanitario.Localidad.Provincia, establecimientoSanitario.Localidad);
                    }                 
                }
                driver.Close();
            }
            string codPostalErrors = "";
            foreach (EstablecimientoSanitario establecimientoSanitario in establecimientosSanitariosError)
            {
                 codPostalErrors = codPostalErrors + "\n" + establecimientoSanitario.Nombre + ": Error en código postal";
            }
            return codPostalErrors;
        }

        //Scraper to get Latitude & Longitude
        public (string, string) ScrapingForLatLon(IWebDriver driver, EstablecimientoSanitario eS)
        {
            try
            {
                var direccion = driver.FindElement(By.Id("address"));
                Actions actions = new Actions(driver);

                direccion.Clear();
                actions.MoveToElement(direccion).Click().Perform();
                direccion.SendKeys(eS.Direccion + ", " + eS.Localidad.Nombre);
                var direccionButton = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[3]/div[1]/form[1]/div[2]/div/button"));
                if (direccionButton.Text == "Obtener Coordenadas GPS")
                    actions.MoveToElement(direccionButton).Click().Perform();
                var latitud = driver.FindElement(By.Id("latitude"));
                var longitud = driver.FindElement(By.Id("longitude"));

                Thread.Sleep(1000);
                actions.MoveToElement(latitud).Click().Perform();
                var lat = latitud.GetAttribute("value");
                actions.MoveToElement(longitud).Click().Perform();
                var longi = longitud.GetAttribute("value");

                return (lat, longi);
            }
            catch
            {
                var direccion = driver.FindElement(By.Id("address"));
                Actions actions = new Actions(driver);

                direccion.Clear();
                actions.MoveToElement(direccion).Click().Perform();
                direccion.SendKeys(eS.Localidad.Nombre + ", " + eS.Localidad.Provincia.Nombre);
                var direccionButton = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[3]/div[1]/form[1]/div[2]/div/button"));
                if (direccionButton.Text == "Obtener Coordenadas GPS")
                    actions.MoveToElement(direccionButton).Click().Perform();
                var latitud = driver.FindElement(By.Id("latitude"));
                var longitud = driver.FindElement(By.Id("longitude"));

                Thread.Sleep(1000);
                actions.MoveToElement(latitud).Click().Perform();
                var lat = latitud.GetAttribute("value");
                actions.MoveToElement(longitud).Click().Perform();
                var longi = longitud.GetAttribute("value");

                return (lat, longi);
            }
        }

        //Main scraper to get CodigoPostal
        public string ScrapingForCodigoPostal(IWebDriver driver, EstablecimientoSanitario eS)
        {
            var direccion = driver.FindElement(By.Id("address"));
            Actions actions = new Actions(driver);

            direccion.Clear();
            actions.MoveToElement(direccion).Click().Perform();

            var latitud = driver.FindElement(By.Id("latitude"));
            latitud.Clear();
            actions.MoveToElement(latitud).Click().Perform();
            latitud.SendKeys(eS.Latitud);


            var longitud = driver.FindElement(By.Id("longitude"));
            longitud.Clear();
            actions.MoveToElement(longitud).Click().Perform();
            longitud.SendKeys(eS.Longitud);

            var latLonButton = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[3]/div[1]/form[2]/div[3]/div/button"));
            if (latLonButton.Text == "Obtener Dirección")
                actions.MoveToElement(latLonButton).Click().Perform();

            Thread.Sleep(1000);
            actions.MoveToElement(direccion).Click().Perform();
            var codigoPostal = direccion.GetAttribute("value");

            return codigoPostal;
        }

        //NOT IN USE
        // Alternative scraper to get CodigoPostal
        public string ScrapingForCodigoPostalAlternative(IWebDriver driver, EstablecimientoSanitario eS)
        {
            var direccion = driver.FindElement(By.Id("address"));
            Actions actions = new Actions(driver);

            direccion.Clear();
            actions.MoveToElement(direccion).Click().Perform();
            direccion.SendKeys(eS.Localidad.Nombre);
            var direccionButton = driver.FindElement(By.XPath("/html/body/div[2]/div[2]/div[3]/div[1]/form[1]/div[2]/div/button"));
            if (direccionButton.Text == "Obtener Coordenadas GPS")
                actions.MoveToElement(direccionButton).Click().Perform();
            Thread.Sleep(1000);
            actions.MoveToElement(direccion).Click().Perform();
            var codigoPostal = direccion.GetAttribute("value");

            return codigoPostal;
        }

        //Get CodigoPostal from string
        public string ProcessCodigoPostal(string texto)
        {
            Match m = Regex.Match(texto, "([0-9]{5})");
            string num = string.Empty;

            if (m.Success)
            {
                num = m.Value;
            }
            if (num == "")
                return null;
            return num;
        }

        //Scrapper configuration
        public IWebDriver GetDriver()
        {
            var user_agent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument($"user_agent={user_agent}");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArguments("start-maximized");
            options.AddArguments("--deny-permission-prompts");
            options.AddArguments("--incognito");
            IWebDriver driver = new ChromeDriver(Directory.GetCurrentDirectory(), options);
            return driver;
        }
    }
}
