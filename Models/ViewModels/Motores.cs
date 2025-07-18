using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace P0006.Models.ViewModels
{
    public class Motores
    {
        public int IdMotor { get; set; }
        public string Descripcion { get; set; }
        public string Combustible { get; set; }
        public string Transmision { get; set; }
    }
}