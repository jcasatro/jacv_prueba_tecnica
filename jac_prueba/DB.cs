using Amazon;
using Amazon.Lambda.Core;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using jac_prueba.Modelo;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace jac_prueba
{
    public class DB
    {
        public static bool Sp_jac_persona(Persona persona, int idOpcion, MySqlConnection sqlConnection, ref List<Persona> DataPersona)
        {
            try
            {
                DataPersona = [];

                var cmd = new MySqlCommand("sp_jac_persona", sqlConnection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ck_IdOpcion", idOpcion);
                cmd.Parameters.AddWithValue("@Ck_IdPersona", persona.IdPersona);
                cmd.Parameters.AddWithValue("@Ck_Nombre", persona.Nombre);
                cmd.Parameters.AddWithValue("@Ck_Apellido", persona.Apellido);
                cmd.Parameters.AddWithValue("@Ck_DNI", persona.DNI);
                cmd.Parameters.AddWithValue("@Ck_FechaNacimiento", persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Ck_Email", persona.Email);
                if (sqlConnection.State == System.Data.ConnectionState.Open) { sqlConnection.Close(); }
                sqlConnection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
              
                while (reader.Read())
                {
                    switch (idOpcion)
                    {
                        case 4:
                            var p = new Persona();
                            p.IdPersona = int.Parse(reader["nIdPersona"].ToString());
                            p.Nombre = reader["sNombre"].ToString();
                            p.Apellido = reader["sApellido"].ToString();
                            p.DNI = reader["sDNI"].ToString();
                            p.FechaNacimiento = DateTime.Parse(reader["dFechaNacimiento"].ToString());
                            p.Email = reader["sEmail"].ToString();
                            DataPersona.Add(p);
                            break;
                        case 2 or 3:
                            int filasAfectadas = int.Parse(reader["filasAfectadas"].ToString());
                            if (filasAfectadas <= 0)
                                return false;
                            if (idOpcion == 2)
                                DataPersona.Add(persona);
                            break;
                        case 1:
                            int idPersona = int.Parse(reader["idPersona"].ToString());
                            if (idPersona <= 0)
                                return false;
                            persona.IdPersona = idPersona;
                            DataPersona.Add(persona);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log("Error al ejecutar el sp " + ex.Message);
                return false;
            }

            return true;
        }

        public static MySqlConnection Connection(string secretName, string region, string schema, string VersionStage)
        {


            MySqlConnection conn = null;
            DBCredentials dBCredentials = null;
            bool b = GetDBCredentials(secretName, region, ref dBCredentials, VersionStage);
            if (!b)
            {
                LambdaLogger.Log("No se logró obtener datos del servidor bd desde el secret");
                return conn;
            }

            LambdaLogger.Log(String.Format("CreateConn: {0}|{1}|{2}|{3}|{4}|{5}|{6}", dBCredentials.host, schema, schema, "***", "***", dBCredentials.host, DateTime.Now.ToString()));
            try
            {
                conn = new MySqlConnection(String.Format("server={0};user={1};database={2};port={3};password={4};SslMode=Required;SslCa=../rds-ca-2019-root.pem", dBCredentials.host, dBCredentials.username, schema, dBCredentials.port, dBCredentials.password));
                conn.Open();
                conn.Close();
                LambdaLogger.Log("Conexión exitosa");
                return conn;
            }
            catch (Exception e)
            {
                conn = null;
                LambdaLogger.Log("Conexión fallida " + e.ToString());
                return conn;
            }

        }
        private static bool GetDBCredentials(string secretName, string region, ref DBCredentials credentials, string VersionStage)
        {
            credentials = new();
            LambdaLogger.Log(string.Concat("Inicia GetDBCredentials [", secretName, "]", "[", region, "] ", DateTime.Now));
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            GetSecretValueRequest request = new()
            {
                SecretId = secretName,
                VersionStage = VersionStage
            };

            GetSecretValueResponse response;
            try
            {
                response = client.GetSecretValueAsync(request).GetAwaiter().GetResult();
                credentials = JsonConvert.DeserializeObject<DBCredentials>(response.SecretString);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(string.Concat("Error al obtener el secreto " + ex.Message, DateTime.Now));
                return false;
            }

            return true;
        }
    }
}
