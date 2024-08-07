using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL_WebApi.Controllers
{
    public class OperacionesController : ApiController
    {
        //Status Code + Informacion
        //Investigar los status code de http
        //200 aceptacion // Correcto

        //Route  ULR para acceder a este metodo
        [Route("api/Operaciones/Saludar")]
        [HttpGet]
        public IHttpActionResult Saludar(string nombre)
        {
            return Ok("Hola " + nombre);
        }

        [Route("api/Operaciones/Sumar")]
        [HttpPost]
        public IHttpActionResult Sumar(int numero1, int numero2)
        {
            return Ok(numero1+numero2);
        }
    }
}
