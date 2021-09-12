using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public List<Compresion> Historial;
        private Singleton()
        {
            Historial = new List<Compresion>();
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
