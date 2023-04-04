using Data.DTO;
using Data.Model;
using Newtonsoft.Json;
using Repository;
using static Data.DTO.XmlData;

namespace Domain
{
    public class IBDomain
    {
        private readonly DataRepository dataRepository;
        private readonly TipoRepository tipoRepository;

        public IBDomain(DataRepository dataRepository, TipoRepository tipoRepository)
        {
            this.dataRepository = dataRepository;
            this.tipoRepository = tipoRepository;
        }

        public static async Task<string> GetJson()
        {
            HttpClient client = new();
            var response = await client.GetAsync("https://localhost:7051/api/IB");
            return await response.Content.ReadAsStringAsync();
        }

        public string StoreJson()
        {
            string json = GetJson().Result;

            //Crear objeto XmlDto
            XmlFile xmlFile = JsonConvert.DeserializeObject<XmlFile>(json); 
            List<XmlDto> xmlDtos = xmlFile.response.row.row;
            string? errors = null;
            int cont = 0;
            foreach (XmlDto xmlDto in xmlDtos)
            {
                try
                {
                    //Add Provincia to BDD
                    Provincia provincia = new()
                    {
                        Nombre = "Illes Balears",
                        //CodigoPostal -> Scrapper

                    };
                    dataRepository.AddProvincia(provincia);

                    //Add Localidad to BDD
                    Localidad localidad = new()
                    {
                        Nombre = xmlDto.Municipi,
                        //CodigoPostal -> Scrapper
                    };
                    Provincia provinciaToAdd = dataRepository.GetProvincia(provincia);
                    localidad.Provincia = provinciaToAdd;
                    dataRepository.AddLocalidad(localidad);

                    //Add EstablecimientoSanitario to BDD
                    EstablecimientoSanitario establecimientoSanitario = new()
                    {
                        Nombre = xmlDto.Nom,
                        Direccion = xmlDto.Adreca,
                        //CodigoPostal -> Scrapper
                        Latitud = xmlDto.Lat,
                        Longitud = xmlDto.Long
                    };
                    Localidad localidadToAdd = dataRepository.GetLocalidad(localidad);
                    establecimientoSanitario.Localidad = localidadToAdd;
                    var tipoId = tipoRepository.SetTipo(xmlDto.Funcio);
                    Tipo tipo = tipoRepository.GetTipo(tipoId);
                    establecimientoSanitario.Tipo = tipo;
                    dataRepository.AddEstablecimientoSanitario(establecimientoSanitario);
                    cont++;
                }
                catch (Exception e)
                {
                    errors += "Error al procesar " + xmlDto.Nom + "\n";
                }  
            }
            return "Procesados " + cont.ToString() + " Establecimientos Sanitarios de las Islas Baleares" + "\n" + errors;
        }
    }
}
