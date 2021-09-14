using System;
using System.Collections.Generic;
using System.Text;

namespace Huffman
{
    public class NodoHuffman
    {
        public NodoHuffman Contenedor { get; set; }
        public NodoHuffman Izquierda { get; set; }
        public NodoHuffman Derecha { get; set; }
        public string Prefijo { get; set; }
        public string huffnodo { get; set; }
        public byte Key { get; set; }
        public int Count { get; set; }
        public double Probabilidad { get; set; }
        public static int Compare_prob(NodoHuffman x, NodoHuffman y)
        {
            int r = x.Probabilidad.CompareTo(y.Probabilidad);
            return r;
        }
    }
    public class Ocurrencia
    {
        public List<NodoHuffman> Ocurrencias { get; set; }
        public int Total { get; set; }
    }

}
