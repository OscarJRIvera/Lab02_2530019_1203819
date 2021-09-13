using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ArbolDePrioridad;

namespace Huffman
{
    public class Huffman
    {
        Ocurrencia letras = new Ocurrencia();
        ArbolDePrioridad<NodoHuffman> Heap = new ArbolDePrioridad<NodoHuffman>(NodoHuffman.Compare_prob);

        public Ocurrencia Recurrencia(String texto)
        {
            List<NodoHuffman> l = texto.GroupBy(c => c).Select(c => new NodoHuffman { Key = c.Key.ToString(), Count = c.Count() }).ToList();
            letras.Ocurrencias = l;
            letras.Total = l.Sum(c => c.Count);
            foreach (NodoHuffman x in l)
            {
                double prob = (double)x.Count / (double)letras.Total;
                x.Probabilidad = prob;
            }
            return letras;
        }
        public void AgregarHeap(Ocurrencia letras)
        {
            foreach (NodoHuffman o in letras.Ocurrencias)
            {
                Heap.add(o);
            }
        }
        public void AgregarHuffman()
        {

            int i = 1;
            while (Heap.CantidadN() > 1)
            {
                NodoHuffman NodoH = new NodoHuffman();
                NodoH.Izquierda = Heap.Remove();
                NodoH.Derecha = Heap.Remove();
                Double SumProb1 = NodoH.Derecha.Probabilidad + NodoH.Izquierda.Probabilidad;
                NodoH.Probabilidad = SumProb1;
                NodoH.Key = "N" + i.ToString();
                i++;
                Heap.add(NodoH);
            }

        }
        public void Prefijos()
        {
            string prefijo = "";
            NodoHuffman temp = Heap.Peek();
            if (temp.Izquierda != null)
            {
                prefijo = "0";
                Prefijos2(prefijo, temp.Izquierda, null);
                if (temp.Derecha != null)
                {
                    prefijo = "1";
                    Prefijos2(prefijo, temp.Derecha, null);
                }
            }
        }
        private void Prefijos2(string prefijos, NodoHuffman actual, NodoHuffman padre)
        {
            if (actual.Izquierda == null && actual.Derecha == null)
            {
                actual.Prefijo = prefijos;
            }
            if (actual.Izquierda != null)
            {
                Prefijos2(prefijos + "0", actual.Izquierda, actual);
                actual.Prefijo = prefijos;

            }
            if (actual.Derecha != null)
            {
                Prefijos2(prefijos + "1", actual.Derecha, actual);
                actual.Prefijo = prefijos;
            }
        }
        public Dictionary<string, string> CargarDiccionario()
        {
            Dictionary<string, string> Diccionario = new Dictionary<string, string>();
            NodoHuffman temp = Heap.Peek();

            if (temp.Izquierda != null)
            {
                CargarDiccionario(ref Diccionario, temp.Izquierda);
            }
            if (temp.Derecha != null)
            {
                CargarDiccionario(ref Diccionario, temp.Derecha);
            }
            return Diccionario;
        }
        private void CargarDiccionario(ref Dictionary<string, string> Diccionario, NodoHuffman actual)
        {
            if (actual.Izquierda == null && actual.Derecha == null)
            {
                Diccionario.Add(actual.Key, actual.Prefijo);
            }
            if (actual.Izquierda != null)
            {
                CargarDiccionario(ref Diccionario, actual.Izquierda);
            }
            if (actual.Derecha != null)
            {
                CargarDiccionario(ref Diccionario, actual.Derecha);
            }
        }
        public Dictionary<string, int> CargarTabla()
        {
            Dictionary<string, int> Diccionario = new Dictionary<string, int>();
            NodoHuffman temp = Heap.Peek();

            if (temp.Izquierda != null)
            {
                CargarTabla(ref Diccionario, temp.Izquierda);
            }
            if (temp.Derecha != null)
            {
                CargarTabla(ref Diccionario, temp.Derecha);
            }
            return Diccionario;
        }
        private void CargarTabla(ref Dictionary<string, int> Diccionario, NodoHuffman actual)
        {
            if (actual.Izquierda == null && actual.Derecha == null)
            {
                Diccionario.Add(actual.Key, actual.Count);
            }
            if (actual.Izquierda != null)
            {
                CargarTabla(ref Diccionario, actual.Izquierda);
            }
            if (actual.Derecha != null)
            {
                CargarTabla(ref Diccionario, actual.Derecha);
            }
        }
        public string EscribirTablaAscii(Dictionary<string, int> Diccionario)
        {
            string Texto = "4";
            foreach (var item in Diccionario)
            {
                Texto += item.Key;
                byte[] Frecuencia = BitConverter.GetBytes(item.Value);
                foreach (var x in Frecuencia)
                {
                    char c = Convert.ToChar(x);
                    Texto += c;
                }
            }
            return Texto;
        }
        public string EscribirCompresión(Dictionary<string, string> Diccionario, string texto)
        {
            string Compresión = "";
            string Binario = "";
            foreach (var c in texto)
            {
                string x = c.ToString();
                Binario += Diccionario[x]; //junta los 1 y 0 
            }
            int extras = Binario.Length % 8;
            Compresión += extras.ToString();
            for (int i = 0; i < extras; i++)//agrega los 0 que faltan
            {
                Binario += "0";
            }
            string temp = "";
            foreach (var item in Binario)
            {
                temp += item;
                if (temp.Length == 8)
                {
                    byte b = Convert.ToByte(temp, 2);
                    Compresión += Convert.ToChar(b);
                    temp = "";
                }
            }
            return Compresión;
        }



    }
}
