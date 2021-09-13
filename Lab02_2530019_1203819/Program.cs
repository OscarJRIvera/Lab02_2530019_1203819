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

            string vari = "Cuando cuentes cuentos, cuenta cuántos cuentos cuentas; porque si no cuentas cuántos cuentos cuentas, nunca sabrás cuántos cuentos cuentas tú";

            Ocurrencia l = prueba.Recurrencia(vari);
            foreach (NodoHuffman ocurrencia in l.Ocurrencias)
            {
                Console.WriteLine("Letra: " + ocurrencia.Key + " Cantidad: " + ocurrencia.Count.ToString() + " Prob:" + ocurrencia.Probabilidad.ToString());
            }
            Console.WriteLine("Total :" + l.Total.ToString());
            prueba.AgregarHeap(l);
            prueba.AgregarHuffman();
            prueba.Prefijos();
            var d = prueba.CargarDiccionario();
            foreach (var t in d)
            {
                Console.WriteLine(t.Key + " - " + t.Value);
            }

            var f = prueba.CargarTabla();
            foreach (var t in f)
            {
                Console.WriteLine(t.Key + " - " + t.Value);
            }
            var x = prueba.EscribirTablaAscii(f);
            Console.WriteLine(x);

            var z = prueba.EscribirCompresión(d, vari);
            Console.WriteLine(z);

            prueba.AgregarHeap(prueba.HacerArbol(prueba.DescompresionRecurrencias(x)));
            prueba.AgregarHuffman();
            prueba.Prefijos();
            string aver = prueba.descomprimir(z);
            int hg = 0;
            Console.WriteLine("" + aver);
        }


    }
}
