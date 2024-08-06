using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MateriaService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MateriaService.svc or MateriaService.svc.cs at the Solution Explorer and start debugging.
    public class MateriaService : IMateriaService
    {
        public Result Add(ML.Materia materia)
        {
            ML.Result result = BL.Materia.AddEFLINQ(materia); 
            return result;
        }

        public Result GetAll(ML.Materia materia)
        {
            ML.Result result = BL.Materia.GetAllSPEF(materia);
            return result;
        }
    }
}
