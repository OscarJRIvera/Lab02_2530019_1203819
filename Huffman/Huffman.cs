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
        ArbolDePrioridad<NodoHuffman> HeapAux;


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
            NodoHuffman temp  = Heap.Peek();
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

        private void Prefijos2(string prefijos,NodoHuffman actual, NodoHuffman padre)
        {
            if (actual.Izquierda==null && actual.Derecha == null)
            {
                actual.Prefijo = prefijos;
            }
            if (actual.Izquierda != null)
            {
                Prefijos2(prefijos + "0",actual.Izquierda, actual);
                actual.Prefijo = prefijos;

            }
            if (actual.Derecha!=null)
            {
                Prefijos2(prefijos + "1",actual.Derecha, actual);
                actual.Prefijo = prefijos;
            }
        }
    }
}
