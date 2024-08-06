using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class MateriaController : Controller
    {

        //[HttpPost]
        //public JsonResult ChangeStatus(bool Status, int IdMateria)
        //{
        //    ML.Result result = BL.MaterDefaultia.ChangeStatus(Status, IdMateria);
        //    return Json(result);
        //}
        // GET: Materia
        [HttpGet] //va a mostrar toda la tabla
        public ActionResult GetAll()
        {
            ML.Materia materia = new ML.Materia();
            // ML.Result result = BL.Materia.GetAllSPEF(materia); Se remplaza por el consumo del servicio
            ServiceMateria.MateriaServiceClient client = new ServiceMateria.MateriaServiceClient();
            ML.Result result = client.GetAll(materia);
            //DateTime fecha = DateTime.Now();

            materia.Materias = result.Objects;

            return View(materia);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Materia materia)
        {
            //usar el operador ternario

            materia.Nombre = materia.Nombre == null ? "" : materia.Nombre;

            ML.Result result = BL.Materia.GetAllSPEF(materia);

            if (result.Correct)
            {
                ML.Materia materias = new ML.Materia();
                materias.Materias = result.Objects;
                return View(materias);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int? idMateria)
        {
            ML.Materia materia = new ML.Materia();

            if (idMateria == null)
            {
                //Vamos a registrar un Materia
                ML.Result result = BL.Semestre.GetAll();
                materia.Semestre = new ML.Semestre();


                materia.Semestre.Semestres = result.Objects; //llenamos la lista
            }
            else
            {
                //Vamos a actualizar un registro
                //GetById
                ML.Result result = BL.Materia.GetByIdEF(idMateria.Value);
                materia = (ML.Materia)result.Object; //unboxing

                ML.Result resultSemestre = BL.Semestre.GetAll();
                //materia.Semestre = new ML.Semestre();
                materia.Semestre.Semestres = resultSemestre.Objects; //llenamos la lista
            }

            return View(materia);
        }

        [HttpPost]
        public ActionResult Form(ML.Materia materia)
        {
            if (ModelState.IsValid)
            {
                DateTime date = DateTime.ParseExact(materia.Fecha, "dd MMMM, yyyy", CultureInfo.InvariantCulture);
                string formattedDate = date.ToString("yyyy-MM-dd");

                materia.Fecha = formattedDate.ToString();
                HttpPostedFileBase imagen = Request.Files["imagen"];
                if (imagen != null && imagen.ContentLength > 0)
                {
                    materia.Imagen = ConvertirABase64(imagen);
                }


                if (materia.IdMateria == 0)
                {
                    //ADD
                    //BL.Materia.AddSP(materia);
                    ServiceMateria.MateriaServiceClient client = new ServiceMateria.MateriaServiceClient();
                    client.Add(materia);
                }
                else
                {
                    //UPDATE
                    BL.Materia.UpdateEF(materia);
                }
                return RedirectToAction("GetAll", "Materia");
            }
            else
            {
                return View(materia); //Regresar informacion al form
            }
            
        }



        public string ConvertirABase64(HttpPostedFileBase Foto)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(Foto.InputStream);
            byte[] data = reader.ReadBytes((int)Foto.ContentLength);
            string imagen = Convert.ToBase64String(data);
            return imagen;
        }
    }
}
