using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace P0006.Models.ViewModels
{
    public class Tipos
    {
        public int IdTipo { get; set; }
        public string Descripcion { get; set; }
        public byte[] Imagen { get; set; }
        public string ImagenBase64 { get; set; }
        public bool Estatus { get; set; }
    }
}