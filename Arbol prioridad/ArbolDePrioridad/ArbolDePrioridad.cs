using System;
using System.Collections.Generic;
using System.Text;

namespace ArbolDePrioridad
{
    [Serializable]
    public class ArbolDePrioridad<T> : ICloneable
    {
        public ArbolDePrioridad<T> Clone()
        {
            return new ArbolDePrioridad<T>(this);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
        public ArbolDePrioridad(ArbolDePrioridad<T> a)
        {
            this.root = a.root.Clone();
            this.comparador = a.comparador;
            this.Comprobacion = a.Comprobacion;
            this.CantidadNodos = a.CantidadNodos;
            this.Ordenado = a.Ordenado;
        }
        public delegate int Comparador<T>(T a, T b);
        public ArbolDePrioridad(Comparador<T> Funcomparador)
        {
            this.comparador = Funcomparador;
        }
        Nodo<T> root = new Nodo<T>();

        internal Comparador<T> comparador;
        bool Ordenado;
        bool Comprobacion;
        int CantidadNodos = 0;
        public bool isempty()
        {
            if (root == null)
            {
                return true;
            }
            else if (root.Value == null)
            {
                return true;
            }
            return false;
        }
        public  int CantidadN()
        {
            return CantidadNodos;
        }
        public void Empty()
        {
            root = null;
            CantidadNodos = 0;
            Comprobacion = new bool();
            Ordenado = new bool();
        }
        public T Peek()
        {
            return root.Value;
        }
        public void add(T dato)
        {
            CantidadNodos++;
            if (root == null)
            {
                root = new Nodo<T>();
                root.Value = dato;
                root.Left = new Nodo<T>();
                root.Right = new Nodo<T>();
                root.Pos = 1;
            }
            else if (root.Value == null)
            {

                root = new Nodo<T>();
                root.Value = dato;
                root.Left = new Nodo<T>();
                root.Right = new Nodo<T>();
                root.Pos = 1;

            }
            else
            {
                Comprobacion = false;
                add2(dato, root);
            }


        }
        private void add2(T dato, Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.Value == null)
            {
                CurrentRoot.Value = dato;
                CurrentRoot.Pos = CantidadNodos;
                Comprobacion = true;
                CurrentRoot.Left = new Nodo<T>();
                CurrentRoot.Right = new Nodo<T>();
            }
            else if (CurrentRoot.Left.Value != null && CurrentRoot.Right.Value != null)
            {
                add2(dato, CurrentRoot.Left);
                if (Comprobacion == false)
                {
                    add2(dato, CurrentRoot.Right);
                }
            }
            else if (CurrentRoot.Left.Value == null)
            {
                bool Left = CantidadNodos / 2 == CurrentRoot.Pos;
                if (Left == true)
                {
                    add2(dato, CurrentRoot.Left);
                }
            }
            else if (CurrentRoot.Right.Value == null)
            {

                bool Right = (CantidadNodos - 1) / 2 == CurrentRoot.Pos;
                if (Right == true)
                {
                    add2(dato, CurrentRoot.Right);
                }
            }
            ComprobarOrden(CurrentRoot);
        }
        public void ComprobarOrden(Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.Left.Value != null || CurrentRoot.Right.Value != null)
            {
                Nodo<T> Temp = new Nodo<T>();
                Temp.Value = CurrentRoot.Value;


                if (CurrentRoot.Left.Value != null)
                {
                    if (comparador.Invoke(CurrentRoot.Left.Value, CurrentRoot.Value) == -1)
                    {
                        CurrentRoot.Value = CurrentRoot.Left.Value;
                        CurrentRoot.Left.Value = Temp.Value;
                    }

                }
                if (CurrentRoot.Right.Value != null)
                {
                    if (comparador.Invoke(CurrentRoot.Right.Value, CurrentRoot.Value) == -1)
                    {
                        CurrentRoot.Value = CurrentRoot.Right.Value;
                        CurrentRoot.Right.Value = Temp.Value;
                    }

                }
            }
        }
        public T Remove()
        {
            Nodo<T> Temp = new Nodo<T>();
            Temp.Value = root.Value;
            if (root.Left == null)
            {
                root = null;
            }
            else if (root.Left.Value == null)
            {
                root = null;
            }
            else
            {
                Remove2(root);
            }
            if (root == null)
            {
                CantidadNodos--;
                return Temp.Value;
            }
            OrdenarEliminacion(root);
            CantidadNodos--;
            return Temp.Value;

        }
        private void Remove2(Nodo<T> CurrentRoot)
        {

            if (CurrentRoot.EsHoja)
            {
                root.Value = CurrentRoot.Value;
                CurrentRoot.Value = default(T);
            }
            else
            {
                double Cant = CantidadNodos;
                double nivel1 = Math.Truncate(Math.Log2(CurrentRoot.Pos)) + 1;
                double nivel2 = Math.Truncate(Math.Log2(CantidadNodos));
                for (int i = Convert.ToInt32(nivel1); i < nivel2; i++)
                {
                    Cant = Cant / 2;
                }
                int cant2 = Convert.ToInt32(Math.Truncate(Cant));
                if (cant2 == (CurrentRoot.Left.Pos))
                {
                    Remove2(CurrentRoot.Left);
                }
                else
                {
                    Remove2(CurrentRoot.Right);
                }
            }
        }
        public void OrdenarEliminacion(Nodo<T> CurrentRoot)
        {
            if (CurrentRoot.TieneDosHijos)
            {
                int i = comparador.Invoke(CurrentRoot.Left.Value, CurrentRoot.Right.Value);
                if (i != 1)
                {
                    int i2 = comparador.Invoke(CurrentRoot.Value, CurrentRoot.Left.Value);
                    if (i2 == 1)
                    {
                        Nodo<T> temp = new Nodo<T>();
                        temp.Value = CurrentRoot.Value;
                        CurrentRoot.Value = CurrentRoot.Left.Value;
                        CurrentRoot.Left.Value = temp.Value;
                        OrdenarEliminacion(CurrentRoot.Left);
                    }

                }
                else
                {
                    int i2 = comparador.Invoke(CurrentRoot.Value, CurrentRoot.Right.Value);
                    if (i2 == 1)
                    {
                        Nodo<T> temp = new Nodo<T>();
                        temp.Value = CurrentRoot.Value;
                        CurrentRoot.Value = CurrentRoot.Right.Value;
                        CurrentRoot.Right.Value = temp.Value;
                        OrdenarEliminacion(CurrentRoot.Right);
                    }
                }
            }
            else if (!CurrentRoot.EsHoja)
            {
                int i2 = comparador.Invoke(CurrentRoot.Value, CurrentRoot.Left.Value);
                if (i2 == 1)
                {
                    Nodo<T> temp = new Nodo<T>();
                    temp.Value = CurrentRoot.Value;
                    CurrentRoot.Value = CurrentRoot.Left.Value;
                    CurrentRoot.Left.Value = temp.Value;
                }
            }
        }
    }
}
