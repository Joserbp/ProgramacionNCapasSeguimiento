using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.IO;

namespace PL_MVC.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {

            try
            {
                string pathCorreoTemplate = Server.MapPath("~/Content/CorreoTemplate/Correo.html");

                string body = string.Empty;

                using (StreamReader reader = new StreamReader(pathCorreoTemplate))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{nombreCandidato}", "<td></td>");
                body = body.Replace("{LINK}", "http://www.google.com");


                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("jguevaraflores3@gmail.com", "ywqezqcviboahahp"),
                    EnableSsl = true
                };

                var mensaje = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("jguevaraflores3@gmail.com", "displayName"),
                    Subject = "Cita Agendada",
                    Body = body,
                    IsBodyHtml = true
                };

                mensaje.To.Add("jguevaraflores3@gmail.com");
                smtpClient.Send(mensaje);

            } catch {
                //MODAL
            }

            return View();
        }

        public ActionResult SendEmail()
        {
            return View();
        }

        public ActionResult Enviar()
        {

            string path = @"C:\Users\digis\Downloads\JGuevaraProgramacionNCapas (3)\JGuevaraProgramacionNCapas\PL_MVC\Content\CorreoTemplate\Correo.html";

            string body = string.Empty;
            using (StreamReader leer = new StreamReader(path))
            {
                body = leer.ReadToEnd();
            }

            //codigo para enviar el EMAIL
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jguevaraflores3@gmail.com", "dxyevrhvlmemuqjy"),
                EnableSsl = true
            };

            var mensaje = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress("jguevaraflores3@gmail.com", "TEST .NET MAYO"),
                Subject = "Correo de prueba Jorge Guevara Flores",
                Body = body,
                IsBodyHtml = true
            };


            mensaje.To.Add("jtorres@digis01.com");
            mensaje.CC.Add("jguevaraflores@gmail.com");

            smtpClient.Send(mensaje);

            return View();
        }


    }
}