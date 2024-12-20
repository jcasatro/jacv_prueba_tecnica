using Amazon.Lambda.Core;
using jac_prueba.Modelo;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace jac_prueba;

public class Function
{
    public Response Personas(Request input, ILambdaContext context)
    {
        string SecretName = "Secret_PCI_SB";
        string VersionStage = "AWSCURRENT";
        string Schema = "paycash_middle_sb";

        string awsRegion = context.InvokedFunctionArn.Split(':')[3];

        LambdaLogger.Log("Inicia la peticioón " + input.ToJson());
        if (input.Opcion < 1 || input.Opcion > 4)
            return new Response() { errorCode = "100", errorMessage = "Opción erronea" };


        if (input.Opcion == 1 || input.Opcion == 2)
        {
            if (input.FechaNacimiento > DateTime.Now.Date)
                return new Response() { errorCode = "102", errorMessage = "Fecha de nacimiento no puede ser mayor a la fecha actual" };

            if (input.FechaNacimiento < new DateTime(year: 1900, month: 1, day: 1))
                return new Response() { errorCode = "102", errorMessage = "Fecha de nacimiento no puede ser menor a 1900-01-01" };

            if (input.Email.Trim() != "")
                if (!IsValidEmail(input.Email))
                    return new Response() { errorCode = "103", errorMessage = "Email no tiene el formato correcto" };

            if (input.Nombre.Trim() == "")
                return new Response() { errorCode = "103", errorMessage = "Debe proporcionar un nombre" };

            if (input.Apellido.Trim() == "")
                return new Response() { errorCode = "103", errorMessage = "Debe proporcionar un apellido" };

            if (input.DNI.Trim() == "")
                return new Response() { errorCode = "103", errorMessage = "Debe proporcionar un DNI" };
        }
        LambdaLogger.Log("Obtiene las credenciales y cadena de conexión");

        MySqlConnection conecta = DB.Connection(SecretName, awsRegion, Schema, VersionStage);
        if (conecta == null)
            return new Response() { errorCode = "500", errorMessage = "Error inesperado [1]" };

        LambdaLogger.Log("Ejecuta Sp ");
        List<Persona> personas = [];
        bool b = DB.Sp_jac_persona(input, input.Opcion, conecta, ref personas);
        if (input.Opcion == 2 || input.Opcion == 3 || input.Opcion == 1)
        {
            if (!b)
                return new Response()
                {
                    errorCode = "101",
                    errorMessage = "No se logró actualizar el registro, o el id ["+ input.IdPersona +"] se encuentra inactivo"
                };
        }

        if (input.Opcion == 4 && !b)
            return new() { errorCode = "500", errorMessage = "Error inesperado [2]" };

        return new Response()
        {
            errorCode = "200",
            errorMessage = "Operación exitosa",
            Data = personas
        };
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
