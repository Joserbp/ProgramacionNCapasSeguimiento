using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Materia
    {
        public static void Add()
        {
            ML.Materia materia = new ML.Materia();

            Console.WriteLine("Ingrese el nombre de la Materia");
            materia.Nombre = Console.ReadLine();

            Console.WriteLine("Ingrese los créditos de la Materia");
            materia.Creditos = Convert.ToByte(Console.ReadLine());

            Console.WriteLine("Ingrese el costo de la Materia");
            materia.Costo = Convert.ToDecimal(Console.ReadLine());

            BL.Materia.Add(materia);

        }
    }
}
