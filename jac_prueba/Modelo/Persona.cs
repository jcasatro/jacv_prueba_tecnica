using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jac_prueba.Modelo
{
    public class Persona
    {
        public int IdPersona { get; set; } = -1;
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string DNI { get; set; } = "";
        public DateTime FechaNacimiento { get; set; } = new DateTime(1900,1,1);
        public string Email { get; set; } = ""; 
    }
}
