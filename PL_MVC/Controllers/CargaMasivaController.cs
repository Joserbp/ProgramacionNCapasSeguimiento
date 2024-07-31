using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class CargaMasivaController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(HttpPostedFileBase archivoExcel)
        {
            if (Session["PathArchivo"] == null)  //NO SE A VALIDADO
            {
                //Validar si el archivo tiene datos
                if (archivoExcel != null || archivoExcel.ContentLength > 0)
                {
                    //Validar si el archivo corresponde a una extension de EXCEL
                    string extensionArchivo = Path.GetExtension(archivoExcel.FileName);
                    string extensionValida = ".xlsx";
                    if (extensionArchivo == extensionValida)
                    {
                        //Generar una copia de ese archivo de Excel
                        //Respaldo en caso de un error en la lectura
                        //Historico de los Excel que han sido procesados
                        string pathArchivoCopia = Server.MapPath("~/CargaMasiva/") + Path.GetFileNameWithoutExtension(archivoExcel.FileName) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"; //C:server/docu/cargamasiva/carga-20240723113512.xlsx;            
                        if (!System.IO.File.Exists(pathArchivoCopia)) //Validar que si se haya generado la copia
                        {
                            //Guardar la copia en una carpeta del proyecto
                            archivoExcel.SaveAs(pathArchivoCopia);
                            string connectionStringExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathArchivoCopia + ";Extended Properties= Excel 12.0 XML;";


                            var result = BL.Materia.GetAllByExcel(connectionStringExcel);
                            //Leer el archivo (Copia)
                            if (result.Item1)  // Validar que la lectura de los datos fue correcta (RESULT)
                            {
                                //Validar que todos los campos tengan informacion
                                ML.ErrorExcel errorExcel = BL.Materia.Validar(result.Item3);
                                if (errorExcel.Errores.Count > 0)
                                {
                                    return View(errorExcel);
                                    //Regresar a la vista y mostrar una tabla con la fila y su mensaje de error
                                }
                                else
                                {
                                    //Guardar los datos en la b
                                    // Investigar el uso de una session de C#
                                    //Session  -- Guardar informacion //Object,Int,String,Modelo,Array
                                    //Recuperar en cualquier parte del codigo
                                    Session["PathArchivo"] = pathArchivoCopia;
                                    return View(errorExcel);
                                }
                            }
                            else
                            {
                                //
                            }
                        }
                        else
                        {
                            //YA EXISTE ESE ARCHIVO
                            //Manejo del error
                        }
                    }
                    //Validar que si se haya generado la copia
                    //Leer el archivo (Copia)
                    // Validar que la lectura de los datos fue correcta (RESULT)
                    //TRUE //INSERT A LA BD
                    //FALSE // Mandar un reporte de que filas tienen error y en que columnas
                    // CAMPOS VACIOS 

                    else
                    {
                        //Manejo del error
                    }

                }
                else
                {
                    //Manejo del error
                }
            }
            else //YA SE VALIDO
            {
                string pathArchivo = Session["PathArchivo"].ToString();
                string connectionStringExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathArchivo + ";Extended Properties= Excel 12.0 XML;";
                var result = BL.Materia.GetAllByExcel(connectionStringExcel);
                foreach (ML.Materia materia in result.Item3)
                {
                    BL.Materia.Add(materia);
                }
                ViewBag.Message = "Se cargaron los registros a la bd";
                Session["PathArchivo"] = null;
                return PartialView("Modal");
            }
        return View();
        }

    }
}