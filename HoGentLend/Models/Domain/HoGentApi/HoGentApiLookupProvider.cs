using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HoGentLend.Models.Domain.HoGentApi
{
    public class HoGentApiLookupProvider : IHoGentApiLookupProvider
    {
        private const string API_URL_FORMAT = "https://studservice.hogent.be/auth/{0}/{1}";

        public HoGentApiLookupResult Lookup(string userId, string unhashedPassword)
        {
            if (userId == null || unhashedPassword == null || userId.Trim().Length == 0 || unhashedPassword.Trim().Length == 0)
            {
                throw new ArgumentException("userId en password zijn verplicht");
            }
            string hashedPassword = HGLUtils.ToSHA256Hash(unhashedPassword);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format(API_URL_FORMAT, userId, hashedPassword));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content.ReadAsStringAsync().Result;
                var lookupResult = new HoGentApiLookupResult();
                JObject jsonObject;
                try
                {
                    jsonObject = JObject.Parse(jsonString);
                }
                catch (JsonReaderException e)
                {
                    return lookupResult;
                }


                foreach (KeyValuePair<String, JToken> jsonEntry in jsonObject)
                {
                    switch (jsonEntry.Key)
                    {
                        case "FACULTEIT":
                            lookupResult.Faculteit = jsonEntry.Value.Value<string>();
                            break;
                        case "NAAM":
                            lookupResult.LastName = jsonEntry.Value.Value<string>();
                            break;
                        case "TYPE":
                            lookupResult.Type = jsonEntry.Value.Value<string>();
                            break;
                        case "VOORNAAM":
                            lookupResult.FirstName = jsonEntry.Value.Value<string>();
                            break;
                        case "EMAIL":
                            lookupResult.Email = jsonEntry.Value.Value<string>();
                            break;
                        case "BASE64FOTO":
                            lookupResult.Base64Foto = jsonEntry.Value.Value<string>();
                            break;
                    }
                }
                return lookupResult;
            }
            else
            {
                throw new HttpRequestException(string.Format("De hogent API lookup was niet succesvol. ({0}, {1})", (int)response.StatusCode, response.ReasonPhrase));
            }
        }
    }
}