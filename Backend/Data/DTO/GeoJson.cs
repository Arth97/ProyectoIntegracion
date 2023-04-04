namespace Data.DTO
{
    public class GeoJson
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }

        public string[] Coordinates { get; set; }
    }

    public class Properties
    {
        public string Nombre { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string? CodigoPostal { get; set; }
        public string Tipo { get; set; }
    }
}
