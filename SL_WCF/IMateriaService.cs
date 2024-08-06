using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SL_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMateriaService" in both code and config file together.
    [ServiceContract]
    public interface IMateriaService
    {
        [OperationContract]
        //tipo de retorno + nombre + parametros
        ML.Result Add(ML.Materia materia);

        [OperationContract]
        [ServiceKnownType(typeof(ML.Materia))]
        ML.Result GetAll(ML.Materia materia);
    }
}
