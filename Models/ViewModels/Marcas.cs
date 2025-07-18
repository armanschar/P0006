using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace P0006.Metodos
{
    public class Marcas
    {
        public int IdMarca { get; set; }
        public string Descripcion { get; set; }
        public byte[] Imagen { get; set; }
        public bool Estatus { get; set; }
        public string ImagenBase64 { get; set; }
    }
}