using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Conexion
    {
        public static string GetConnection()
        {
            //string cadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["JGuevaraProgramacionNCapas"].ConnectionString.ToString();
            //return cadenaConexion;
            return System.Configuration.ConfigurationManager.ConnectionStrings["JGuevaraProgramacionNCapas"].ConnectionString.ToString();
        }
    }
}
