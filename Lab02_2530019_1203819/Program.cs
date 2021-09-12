using System;
using Huffman;
using System.Collections.Generic;

namespace Lab02_2530019_1203819
{
    class Program
    {

        static void Main(string[] args)
        {
            Huffman.Huffman prueba = new Huffman.Huffman();

            //string vari = "Cuando cuentes cuentos, cuenta cuántos cuentos cuentas; porque si " +
            //    "no cuentas cuántos cuentos cuentas, nunca sabrás cuántos cuentos cuentas tú";

            string vari = "Diana ";

            Ocurrencia l = prueba.Recurrencia(vari);
            foreach (NodoHuffman ocurrencia in l.Ocurrencias)
            {
                Console.WriteLine("Letra: " + ocurrencia.Key + " Cantidad: " + ocurrencia.Count.ToString() + " Prob:" + ocurrencia.Probabilidad.ToString());
            }
            Console.WriteLine("Total :" + l.Total.ToString());
            prueba.AgregarHeap(l);
            prueba.AgregarHuffman();
            prueba.Prefijos();



        }

    }
}
