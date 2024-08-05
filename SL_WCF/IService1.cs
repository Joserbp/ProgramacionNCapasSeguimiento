using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SL_WCF
{
    [ServiceContract] // Contrato de servicios a exponer
    public interface IService1
    {
        //Firma de metodos
        //Tipo dato retorno + Nombre + Parametros (Opcionales)

        [OperationContract] //Metodo a exponer de nuestro servicio
        string Saludar(string nombre);

        [OperationContract]
        int Sumar(int numero1, int numero2);
        [OperationContract]
        int Restar(int numero1, int numero2);
    }
}
