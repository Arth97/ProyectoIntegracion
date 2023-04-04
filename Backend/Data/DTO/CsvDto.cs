using Newtonsoft.Json;

namespace Data.DTO
{
    public class CsvDto
    {
        [JsonProperty(PropertyName = "Província / Provincia")]
        public string Provincia;

        [JsonProperty(PropertyName = "Municipi / Municipio")]
        public string Municipio;

        [JsonProperty(PropertyName = "Centre / Centro")]
        public string Nombre;

        [JsonProperty(PropertyName = "Tipus_centre / Tipo_centro")]
        public string TipoDeCentro;

        [JsonProperty(PropertyName = "Règim /Régimen")]
        public string Regimen;

        [JsonProperty(PropertyName = "Adreça / Dirección")]
        public string Direccion;

        [JsonProperty(PropertyName = "Codi_província / Código_provincia")]
        public string CodigoProvincia;
    }
}
