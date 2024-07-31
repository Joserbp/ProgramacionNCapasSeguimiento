using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Materia
    {
        //METODOS 
        public static void Add(ML.Materia materia)
        {
            try
            {

                using (SqlConnection context = new SqlConnection())
                {
                    context.ConnectionString = DL.Conexion.GetConnection();
                    context.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Materia]([Nombre],[Creditos],[Costo]) VALUES (@Nombre,@Creditos,@Costo)", context);
                    cmd.Parameters.AddWithValue("@Nombre", materia.Nombre);
                    cmd.Parameters.AddWithValue("@Creditos", materia.Creditos);
                    cmd.Parameters.AddWithValue("@Costo", materia.Costo);

                    cmd.ExecuteNonQuery();
                }


                
            } 
            catch (Exception ex)
            {

            }
            
        }
    }
}
