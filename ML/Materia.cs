using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Materia
    {
        //Atributos//Propiedades
        //Decoradores
        
        public int IdMateria { get; set; }
        [Required]  //Siempre debe tener ese dato
        [DisplayName("Nombre de la materia")]
        [MaxLength(100)]
        [MinLength(5)] //Rangos en longitud de cadenas
        [RegularExpression("[A-Z]")]
        public string Nombre { get; set; }

        [Required]  //Siempre debe tener ese dato
        [DisplayName("Creditos")]
        [Range(100, 200)] //Rango numerico
        public byte Creditos { get; set; }
        public decimal Costo { get; set; }
        public string Imagen { get; set; }
        public string Fecha { get; set; }
        public string Turno { get; set; }
        public bool Status { get; set; }
        //Propiedad de navegación //Llave foránea
        public ML.Semestre Semestre { get; set; }

        public List<object> Materias { get; set; }
       
     
    }
}
