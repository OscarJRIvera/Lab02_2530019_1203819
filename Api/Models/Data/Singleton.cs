using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Huffman;

namespace Api.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public List<Compresion> Historial;
        public Huffman.Huffman Huff;
        public Dictionary<string, string> Nombres;
        private Singleton()
        {
            Huff = new Huffman.Huffman();
            Historial = new List<Compresion>();
            Nombres = new Dictionary<string, string>();
        }
        public static Singleton Instance
        {

            get
            {
                return _instance;
            }
        }
    }
}
