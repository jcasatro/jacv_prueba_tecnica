using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace jac_prueba.Modelo
{
    public class Response
    {
        public string errorCode { get; set; } = "101";
        public string errorMessage { get; set; } = "Error al ejecutar la petición";
        public List<Persona> Data { get; set; } = null;
    }       
}
