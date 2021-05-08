using Newtonsoft.Json;
using PizzaManager.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using static System.Console;

namespace ProductManager
{
    partial class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:44321/api/");

            bool applicationRunning = true;

            var jwtToken = Login();

            Clear();

            var jwtHandler = new JwtSecurityTokenHandler();

            var jsonToken = jwtHandler.ReadToken(jwtToken);

            var token = jsonToken as JwtSecurityToken;

            var userName = token.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;

            WriteLine($"Welcome, {userName}");

            Thread.Sleep(2000);

            Clear();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            do
            {
                WriteLine("1. List Pizzas");

                WriteLine("2. Add New Pizza");

                WriteLine("3. Delete Pizza");

                WriteLine("4. Exit Application");

                CursorVisible = false;

                ConsoleKeyInfo input = ReadKey(true);

                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:

                        ListProducts();

                        break;

                    case ConsoleKey.D2:

                        AddProduct();

                        break;

                    case ConsoleKey.D3:

                        DeleteProduct();

                        break;

                    case ConsoleKey.D4:

                        applicationRunning = false;

                        break;
                }

                Clear();

            } while (applicationRunning);
        }

        private static void ListProducts()
        {
            var response = httpClient.GetAsync("Pizzas")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .Result;

                var pizzas = JsonConvert.DeserializeObject<IEnumerable<Pizzas>>(jsonString);

                foreach (var pizza in pizzas)
                {
                    WriteLine($"{pizza.Id}\t\t{pizza.PizzaName}\t\t{pizza.Price}");
                }
            }

            ReadKey(true);
        }

        private static void AddProduct()
        {
            CursorVisible = true;

            Write("Pizza Name: ");

            var pizzaName = ReadLine();

            Write("Pizza Sizes: ");

            var pizzaSizes = ReadLine();

            Write("Price: ");

            var price = decimal.Parse(ReadLine());

            var newPizza = new Pizzas(pizzaName, pizzaSizes, price);

            var serializedPizza = JsonConvert.SerializeObject(newPizza);

            var data = new StringContent(serializedPizza, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("pizzas", data).Result;

            if (response.IsSuccessStatusCode)
            {
                WriteLine("Pizza Successfully Added");
            }
            else
            {
                WriteLine("Try again, invalid data input.");
            }

            Thread.Sleep(2000);
        }

        private static void ListProductsWithinDelete()
        {
            var response = httpClient.GetAsync("Pizzas")
                .GetAwaiter()
                .GetResult();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync()
                    .Result;

                var pizzas = JsonConvert.DeserializeObject<IEnumerable<Pizzas>>(jsonString);

                foreach (var pizza in pizzas)
                {
                    WriteLine($"{pizza.Id}\t\t{pizza.PizzaName}\t\t{pizza.Price}");
                }
            }

            WriteLine(" ");
        }

        private static void DeleteProduct()
        {
            ListProductsWithinDelete();

            Write("Enter ID of Pizza You'd Like To Delete: ");

            CursorVisible = true;

            string urlId = ReadLine();

            var response = httpClient.DeleteAsync($"pizzas/{urlId}").Result;

            if (response.IsSuccessStatusCode)
            {
                WriteLine("Product Successfully Deleted");
            }
            else
            {
                WriteLine("An Error Has Been Encountered. Either Product Does Not Exist or Runtime Error.");
            }

            Thread.Sleep(2000);
        }

        // Login Method
        private static string Login()
        {
            bool notLoggedIn = true;

            string token = "";

            do
            {
                Clear();

                WriteLine("Enter Admin Credentials to Login");

                Write("Username: ");

                string username = ReadLine();

                Write("Password: ");

                string password = ReadLine();

                var creds = new Credentials
                {
                    Username = username,
                    Password = password
                };

                var serializedData = JsonConvert.SerializeObject(creds);

                var data = new StringContent(serializedData, Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync("login", data).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;

                    token = json;

                    notLoggedIn = false;
                }
                else
                {
                    Clear();

                    WriteLine("Login Failed. Try Again.");

                    Thread.Sleep(2000);
                }

            } while (notLoggedIn);

            return token;
        }
    }
}
