using Data.DTO;
using Data.Model;
using Newtonsoft.Json;
using Repository;

namespace Domain
{
    public class EUSDomain
    {
        private readonly DataRepository dataRepository;
        private readonly TipoRepository tipoRepository;

        public EUSDomain(DataRepository dataRepository, TipoRepository tipoRepository)
        {
            this.dataRepository = dataRepository;
            this.tipoRepository = tipoRepository;
        }

        public static async Task<string> GetJson()
        {
            HttpClient client = new();
            var response = await client.GetAsync("https://localhost:7058/api/EUS");
            return await response.Content.ReadAsStringAsync();
        }

        public string StoreJson()
        {
            string json = GetJson().Result;
            List<JsonDto> jsonDtos = JsonConvert.DeserializeObject<List<JsonDto>>(json);
            string? errors = null;
            int cont = 0;
            foreach (JsonDto jsonDto in jsonDtos)
            {
                try
                {
                    //Add Provincia to BDD
                    Provincia provincia = new()
                    {
                        Nombre = jsonDto.Provincia,
                        CodigoPostal = jsonDto.CodigoPostal[..2]

                    };
                    dataRepository.AddProvincia(provincia);

                    //Add Localidad to BDD
                    Localidad localidad = new()
                    {
                        Nombre = jsonDto.Municipio,
                        CodigoPostal = jsonDto.CodigoPostal
                    };
                    Provincia provinciaToAdd = dataRepository.GetProvincia(provincia);
                    localidad.Provincia = provinciaToAdd;
                    dataRepository.AddLocalidad(localidad);

                    //Add EstablecimientoSanitario to BDD
                    EstablecimientoSanitario establecimientoSanitario = new()
                    {
                        Nombre = jsonDto.Nombre,
                        Direccion = jsonDto.Direccion,
                        CodigoPostal = jsonDto.CodigoPostal,
                        Latitud = jsonDto.LATWGS84,
                        Longitud = jsonDto.LONWGS84,
                        Telefono = jsonDto.Telefono.Replace(".", string.Empty).Replace(",", string.Empty),
                        Descripcion = jsonDto.CorreoElectronico
                    };
                    Localidad localidadToAdd = dataRepository.GetLocalidad(localidad);
                    establecimientoSanitario.Localidad = localidadToAdd;
                    var tipoId = tipoRepository.SetTipo(jsonDto.Nombre);
                    Tipo tipo = tipoRepository.GetTipo(tipoId);
                    establecimientoSanitario.Tipo = tipo;
                    dataRepository.AddEstablecimientoSanitario(establecimientoSanitario);
                    cont++;
                }
                catch (Exception e)
                {
                    errors += "Error al procesar " + jsonDto.Nombre + "\n";
                }
            }
            return "Procesados " + cont.ToString() + " Establecimientos Sanitarios de Euskadi" + "\n" + errors;
        }
    }
}
