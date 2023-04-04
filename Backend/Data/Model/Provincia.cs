using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Provincia
    {
        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string? CodigoPostal { get; set; }
    }
}
