using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var result = BL.Materia.CargaMasiva();
            if (result.Item1) 
            {
                Console.WriteLine("Carga masiva con exito");
            }
            else
            {
                Console.WriteLine("Ocurrio un error" + result.Item2);
            }
        }
    }
}
