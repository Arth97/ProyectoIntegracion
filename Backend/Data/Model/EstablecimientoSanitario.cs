using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class EstablecimientoSanitario
    {
        [Key]
        public int ID { get; set; }
        public string? Nombre { get; set; }
        public Tipo Tipo { get; set; }
        public int TipoId { get; set; }
        public string? Direccion { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Longitud { get; set; }
        public string? Latitud { get; set; }
        public string? Telefono { get; set; }
        public string? Descripcion { get; set; }
        public Localidad Localidad { get; set; }
        public int LocalidadId { get; set; }
    }
}
