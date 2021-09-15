using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Compresion
    {
        public int id { get; set; }
        public string NombreOriginal { get; set; }
        public string NombreComprimido { get; set; }
        public string RutaComprimido { get; set; }
        public double RazonDeCompresion { get; set; }
        public double FactorDeCompresion { get; set; }
        public string PorcentajeDeReduccion { get; set; }
    }
}
