﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Huffman;
using LZW;

namespace Api.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public List<Compresion> Historial;
        public Huffman.Huffman Huff;
        public Dictionary<string, string> Nombres;
        public Dictionary<string, Compresion> historial;
        public LZW.LZW lzw;
        private Singleton()
        {
            Huff = new Huffman.Huffman();
            Historial = new List<Compresion>();
            Nombres = new Dictionary<string, string>();
            historial = new Dictionary<string, Compresion>();
            lzw = new LZW.LZW();
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
