using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using PasswordStrengthChecker.BusinessLogic.Interface;
using PasswordStrengthChecker.BusinessLogic.Model;
using System.Linq;

namespace PasswordStrengthChecker.BusinessLogic
{
    public class DataBreachChecker : IPasswordBreachChecker
    {
        private static readonly HttpClient client = new HttpClient();
        public string ExternalApiURL { get; }
        public string UserAgent { get; }
        public string HIBPApiKey { get; }
        public string PasswordRangeURL { get; }

        public DataBreachChecker(string externalApiURL, string userAgent, string hibpApiKey, string passwordRangeURL)
        {
            ExternalApiURL = externalApiURL;
            UserAgent = userAgent;
            HIBPApiKey = hibpApiKey;
            PasswordRangeURL = passwordRangeURL;
        }

        public async Task<int> CheckIfPasswordPwned(string password)
        {
            // Compute the SHA1 hash of the string
            SHA1 sha1 = SHA1.Create();
            byte[] byteString = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha1.ComputeHash(byteString);
            string hashString = "";
            StringBuilder sb = new StringBuilder();
            int DataBreachCount = 0;

            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            hashString = sb.ToString();

            // Break the SHA1 into two pieces:
            //   1) the first five characters of the hash
            //   2) the rest of the hash
            string hashFirstFive = hashString.Substring(0, 5);
            string hashLeftover = hashString.Substring(5, hashString.Length - 5);

            string api = "range";
            var response = await GETRequestAsync($"{api}/{hashFirstFive}", PasswordRangeURL);
            var responseContainsHash = response.Body.Contains(hashLeftover);

            if (responseContainsHash)
            {                
                string[] strResponse = response.Body.Split( new[] { Environment.NewLine }, StringSplitOptions.None );
                foreach (string hash in strResponse)
                {
                    if (hash.Contains(hashLeftover))
                    {
                        hash.Replace("\r","");
                        string[] strHash = hash.Split(':');
                        if(strHash.Length >0)
                        {
                            bool result = int.TryParse(strHash[1], out DataBreachCount);                            
                        }
                    }
                }
                return DataBreachCount;
            }
            return DataBreachCount;
        }

        private async Task<Response> GETRequestAsync(string parameters)
        {
            Response response = await GETRequestAsync(parameters, ExternalApiURL);
            return response;
        }

        private async Task<Response> GETRequestAsync(string parameters, string overrideURL)
        {
            Response RestResponse = new Response();
            Uri uri = new Uri($"{overrideURL}/{parameters}");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("hibp-api-key", HIBPApiKey);

            HttpResponseMessage response = null;
            request.Headers.TryAddWithoutValidation("User-Agent", UserAgent);

            try
            {
                response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                string statusCode = response.StatusCode.ToString();

                RestResponse.Body = responseBody;
                RestResponse.StatusCode = statusCode;

                return RestResponse;
            }
            catch (HttpRequestException e)
            {
                RestResponse.Body = null;
                if (response != null) RestResponse.StatusCode = response.StatusCode.ToString();
                RestResponse.HttpException = e.Message;
                return RestResponse;
            }
        }


    }
}
