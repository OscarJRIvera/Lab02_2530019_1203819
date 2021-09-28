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
        LZWInfo descinfo = new LZWInfo();
        bool repitir = false;
        string Cadena = "";
        string tempbina = "";
        int maximobits = 0;
        int maxbit = 0;
        int contaletra = 0;
        public void Comprimir(String Ruta, String Ruta2)
        {
            contaletra = 0;
            maxbit = 0;
            maximobits = 0;
            tempbina = "";
            Cadena = "";
            repitir = false;
            descinfo = new LZWInfo();
            diccionariolzw = new Dictionary<int, string>();
            Dic2 = new Dictionary<string, int>();
            Dic3 = new Dictionary<string, int>();
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
            byte[] Escribirletras = new byte[diccionariolzw.Count + 2];
            Escribirletras[1] = Convert.ToByte(diccionariolzw.Count);
            int contad = 2;
            foreach (var k in diccionariolzw)
            {
                Escribirletras[contad] = Convert.ToByte(Convert.ToChar(k.Value));
                contad++;
            }
            contad = 0;
            int llave = 0;
            string numcomp = "";
            archivoo.Position = 0;
            int conta = 0;
            bool ultimo = false;
            Cadenas(archivoo, leer);
            archivoo.Position = 0;
            maximobits = Maximobits(0, 0);
            Escribirletras[0] = Convert.ToByte(maximobits);
            archivoc.Write(Escribirletras);
            archivoc.Flush();
            while (archivoo.Position < archivoo.Length)
            {

                var buffer = leer.ReadBytes(50);
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
                        numcomp = Convert.ToString(llave);
                        llave = Dic3[Cadena];

                    }
                    comprimirbits(archivoc, leer, numcomp, ultimo);
                    numcomp = "";
                    if (conta == archivoo.Length)
                    {
                        numcomp = Convert.ToString(llave);
                        ultimo = true;
                        comprimirbits(archivoc, leer, numcomp, ultimo);
                    }
                }

            }
            archivoc.Close();
            archivoo.Close();
        }
        public void Descomprimir(String Ruta, String Ruta2)
        {
            FileStream archivoC = new FileStream(Ruta, FileMode.Open);
            FileStream archivoD = new FileStream(Ruta2, FileMode.OpenOrCreate);
            using var leer = new BinaryReader(archivoC);
            Dic2 = DescomprimirLetras(archivoC, leer);
            diccionariolzw = new Dictionary<int, string>();
            foreach (var f in Dic2)
            {
                diccionariolzw.Add(f.Value, f.Key);
            }
            archivoC.Position = 0;
            DescomprimirTexto(archivoC, leer, archivoD);
            archivoC.Close();
            archivoD.Close();
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
            suma += Math.Pow(2, conta);
            conta++;
            if (suma >= Dic2.Count)
            {
                return conta;
            }
            return Maximobits(suma, conta);
        }
        public void comprimirbits(FileStream archivoC, BinaryReader leer, string numcomp, bool ultimo)
        {
            int conta3 = 0;
            int cuantosbytes0 = 0;
            string Binario = "";
            if (numcomp != "")
            {
                int temp = Convert.ToInt32(numcomp);
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
                cuantosbytes0 = (maximobits - esc2.Length) / 8;
                while (esc2.Length < maximobits - (cuantosbytes0 * 8))
                {
                    esc2 = "0" + esc2;
                }
                Binario += esc2;
                if (ultimo == true)
                {
                    while ((Binario.Length + tempbina.Length) % 8 != 0)
                    {
                        Binario += "0";
                    }
                }
                double size = (Convert.ToDouble(Binario.Length) + Convert.ToDouble(tempbina.Length)) / 8;
                int size2 = Convert.ToInt32(Math.Truncate(size));
                byte[] escribir = new byte[size2 + cuantosbytes0];
                while (cuantosbytes0 != 0)
                {
                    cuantosbytes0--;
                    for (int o = 0; o < 8; o++)
                    {
                        tempbina += '0';
                        if (tempbina.Length == 8)
                        {
                            byte b = Convert.ToByte(tempbina, 2);
                            escribir[conta3] = b;
                            tempbina = "";
                            conta3++;
                        }
                    }
                    if (cuantosbytes0 > 1)
                    {
                        conta3 += cuantosbytes0;
                        cuantosbytes0 = 0;
                    }
                }
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
                if (escribir.Length != 0)
                {
                    archivoC.Write(escribir);
                    archivoC.Flush();
                }

            }
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
        public Dictionary<string, int> DescomprimirLetras(FileStream archivoC, BinaryReader leer)
        {
            int contad = 0;
            int cuantasletras = 0;
            int letrascant = 1;
            Dictionary<string, int> letras = new Dictionary<string, int>();
            while (archivoC.Position < archivoC.Length)
            {
                var buffer = leer.ReadBytes(50);
                foreach (var k in buffer)
                {
                    if (contad == 0)
                    {
                        maxbit = Convert.ToInt32(k);
                    }
                    else if (contad == 1)
                    {
                        cuantasletras = Convert.ToInt32(k);
                    }
                    else
                    {
                        if (cuantasletras != 0)
                        {
                            letras.Add(Convert.ToString(Convert.ToChar(k)), letrascant);
                            letrascant++;
                            cuantasletras--;
                        }
                        else
                        {
                            contaletra = letras.Count + 2;
                            return letras;
                        }
                    }
                    contad++;
                }
            }
            return null;
        }
        public void DescomprimirTexto(FileStream archivoC, BinaryReader leer, FileStream archivoD)
        {
            descinfo.actual = ""; descinfo.nuevo = ""; descinfo.previo = "";
            tempbina = "";
            int contador = 0;
            string binario = "";
            int suma = 0;
            int exponente = 0;
            while (archivoC.Position < archivoC.Length)
            {
                var buffer = leer.ReadBytes(50);
                foreach (var k in buffer)
                {
                    if (contador >= contaletra)
                    {
                        binario += Convert.ToString(k, 2);
                        while (binario.Length % 8 != 0)
                        {
                            binario = "0" + binario;
                        }
                        foreach (var o in binario)
                        {
                            tempbina += o;
                            if (tempbina.Length == maxbit)
                            {
                                exponente = maxbit - 1;
                                suma = 0;
                                foreach (var F in tempbina)
                                {
                                    if (F == '1')
                                    {
                                        suma += Convert.ToInt32(Math.Pow(2, exponente));
                                    }
                                    exponente--;
                                }
                                if (suma == 0)
                                {
                                    return;
                                }
                                tempbina = "";
                                string temp = "";
                                if (suma == diccionariolzw.Count + 1)
                                {
                                    temp = descinfo.previo + descinfo.previo;
                                }
                                else
                                {
                                    temp = diccionariolzw[suma];
                                }
                                byte[] escribir = new byte[temp.Length];
                                int contadorescribir = 0;
                                foreach (var ch in temp)
                                {
                                    escribir[contadorescribir] = Convert.ToByte(ch);
                                    contadorescribir++;
                                }
                                if (descinfo.previo == "")
                                {
                                    descinfo.previo = temp;
                                }
                                else if (descinfo.actual == "")
                                {
                                    descinfo.actual = temp;
                                    descinfo.nuevo = descinfo.previo + descinfo.actual.Substring(0, 1);
                                    diccionariolzw.Add(diccionariolzw.Count + 1, descinfo.nuevo);
                                    descinfo.previo = descinfo.actual;
                                    descinfo.actual = "";
                                    descinfo.nuevo = "";
                                }
                                archivoD.Write(escribir);
                                archivoD.Flush();
                            }
                        }
                        binario = "";
                    }
                    contador++;
                }
            }
        }

    }
}
