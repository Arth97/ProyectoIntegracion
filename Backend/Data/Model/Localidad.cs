﻿using System.ComponentModel.DataAnnotations;

namespace Data.Model
{
    public class Localidad
    {
        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string? CodigoPostal { get; set; }
        public Provincia Provincia { get; set; }
        public int ProvinciaId { get; set; }
    }
}
