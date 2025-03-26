using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;

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

        public static async Task<List<Rdv>> getRdv()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "http://infotools.test/api/rdvs";
            string token = await SecureStorage.GetAsync("UserToken");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    // Vérification du succès de la réponse
                    if (apiResponse.succes == true)
                    {
                        List<Rdv> rdvList = new List<Rdv>();

                        // Boucle sur chaque rendez-vous retourné dans "data"
                        foreach (var rdvData in apiResponse.data)
                        {
                            string dateFormat = "yyyy-MM-dd HH:mm:ss";
                            DateTime dateRdv;

                            if (DateTime.TryParseExact(rdvData.date.ToString(), dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateRdv))
                            {
                                rdvList.Add(new Rdv
                                {
                                    Id = rdvData.id,
                                    Client = rdvData.client.nom + " " + rdvData.client.prenom,
                                    NameCom = rdvData.commercial.name,
                                    DateRdv = dateRdv  
                                });
                            }
                            else
                            {

                                Console.WriteLine($"Erreur de format pour la date du rendez-vous ID: {rdvData.id}");
                            }
                        }


                        return rdvList;
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
