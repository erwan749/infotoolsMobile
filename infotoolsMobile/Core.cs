﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Globalization;
using Microsoft.Maui.ApplicationModel.Communication;

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

        public static async Task<List<Client>> GetClients()
        {
            HttpClient client = new HttpClient();
            string apiUrl = "http://infotools.test/api/clients";
            string token = await SecureStorage.GetAsync("UserToken");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            try
            {
                HttpResponseMessage reponse = await client.GetAsync(apiUrl);
                if (reponse.IsSuccessStatusCode)
                {
                    string jsonResponse = await reponse.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    if (apiResponse.succes == true)
                    {
                        List<Client> clients = new List<Client>();

                        foreach(var clientData in apiResponse.data)
                        {
                            clients.Add(new Client
                            {
                                id = clientData.id,
                                name = clientData.name.nom + " " + clientData.name.prenom,
                            });
                        }
                        return clients;
                    }
                }
                else
                {
                    Console.WriteLine("Échec de la connexion, statut: " + reponse.StatusCode);
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

        public static async Task<bool> AddRdv(Client unClient , string uneDate)
        {
            HttpClient client = new HttpClient();
            string apiUrl = "http://infotools.test/api/rdvs";
            string token = await SecureStorage.GetAsync("UserToken");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var value = new
            {
                DateRdv = uneDate,
                NoClient = Convert.ToString(unClient.id)
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    return apiResponse.success;
                }
                
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Erreur de connexion: {ex.Message}");
            }
            return false;
        }
        public static async Task<bool> UpdateRdv(Client unClient, string uneDate, int rdvId)
        {
            HttpClient client = new HttpClient();
            string apiUrl = $"http://infotools.test/api/rdvs/{rdvId}";  // Utiliser l'ID du rendez-vous pour l'URL
            string token = await SecureStorage.GetAsync("UserToken");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var value = new
            {
                DateRdv = uneDate,  // La nouvelle date du rendez-vous
                NoClient = Convert.ToString(unClient.id)  // ID du client associé au rendez-vous
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

            try
            {
                // Utilisation de la méthode PUT pour mettre à jour le rendez-vous existant
                HttpResponseMessage response = await client.PutAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                    return apiResponse.success;  // Retourner true si la mise à jour a réussi
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion: {ex.Message}");
            }

            return false;  // Retourner false en cas d'échec
        }


        public static async Task<bool> DeleteRdv(int id)
        {
            string apiUrl = "http://infotools.test/api/rdvs";
            string token = await SecureStorage.GetAsync("UserToken");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Ajouter le token d'authentification
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // Effectuer la requête DELETE pour supprimer le rendez-vous
                    HttpResponseMessage response = await client.DeleteAsync($"{apiUrl}/{id}");

                    // Vérifier si la réponse est réussie
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la suppression du rendez-vous : " + ex.Message);
                return false;
            }
        }
    }
}
