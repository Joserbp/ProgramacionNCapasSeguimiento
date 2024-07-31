using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class Materia
    {
        public static void Add() //Solicitar datos
        {
            ML.Materia materia = new ML.Materia();

            Console.WriteLine("Ingrese el nombre de la Materia");
            materia.Nombre = Console.ReadLine();

            Console.WriteLine("Ingrese los créditos de la Materia");
            materia.Creditos = Convert.ToByte(Console.ReadLine());

            Console.WriteLine("Ingrese el costo de la Materia");
            materia.Costo = Convert.ToDecimal(Console.ReadLine());

            Console.WriteLine("Ingrese el Semestre de la Materia");
            
            materia.Semestre = new ML.Semestre();
            materia.Semestre.IdSemestre = Convert.ToByte(Console.ReadLine());
            
            ML.Result result = BL.Materia.AddEFLINQ(materia);//boxing unboxing

            if (result.Correct)
            {
                Console.WriteLine("La materia fue insertada correctamente");
            }
            else
            {
                Console.WriteLine("Ocurrió un error al insertar la materia" + result.ErrorMessage);
            }

        }
        public static void GetAll() //Solicitar datos
        {

            ML.Result result = BL.Materia.GetAllSPReader();


            if (result.Correct)
            {
                if (result.Objects.Count > 0)
                {
                    foreach (ML.Materia materia in result.Objects)
                    {
                        Console.WriteLine(materia.IdMateria);
                        Console.WriteLine(materia.Nombre);
                        Console.WriteLine(materia.Creditos);
                        Console.WriteLine(materia.Costo);

                    }
                }
                Console.WriteLine("La materia fue insertada correctamente");
            }
            else
            {
                Console.WriteLine("Ocurrió un error al insertar la materia" + result.ErrorMessage);
            }

        }
        public static void GetById() //Solicitar datos
        {

            ML.Result result = BL.Materia.GetByIdEFLinq();


            if (result.Correct)
            {
                if (result.Objects.Count > 0)
                {
                    foreach (ML.Materia materia in result.Objects)
                    {
                        Console.WriteLine(materia.IdMateria);
                        Console.WriteLine(materia.Nombre);
                        Console.WriteLine(materia.Creditos);
                        Console.WriteLine(materia.Costo);

                    }
                }
                Console.WriteLine("La materia fue insertada correctamente");
            }
            else
            {
                Console.WriteLine("Ocurrió un error al insertar la materia" + result.ErrorMessage);
            }

        }




        public static ML.Result CargaMasivaTxt()
        {
            ML.Result result = new ML.Result();
            string path = @"C:\Users\digis\Downloads\JGuevaraProgramacionNCapas (3)\JGuevaraProgramacionNCapas\PL\Files\Insert Materia.txt";

            StreamReader txt = new StreamReader(path);

            string linea = txt.ReadLine();
            while((linea = txt.ReadLine()) != null)
            {
                string [] valores = linea.Split('|');
                ML.Materia materia = new ML.Materia();
                materia.Nombre = valores[0];
                materia.Creditos = Convert.ToByte(valores[1]);
                materia.Costo = Convert.ToDecimal(valores[2]);
                materia.Semestre = new ML.Semestre();
                materia.Semestre.IdSemestre = Convert.ToByte(valores[3]);

                result = BL.Materia.AddSP(materia);

                //las demas propiedades
                //llamar al metodo Add de BL
                //Result 
            }

            return result;
        }
    }
}
