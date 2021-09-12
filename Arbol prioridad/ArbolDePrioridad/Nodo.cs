using System;
using System.Collections.Generic;
using System.Text;


namespace ArbolDePrioridad
{
    [Serializable]
    public class Nodo<T> : ICloneable
    {

        public T Value = default(T);
        public int Pos = 0;
        public Nodo<T> Left;
        public Nodo<T> Right;

        public Nodo<T> Clone()
        {
            return new Nodo<T>(this);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
        public Nodo()
        {
            this.Left = null;
            this.Right = null;
        }
        public Nodo(Nodo<T> n)
        {
            this.Left = n.Left != null ? n.Left.Clone() : n.Left;
            this.Right = n.Right != null ? n.Right.Clone() : n.Right;
            this.Pos = n.Pos;
            this.Value = n.Value;
            //this.TieneDosHijos = n.TieneDosHijos;
            //this.EsHoja = n.EsHoja;
        }


        public bool EsHoja
        {
            get
            {
                if (Left == null && Right == null)
                {
                    return true;
                }
                else if (Left == null || Right == null)
                {
                    if (Left == null)
                    {
                        return Left is null && Right.Value is null;
                    }
                    else
                    {
                        return Left.Value is null && Right is null;
                    }
                }
                return Left.Value is null && Right.Value is null;
            }
            set
            {
                EsHoja = value;
            }

        }
        public bool TieneDosHijos
        {
            get
            {
                return Left.Value != null && Right.Value != null;
            }
            set
            {
                TieneDosHijos = value;
            }

        }
    }
}
