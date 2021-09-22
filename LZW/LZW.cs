using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LZW
{
    public class LZW
    {
        Dictionary<int, string> diccionariolzw = new Dictionary<int, string>();
        Dictionary<string, int> Dic2 = new Dictionary<string, int>();
        Dictionary<string, int> Dic3 = new Dictionary<string, int>();
        bool repitir = false;
        string Cadena = "";
        string tempbina = "";
        public void Comprimir(String Ruta, String Ruta2)
        {

            FileStream archivoc = new FileStream(Ruta2, FileMode.OpenOrCreate);
            FileStream archivoo = new FileStream(Ruta, FileMode.Open);
            using var leer = new BinaryReader(archivoo);
            Caracteres(archivoo, leer);
            Ordenar();
            foreach (var f in diccionariolzw)
            {
                Dic2.Add(f.Value, f.Key);
            }
            foreach (var f in diccionariolzw)
            {
                Dic3.Add(f.Value, f.Key);
            }
            byte[] Escribirletras = new byte[diccionariolzw.Count + 1];
            Escribirletras[0] = Convert.ToByte(diccionariolzw.Count);
            int contad = 0;
            foreach (var k in diccionariolzw)
            {
                contad++;
                Escribirletras[contad] = Convert.ToByte(Convert.ToChar(k.Value));
            }
            contad = 0;
            int llave = 0;
            string numcomp = "";
            archivoo.Position = 0;
            int conta = 0;
            int conta2 = 0;
            int conta3 = 0;
            bool ultimo = false;
            string Binario = "";
            int maximobits = 0;
            Cadenas(archivoo, leer);
            archivoo.Position = 0;
            maximobits = Maximobits(0, 0);
            byte[] escribir2 = Maximobitsescrbiir(maximobits);
            byte[] escrbiirfinal = new byte[escribir2.Length + Escribirletras.Length + 2];
            foreach (var k in escribir2)
            {
                escrbiirfinal[contad] = k;
                contad++;
            }
            escrbiirfinal[contad] = Convert.ToByte('\t');
            contad++;
            escrbiirfinal[contad] = Convert.ToByte('\n');
            contad++;
            foreach (var k in Escribirletras)
            {
                escrbiirfinal[contad] = k;
                contad++;
            }
            archivoc.Write(escrbiirfinal);
            archivoc.Flush();

            while (archivoo.Position < archivoo.Length)
            {
                Binario = "";
                conta3 = 0;
                conta2 = 0;
                var buffer = leer.ReadBytes(100);
                foreach (var y in buffer)
                {
                    conta++;
                    Cadena += Convert.ToString(Convert.ToChar(y));
                    if (Dic3.ContainsKey(Cadena))
                    {
                        llave = Dic3[Cadena];
                    }
                    else
                    {
                        Dic3.Add(Cadena, Dic3.Count + 1);
                        Cadena = Cadena.Substring(Cadena.Length - 1);
                        numcomp = numcomp + llave + ",";
                        conta2++;
                        llave = Dic3[Cadena];

                    }
                    if (conta == archivoo.Length)
                    {
                        numcomp = numcomp + llave + ",";
                        conta2++;
                        ultimo = true;
                    }
                }
                while (numcomp.Length != 0)
                {
                    int pos = numcomp.IndexOf(',');
                    int temp = Convert.ToInt32(numcomp.Substring(0, pos));
                    numcomp = numcomp.Substring(pos + 1, numcomp.Length - pos - 1);
                    string esc2 = "";
                    while (temp != 0)
                    {
                        if (temp % 2 != 0)
                        {
                            esc2 = "1" + esc2;
                        }
                        else
                        {
                            esc2 = "0" + esc2;
                        }
                        temp = temp / 2;
                    }
                    while (esc2.Length < maximobits)
                    {
                        esc2 = "0" + esc2;
                    }
                    Binario += esc2;
                    esc2 = "";
                }
                if (ultimo == true)
                {
                    while ((Binario.Length + tempbina.Length) % 8 != 0)
                    {
                        Binario += "0";
                    }
                }
                byte[] escribir = new byte[(Binario.Length + tempbina.Length) / 8];
                foreach (var item in Binario)
                {
                    tempbina += item;
                    if (tempbina.Length == 8)
                    {
                        byte b = Convert.ToByte(tempbina, 2);
                        escribir[conta3] = b;
                        tempbina = "";
                        conta3++;
                    }
                }
                archivoc.Write(escribir);
                archivoc.Flush();
            }
            archivoc.Close();
        }
        public void Ordenar()
        {
            for (int x = 0; x < diccionariolzw.Count; x++)
            {
                for (int y = 0; y < diccionariolzw.Count - x; y++)
                {
                    char temp = Convert.ToChar(diccionariolzw[x + 1]);
                    if (Convert.ToChar(diccionariolzw[x + 1]) > Convert.ToChar(diccionariolzw[y + 1 + x]))
                    {
                        string aux = diccionariolzw[x + 1];
                        diccionariolzw[x + 1] = diccionariolzw[y + 1 + x];
                        diccionariolzw[y + 1 + x] = aux;
                    }
                }
            }
        }
        public int Maximobits(double suma, int conta)
        {
            for (int x = 0; x <= 8; x++)
            {
                suma += Math.Pow(2, x);
                conta++;
                if (suma >= Dic2.Count)
                {
                    return conta;
                }
            }
            return Maximobits(suma, conta);
        }
        public byte[] Maximobitsescrbiir(int Conta)
        {
            double f = Convert.ToDouble(Conta) / 255;
            int f1 = Convert.ToInt32(Math.Truncate(f));
            if (f % 1 != 0)
            {
                f1++;
            }
            byte[] escribir = new byte[f1];
            int posi1 = 0;
            while (Conta != 0)
            {
                if (Conta > 255)
                {
                    escribir[posi1] = 255;
                    Conta -= 255;
                }
                else
                {
                    escribir[posi1] = (byte)Conta;
                    Conta = 0;
                }
                posi1++;
            }
            return escribir;
        }
        public void Cadenas(FileStream archivoo, BinaryReader leer)
        {
            while (archivoo.Position < archivoo.Length)
            {
                var buffer = leer.ReadBytes(50);
                foreach (var y in buffer)
                {
                    Cadena += Convert.ToString(Convert.ToChar(y));
                    if (!(Dic2.ContainsKey(Cadena)))
                    {
                        Dic2.Add(Cadena, Dic2.Count + 1);
                        Cadena = Cadena.Substring(Cadena.Length - 1);
                    }
                }
            }
            Cadena = "";
        }
        public void Caracteres(FileStream archivoo, BinaryReader leer)
        {
            while (archivoo.Position < archivoo.Length)
            {
                var buffer = leer.ReadBytes(50);
                foreach (var y in buffer)
                {
                    foreach (var x in diccionariolzw)
                    {
                        if (Convert.ToChar(x.Value) == Convert.ToChar(y))
                        {
                            repitir = true;
                        }

                    }
                    if (!repitir == true)
                    {
                        char Temp = Convert.ToChar(y);
                        diccionariolzw.Add(diccionariolzw.Count + 1, Convert.ToString(Convert.ToChar(y)));
                    }
                    repitir = false;
                }
            }
        }
    }
}
