using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace P0006.Models.ViewModels
{
    public class ArticuloFotos
    {
        public int IdCliente { get; set; }
        public int IdArticulo { get; set; }
        public int SecPhoto { get; set; }
        public byte[] FOTO { get; set; }
    }
}