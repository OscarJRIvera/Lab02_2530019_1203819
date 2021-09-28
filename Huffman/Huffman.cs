using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ArbolDePrioridad;
using System.IO;

namespace Huffman
{
    public class Huffman
    {
        Ocurrencia letras = new Ocurrencia();
        ArbolDePrioridad<NodoHuffman> Heap = new ArbolDePrioridad<NodoHuffman>(NodoHuffman.Compare_prob);
        int maximo = new int();
        double bytes = new double();
        Int64 tamaño = 0;
        int cuantoceros = 0;
        string tempbina = "";
        Dictionary<byte, int> prueba = new Dictionary<byte, int>();
        public void Recurrencia(String Ruta, String Ruta2)
        {
            Dictionary<byte, int> texto = new Dictionary<byte, int>();
            FileStream archivoo = new FileStream(Ruta, FileMode.Open);
            using var leer = new BinaryReader(archivoo);
            while (archivoo.Position < archivoo.Length)
            {
                var buffer = leer.ReadBytes(100);
                foreach (var y in buffer)
                {
                    if (texto.ContainsKey(y))
                    {
                        texto[y]++;
                    }
                    else
                    {
                        texto.Add(y, 1);
                    }
                }
            }

            archivoo.Close();
            List<NodoHuffman> l = new List<NodoHuffman>();
            maximo = 0;
            foreach (var x in texto)
            {
                if (x.Value > maximo)
                {
                    maximo = x.Value;
                }
                NodoHuffman temp = new NodoHuffman();
                temp.Key = x.Key;
                temp.Count = x.Value;
                l.Add(temp);
            }
            letras.Ocurrencias = l;
            letras.Total = l.Sum(c => c.Count);
            foreach (NodoHuffman x in l)
            {
                double prob = (double)x.Count / (double)letras.Total;
                x.Probabilidad = prob;
            }
            AgregarHeap(letras);
            AgregarHuffman();
            Prefijos();
            bytes = (Convert.ToDouble(maximo) / 255);
            int f = Convert.ToInt32(bytes);
            if (bytes > Convert.ToDouble(f))
            {
                f++;
            }
            byte[] comprimido = EscribirTablaAscii(texto, f);
            prueba = texto;
            Dictionary<byte, string> prefijos = new Dictionary<byte, string>();
            foreach (NodoHuffman y in letras.Ocurrencias)
            {
                prefijos.Add(y.Key, y.Prefijo);
            }

            FileStream archivoC = new FileStream(Ruta2, FileMode.OpenOrCreate);
            archivoC.Write(comprimido);
            FileStream archivoo2 = new FileStream(Ruta, FileMode.Open);
            using var leer2 = new BinaryReader(archivoo2);
            bool ultimoparte = false;
            tamaño = 0;
            cuantoceros = 0;
            while (archivoo2.Position < archivoo2.Length)
            {
                var buffer = leer2.ReadBytes(100);
                if (archivoo2.Position >= archivoo2.Length)
                {
                    ultimoparte = true;
                }
                byte[] comprimido2 = EscribirCompresión(prefijos, buffer, ultimoparte);
                archivoC.Write(comprimido2);
            }
            archivoC.Flush();
            archivoo2.Close();
            archivoC.Close();
        }
        public void AgregarHeap(Ocurrencia letras)
        {
            Heap = new ArbolDePrioridad<NodoHuffman>(NodoHuffman.Compare_prob);
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
                NodoH.huffnodo = "N" + i.ToString();
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
                foreach (NodoHuffman y in letras.Ocurrencias)
                {
                    if (y.Key == actual.Key)
                    {
                        y.Prefijo = prefijos;
                    }
                }
            }
            if (actual.Izquierda != null)
            {
                Prefijos2(prefijos + "0", actual.Izquierda, actual);
                foreach (NodoHuffman y in letras.Ocurrencias)
                {
                    if (y.Key == actual.Key)
                    {
                        y.Prefijo = prefijos;
                    }
                }
            }
            if (actual.Derecha != null)
            {
                Prefijos2(prefijos + "1", actual.Derecha, actual);
                foreach (NodoHuffman y in letras.Ocurrencias)
                {
                    if (y.Key == actual.Key)
                    {
                        y.Prefijo = prefijos;
                    }
                }
            }
        }

        public byte[] EscribirTablaAscii(Dictionary<byte, int> Diccionario, int f)
        {
            int numero = f;
            int contadorbytes = 0;
            while (numero > 0)
            {
                contadorbytes++;
                numero -= 255;
            }
            int canti = f;
            byte[] Frecuencia1 = new byte[contadorbytes];
            int posi1 = 0;
            while (canti != 0)
            {
                if (canti > 255)
                {
                    Frecuencia1[posi1] = 255;
                    canti -= 255;
                }
                else
                {
                    Frecuencia1[posi1] = (byte)canti;
                    canti = 0;
                }
                posi1++;
            }

            byte[] Texto1 = new byte[(Diccionario.Count * f) + Diccionario.Count + 3 + contadorbytes];
            int h = 0;
            foreach (var y in Frecuencia1)
            {
                Texto1[h] = Frecuencia1[h];
                h++;
            }
            char Texto2 = '\t';
            Texto1[h] = (byte)Texto2;
            h++;
            foreach (var item in letras.Ocurrencias)
            {
                Texto1[h] = item.Key;
                int cantid = item.Count;
                byte[] Frecuencia = new byte[f];
                int posi = 0;
                while (cantid != 0)
                {
                    if (cantid > 255)
                    {
                        Frecuencia[posi] = 255;
                        cantid -= 255;
                    }
                    else
                    {
                        Frecuencia[posi] = (byte)cantid;
                        cantid = 0;
                    }
                    posi++;
                }

                h++;
                int i = 0;
                foreach (var x in Frecuencia)
                {
                    Texto1[h] = Frecuencia[i];
                    h++;
                    i++;
                }
            }
            Texto1[h] = (byte)Texto2;
            h++;
            Texto1[h] = (byte)Texto2;
            return Texto1;
        }
        public byte[] EscribirCompresión(Dictionary<byte, string> Diccionario, byte[] texto, bool ultimoparte)
        {
            byte[] Compresion = new byte[texto.Length * 2];
            string Binario = "";
            foreach (var c in texto)
            {
                Binario += Diccionario[c];
            }
            tamaño += Binario.Length;
            if (ultimoparte == true)
            {
                while ((tamaño % 8) != 0)
                {
                    tamaño++;
                    cuantoceros++;
                }
                for (int ceros = 0; ceros < cuantoceros; ceros++)
                {
                    Binario += "0";
                }
            }

            int conta = 0;
            foreach (var item in Binario)
            {
                tempbina += item;
                if (tempbina.Length == 8)
                {
                    byte b = Convert.ToByte(tempbina, 2);
                    Compresion[conta] = b;
                    tempbina = "";
                    conta++;
                }
            }
            byte[] Compresion2 = new byte[conta];
            for (int i = 0; i < conta; i++)
            {
                Compresion2[i] = Compresion[i];
            }
            return Compresion2;
        }
        public void DescompresionRecurrencias(string Ruta, string Ruta2)
        {
            FileStream archivoC = new FileStream(Ruta, FileMode.Open);
            using var leer = new BinaryReader(archivoC);
            Dictionary<byte, int> recurrencias = new Dictionary<byte, int>();

            int total = 0;
            string texto = "";
            byte[] archivo = new byte[1];
            int verificar2 = 0;
            string prefijo = "";

            char Texto3 = '\t';
            int cant = 0;
            int cont2 = 0;
            bool comprobar1 = false;
            byte temp = 0;
            int suma = 0;
            maximo = 0;
            int verificar = 0;

            //Descomprimir recurencia

            while (archivoC.Position < archivoC.Length)
            {
                var buffer = leer.ReadBytes(100);
                foreach (var k in buffer)
                {
                    if (k == (byte)Texto3)
                    {
                        verificar++;
                    }
                    else if (verificar < 2)
                    {
                        verificar = 0;
                    }

                    if (k != (byte)Texto3 && comprobar1 == false)
                    {
                        cant += k;
                    }
                    else if (verificar < 2 && comprobar1 == true)
                    {
                        if (temp == 0)
                        {
                            temp = k;
                        }
                        else if (cont2 < cant)
                        {
                            cont2++;
                            suma += k;
                        }
                        if (cont2 == cant)
                        {
                            if (maximo < suma)
                            {
                                maximo = suma;
                            }
                            recurrencias.Add(temp, suma);
                            cont2 = 0;
                            suma = 0;
                            temp = 0;
                        }
                    }
                    else
                    {
                        comprobar1 = true;
                    }
                }

            }
            archivoC.Close();

            bytes = (Convert.ToDouble(maximo) / 255);
            int f = Convert.ToInt32(bytes);
            if (bytes > Convert.ToDouble(f))
            {
                f++;
            }

            AgregarHeap(HacerArbol(recurrencias));
            AgregarHuffman();
            Prefijos();

            //descomprimir texto
            FileStream archivoD = new FileStream(Ruta2, FileMode.OpenOrCreate);
            FileStream archivoC2 = new FileStream(Ruta, FileMode.Open);
            using var leer2 = new BinaryReader(archivoC2);
            bool comprobar = false;
            while (archivoC2.Position < archivoC2.Length)
            {
                var buffer = leer2.ReadBytes(100);
                foreach (var k in buffer)
                {
                    if (k == (byte)Texto3)
                    {
                        verificar2++;
                    }
                    else if (verificar2 < 2)
                    {
                        verificar2 = 0;
                    }
                    if (verificar2 >= 2)
                    {
                        if (comprobar == true)
                        {
                            string temp1 = Convert.ToString(k, 2);
                            string temp2 = "";
                            for (int p = temp1.Length; p < 8; p++)
                            {
                                temp2 += "0";
                            }
                            texto += temp2 + temp1;
                        }
                        comprobar = true;
                    }
                    while (letras.Total > total && texto.Length >= 1)
                    {
                        prefijo += texto.Substring(0, 1);
                        texto = texto.Substring(1, texto.Length - 1);
                        byte caracter = comprobarprefijos(prefijo);
                        if (caracter != 0)
                        {
                            archivo[0] = caracter;
                            prefijo = "";
                            total++;
                            archivoD.Write(archivo);
                        }
                    }

                }
            }
            archivoD.Flush();
            archivoD.Close();
        }
        public Ocurrencia HacerArbol(Dictionary<byte, int> recurrencias)
        {
            List<NodoHuffman> recu = new List<NodoHuffman>();
            letras = new Ocurrencia();
            letras.Total = 0;
            foreach (var x in recurrencias)
            {
                letras.Total += x.Value;
            }
            foreach (var x in recurrencias)
            {
                NodoHuffman temp = new NodoHuffman();
                temp.Key = x.Key;
                temp.Count = x.Value;
                double prob = (double)x.Value / (double)letras.Total;
                temp.Probabilidad = prob;
                recu.Add(temp);
            }
            letras.Ocurrencias = recu;
            return letras;
        }
        private byte comprobarprefijos(string prefijo)
        {
            foreach (var y in letras.Ocurrencias)
            {
                if (y.Prefijo == prefijo)
                {
                    return y.Key;
                }
            }
            return 0;
        }



    }
}