using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Semestre
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    var query = context.SemestreGetAll().ToList();

                    if(query != null)//trae registros
                    {
                        result.Objects = new List<object>();

                        foreach(var item in query)
                        {
                            ML.Semestre semestre = new ML.Semestre();
                            semestre.IdSemestre = Convert.ToByte(item.IdSemestre);
                            semestre.Nombre = item.Nombre;

                            result.Objects.Add(semestre);
                        }

                        result.Correct = true;

                    } else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros en Semestre";
                    }
                }

            } catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
