using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace infotoolsMobile
{
    internal class Core
    {
        public static async Task<user> GetUser(string email, string password)
        {
            HttpClient client = new HttpClient();
            string apiUrl = "http://infotools.test/api/login";

            var loginData = new
            {
                email = email,
                psw = password
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    if (apiResponse.success == true)
                    {
                        return new user
                        {
                            Id = apiResponse.data.idUser,
                            name = apiResponse.data.name.ToString(),
                            token = apiResponse.data.token.ToString()
                        };
                    }
                }
                else
                {
                    Console.WriteLine("Échec de la connexion, statut: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion: {ex.Message}");
            }

            return null;
        }

    }
}
