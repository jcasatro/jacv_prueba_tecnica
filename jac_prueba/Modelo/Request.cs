using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jac_prueba.Modelo
{
    public class Request: Persona
    {
        public int Opcion { get; set; } = 5;

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
