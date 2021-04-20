using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Client_RealtorAssistant.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Client_RealtorAssistant.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        string server;
        string apiKey;
        public HomeController()
        {
            client = new HttpClient();
            server = "https://sverma62-eval-prod.apigee.net/realtor_assistant/api/Realtors";
            apiKey = "apikey=q76bgUAfAzstOUKrwxD4VEJIdT7gqp5P";
        }
        public IActionResult First()
        {
            return View();
        }

        //Realtor will login here
        public IActionResult RealtorLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealtorLogin([Bind("Username,Password")] Realtors realtorUser)
        {
            if (ModelState.IsValid && realtorUser != null)
            {
                Realtors userItem = new Realtors();
                try
                {
                    string usernameEntered = realtorUser.Username.ToString();
                    string passwordEntered = realtorUser.Password.ToString();
                    userItem = await RealtorExists(usernameEntered);
                    if (!(userItem == null))
                    {
                        if (userItem.Username.ToString() != usernameEntered || userItem.Password.ToString() != passwordEntered)
                        {
                            ViewBag.result = "User not found.";
                        }
                        else if (userItem.Username.ToString() == usernameEntered && userItem.Password.ToString() == passwordEntered)
                        {
                            //Storing the Id of the realtor in the session, which will help to retrieve the Assets
                            HttpContext.Session.SetInt32("realtorId", userItem.Id);
                            return RedirectToAction("AssetIndex", "Assets");
                        } 
                       
                    }
                    else
                    {
                        ViewBag.result = "User does not exist, please register";
                    }
                }
                catch (Exception e)
                {
                    ViewBag.result = e.Message;
                }
            }
            ViewBag.result = "Username or Password are incorrect. Please try again.";
            return View("First");
        }

        // Realtor will register here
        public IActionResult RealtorRegister()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealtorRegister([Bind("Id,Username,Password,Firstname,Lastname")] Realtors realtors)
        {
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(realtors); //Converting the input into a JSON object
                
                //Passing the json object of our stuff, and the headers in the response
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                string url = server + "?" + apiKey;
                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(First));
                }
                return RedirectToAction(nameof(First));
            }
            return View(realtors);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        //We can you this method to the get the Item of a specific username
        private async Task<Realtors> RealtorExists(string username)
        {
            string url = server + "/realtorusername?username=" + username + "&includeAssets=false" + "&" + apiKey;
            Realtors item = new Realtors();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                item = JsonConvert.DeserializeObject<Realtors>(json);
            }
            return item;
        }
    }
}
