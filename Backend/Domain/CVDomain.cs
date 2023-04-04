using Data.DTO;
using Data.Model;
using Newtonsoft.Json;
using Repository;

namespace Domain
{
    public class CVDomain
    {
        private readonly DataRepository dataRepository;
        private readonly TipoRepository tipoRepository;

        public CVDomain(DataRepository dataRepository, TipoRepository tipoRepository)
        {
            this.dataRepository = dataRepository;
            this.tipoRepository = tipoRepository;
        }

        public static async Task<string> GetJson()
        {
            HttpClient client = new();
            var response = await client.GetAsync("https://localhost:7072/api/CV");
            return await response.Content.ReadAsStringAsync();
        }

        public string StoreJson()
        {
            string json = GetJson().Result;
            List<CsvDto> csvDtos = JsonConvert.DeserializeObject<List<CsvDto>>(json);

            string? errors = null;
            int cont = 0;
            foreach (CsvDto csvDto in csvDtos)
            {
                try
                {
                    //Add Provincia to BDD
                    Provincia provincia = new()
                    {
                        Nombre = csvDto.Provincia,
                        CodigoPostal = csvDto.CodigoProvincia

                    };
                    dataRepository.AddProvincia(provincia);

                    //Add Localidad to BDD
                    Localidad localidad = new()
                    {
                        Nombre = csvDto.Municipio
                        //CodigoPostal -> Scraper
                    };
                    Provincia provinciaToAdd = dataRepository.GetProvincia(provincia);
                    localidad.Provincia = provinciaToAdd;
                    dataRepository.AddLocalidad(localidad);

                    //Add EstablecimientoSanitario to BDD
                    if (csvDto.Direccion.Contains("S/N"))
                        csvDto.Direccion = csvDto.Direccion.Split("S/N")[0] + "S/N";
                    EstablecimientoSanitario establecimientoSanitario = new()
                    {
                        Nombre = csvDto.Nombre,
                        Direccion = csvDto.Direccion,
                        Descripcion = "Régimen " + csvDto.Regimen.ToLower()
                        //CodigoPostal -> Scraper
                        //Latitud -> Scraper
                        //Longitud -> Scraper
                    };
                    Localidad localidadToAdd = dataRepository.GetLocalidad(localidad);
                    establecimientoSanitario.Localidad = localidadToAdd;
                    var tipoId = tipoRepository.SetTipo(csvDto.TipoDeCentro);
                    Tipo tipo = tipoRepository.GetTipo(tipoId);
                    establecimientoSanitario.Tipo = tipo;
                    dataRepository.AddEstablecimientoSanitario(establecimientoSanitario);
                    cont++;
                }
                catch (Exception e)
                {
                    errors += "Error al procesar " + csvDto.Nombre + "\n";
                }
            }
            return "Procesados " + cont.ToString() + " Establecimientos Sanitarios de la ComunidadValenciana" + "\n" + errors;
        }
    }
}