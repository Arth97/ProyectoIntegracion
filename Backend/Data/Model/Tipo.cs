using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Tipo
    {
        [Key]
        public int ID { get; set; }

        public string Nombre { get; set; }
    }
}
