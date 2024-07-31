using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Materia
    {
        //METODOS 
        public static (bool, string, List<Object>) GetAllByExcel(string connectionString)
        {
            try
            {
                using (OleDbConnection context = new OleDbConnection(connectionString))
                {
                    string query = "SELECT * FROM [Sheet1$]";
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = context;
                    cmd.CommandText = query;
                    cmd.Connection.Open();
                    
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    DataTable tablaMateria = new DataTable();

                    adapter.Fill(tablaMateria);

                    if (tablaMateria.Rows.Count > 0)
                    {
                        List<Object> materias = new List<Object>();
                        foreach (DataRow row in tablaMateria.Rows)
                        {

                            //byte byteValue;
                            //bool success = Byte.TryParse(stringToConvert, out byteValue);
                            //if (success)
                            //{
                            //    Console.WriteLine("Converted '{0}' to {1}",
                            //                   stringToConvert, byteValue);
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Attempted conversion of '{0}' failed.",
                            //                      stringToConvert);
                            //}
                            ML.Materia materia = new ML.Materia();
                            materia.Nombre = row[0].ToString();
                            byte validarByte;
                            decimal validarDecimal;
                                                   // ( condicional ) ? 'True' : 'False'
                            materia.Creditos = (byte)((Byte.TryParse(row[1].ToString(), out validarByte)) ? validarByte : 0);
                            materia.Costo = (decimal)((Decimal.TryParse(row[2].ToString(), out validarDecimal)) ? validarDecimal : 0);
                            //bool success = Byte.TryParse(row[1].ToString(), out validarByte);
                            //if (success)
                            //{
                            //    materia.Creditos = validarByte;
                            //}
                            //else
                            //{
                            //    materia.Creditos = 0;
                            //}
                            // Convert.ToByte(row[1].ToString());//byte.Parse("");
                            materias.Add(materia);
                        }
                        return(true,null,materias);
                    }
                    else
                    {
                        return (false, "No exiten registros", null);
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }

        }
        
        public static ML.ErrorExcel Validar(List<Object> Materias)
        {
            ML.ErrorExcel errorExcel = new ML.ErrorExcel();
            errorExcel.Errores = new List<object>();
            int contador = 2;
            foreach (ML.Materia objMateria in Materias)
            {
                string errores = "";
                if (objMateria.Nombre == "")
                {
                    errores += "La columna nombre esta vacia";
                }
                if (objMateria.Creditos == 0)
                {
                    errores += "La columna creditos esta vacia";
                }
                errores += (objMateria.Costo == 0) ? "La columna Costo esta vacia" : "";
               
                if(errores != "")
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.Fila = contador;
                    error.Mensaje = errores;
                    errorExcel.Errores.Add(error);
                }
                contador++;
            }
            return errorExcel;
        }
        
        public static (bool, string) CargaMasivaTxt(Stream archivoTxt)
        {
            try
            {
                // string path = "C:/DIGS/JOSE/ARCHIV/archivo.txt"; Ruta Absoluta
                // string path = ~/archivo.txt Ruta Relativa
                if (archivoTxt != null)
                {
                    StreamReader sr = new StreamReader(archivoTxt);  
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] datosSeparados = line.Split('|');
                        ML.Materia materia = new ML.Materia();
                        materia.Nombre = datosSeparados[0];
                        materia.Creditos = Convert.ToByte(datosSeparados[1]);
                        materia.Costo = decimal.Parse(datosSeparados[2]);
                        ML.Result result = BL.Materia.Add(materia);
                        if (!result.Correct)
                        {
                            return (false, result.ErrorMessage);
                        }
                    }
                    return (true, "");
                }
                else
                {
                    return (false, "No existe el archivo en la ruta");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public static ML.Result Add(ML.Materia materia) //Proceso en la bd
        {
            ML.Result result = new ML.Result();

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

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result AddSP(ML.Materia materia) //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (SqlConnection context = new SqlConnection())
                {
                    context.ConnectionString = DL.Conexion.GetConnection();
                    context.Open();
                    SqlCommand cmd = new SqlCommand("MateriaAdd", context);
                    cmd.Parameters.AddWithValue("@Nombre", materia.Nombre);
                    cmd.Parameters.AddWithValue("@Creditos", materia.Creditos);
                    cmd.Parameters.AddWithValue("@Costo", materia.Costo);
                    cmd.Parameters.AddWithValue("@IdSemestre", materia.Semestre.IdSemestre);
                    cmd.Parameters.AddWithValue("@Imagen", materia.Imagen);
                    cmd.Parameters.AddWithValue("@Fecha", materia.Fecha);
                    cmd.CommandType = CommandType.StoredProcedure;
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result GetAllSP() //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (SqlConnection context = new SqlConnection())
                {
                    context.ConnectionString = DL.Conexion.GetConnection();
                    context.Open();
                    SqlCommand cmd = new SqlCommand("MateriaGetAll", context);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tableMateria = new DataTable();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(tableMateria);


                    if (tableMateria.Rows.Count > 0)
                    {

                        result.Objects = new List<object>();

                        foreach (DataRow row in tableMateria.Rows)
                        {
                            ML.Materia materia = new ML.Materia();
                            materia.IdMateria = Convert.ToInt32(row[0].ToString());
                            materia.Nombre = row[1].ToString();
                            materia.Creditos = Convert.ToByte(row[2].ToString());
                            materia.Costo = Convert.ToDecimal(row[3].ToString());
                            materia.Imagen = row[6].ToString();
                            //materia.Fecha = row[7].ToString();
                            //materia.Fecha = query.Fecha.Value.ToString("dd MM yyyy");
                            materia.Turno = row[8].ToString();
                            DateTime fecha = new DateTime();
                            fecha = DateTime.Now;

                            materia.Fecha = fecha.ToString("dd/MMMM/yyyyy");

                            result.Objects.Add(materia); //Boxing 
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Materia";
                    }


                    //if (rowsAffected > 0)
                    //{
                    //    result.Correct = true;
                    //}
                    //else
                    //{
                    //    result.Correct = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetAllSPReader() //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (SqlConnection context = new SqlConnection())
                {
                    context.ConnectionString = DL.Conexion.GetConnection();
                    context.Open();
                    SqlCommand cmd = new SqlCommand("MateriaGetAll", context);
                    cmd.CommandType = CommandType.StoredProcedure;

                    DataTable tableMateria = new DataTable();

                    SqlDataReader reader = cmd.ExecuteReader();


                    result.Objects = new List<object>();


                    while (reader.Read())
                    {
                        ML.Materia materia = new ML.Materia();
                        materia.IdMateria = Convert.ToInt32(reader[0].ToString());
                        materia.Nombre = reader[1].ToString();
                        materia.Creditos = Convert.ToByte(reader[2].ToString());
                        materia.Costo = Convert.ToDecimal(reader[3].ToString());

                        result.Objects.Add(materia); //Boxing 

                    }

                    result.Correct = true;



                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result AddEFLINQ(ML.Materia materia)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    DL_EF.Materia linqMateria = new DL_EF.Materia();
                    linqMateria.Nombre = materia.Nombre;
                    linqMateria.Creditos = materia.Creditos;
                    linqMateria.Costo = materia.Costo;
                    linqMateria.IdSemestre = materia.Semestre.IdSemestre;

                    context.Materias.Add(linqMateria);
                    int filas = context.SaveChanges();

                    if (filas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetByIdEFLinq() //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    var query = (from materiaLinq in context.Materias
                                 where materiaLinq.IdMateria >= 5
                                 select materiaLinq).ToList();

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result UpdateEF(ML.Materia materia)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    int filas = context.MateriaUpdate(materia.IdMateria, materia.Nombre, materia.Creditos, materia.Costo, materia.Semestre.IdSemestre, materia.Imagen, DateTime.Parse(materia.Fecha), materia.Turno);

                    if (filas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo actualizar";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result GetByIdEF(int idMateria) //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    var query = context.MateriaGetById(idMateria).FirstOrDefault();


                    if (query != null)
                    {

                        ML.Materia materia = new ML.Materia();
                        materia.IdMateria = query.IdMateria;
                        materia.Nombre = query.Nombre;
                        materia.Creditos = query.Creditos.Value;
                        materia.Costo = query.Costo.Value;
                        materia.Imagen = query.Imagen;
                        materia.Fecha = query.Fecha.Value.ToString("dd MMMM, yyyy");
                        materia.Turno = query.Turno;

                        materia.Semestre = new ML.Semestre();
                        materia.Semestre.IdSemestre = Convert.ToByte(query.IdSemestre);

                        result.Object = materia;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Materia";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result GetAllSPEF(ML.Materia materia) //Proceso en la bd
        {
            ML.Result result = new ML.Result();

            try
            {

                using (DL_EF.JGuevaraProgramacionNCapasMayoEntities context = new DL_EF.JGuevaraProgramacionNCapasMayoEntities())
                {
                    var query = context.MateriaGetAll(materia.Nombre).ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach(var item in query)
                        {
                            ML.Materia materiaItem = new ML.Materia();
                            materiaItem.IdMateria = item.IdMateria;
                            materiaItem.Nombre = item.MateriaNombre;
                            materiaItem.Creditos = item.Creditos.Value;
                            materiaItem.Costo = item.Costo.Value;
                            materiaItem.Imagen = item.Imagen;
                            materiaItem.Fecha = item.Fecha.Value.ToString("dd MMMM, yyyy");
                            materiaItem.Turno = item.Turno;
                            materiaItem.Status = item.Status;

                            materiaItem.Semestre = new ML.Semestre();
                            materiaItem.Semestre.IdSemestre = Convert.ToByte(item.IdSemestre);

                            result.Objects.Add(materiaItem);
                        }

                        result.Correct = true;
                    }


                    //if (rowsAffected > 0)
                    //{
                    //    result.Correct = true;
                    //}
                    //else
                    //{
                    //    result.Correct = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
