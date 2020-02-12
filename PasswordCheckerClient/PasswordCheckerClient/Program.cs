using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PasswordCheckerClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string URL = "https://localhost:44304/api/PasswordChecker/";

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter a password");
            string password = Console.ReadLine();
            RunAsync(password).GetAwaiter().GetResult();
            Console.ReadLine();
        }
        /// <summary>
        /// Calls the rest api with the user supplied password and gets the strength of the password as result from api.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        static async Task<string> GetPasswordStrengthAsync(string password)
        {
            HttpResponseMessage response = await client.GetAsync(URL + "/CheckStrength/" + password);
            string strength = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                strength = await response.Content.ReadAsAsync<string>();
            }
            return strength;
        }
        /// <summary>
        /// Calls the rest api with the user supplied password to check for the data breach.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        static async Task<int> GetPasswordBreachCountAsync(string password)
        {
            HttpResponseMessage response = await client.GetAsync(URL + "/CheckDataBreach/" + password);
            int index = 0;
            if (response.IsSuccessStatusCode)
            {
                index = await response.Content.ReadAsAsync<int>();
            }
            return index;
        }

        static async Task RunAsync(string password)
        {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                string strength = await GetPasswordStrengthAsync(password);
                int dataBreachCount = await GetPasswordBreachCountAsync(password);

                Console.WriteLine("The password is " + strength);

                if (dataBreachCount > 0)
                    Console.WriteLine("The password is appeared in the breached list " + dataBreachCount + " times.");
                else
                    Console.WriteLine("The password entered is not found in breached list");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
