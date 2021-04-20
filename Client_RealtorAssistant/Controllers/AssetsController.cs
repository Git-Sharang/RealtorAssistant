using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Client_RealtorAssistant.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Client_RealtorAssistant.Controllers
{
    public class AssetsController : Controller
    {
        HttpClient client;
        string server;
        string apiKey;
        public AssetsController()
        {
            client = new HttpClient();
            server = "https://sverma62-eval-prod.apigee.net/realtor_assistant/api/Assets";
            apiKey = "apikey=q76bgUAfAzstOUKrwxD4VEJIdT7gqp5P";
        }

        // Displaying the List of Assets
        public async Task<IActionResult> AssetIndex()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            
            string url = server + "/"+ realtorId + "/assetSoldStatus?isAssetSold=false" + "&" + apiKey;
            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }

            return View(items);
        }

        // Displaying the List of Sold Assets
        public async Task<IActionResult> SoldProperties()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
         
            string url = server + "/" + realtorId + "/assetSoldStatus?isAssetSold=true" + "&" + apiKey;
            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        public async Task<IActionResult> Filter_Land()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");

            string url = server + "/" + realtorId + "/assetType?assetType=" + "Land&isAssetSold=false" + "&" + apiKey;

            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        public async Task<IActionResult> Filter_Condo()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");

            string url = server + "/" + realtorId + "/assetType?assetType=" + "Condo&isAssetSold=false" + "&" + apiKey;

            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        public async Task<IActionResult> Filter_Detached()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");

            string url = server + "/" + realtorId + "/assetType?assetType=" + "Detached&isAssetSold=false" + "&" + apiKey;

            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        public async Task<IActionResult> Filter_SemiDetached()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");

            string url = server + "/" + realtorId + "/assetType?assetType=" + "Semi&isAssetSold=false" + "&" + apiKey;

            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        public async Task<IActionResult> Filter_PentHouse()
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");

            string url = server + "/" + realtorId + "/assetType?assetType=" + "Penthouse&isAssetSold=false" + "&" + apiKey;

            IEnumerable<Asset> items = new List<Asset>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<Asset>>(json);
            }
            return View(items);
        }

        // Asset Details
        public async Task<IActionResult> Details(int? id)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            if (id == null)
            {
                return NotFound();
            }
            var assetItem = await AssetExists(realtorId, (int)id);
            if (assetItem == null)
            {
                return NotFound();
            }
            return View(assetItem);
        }

        // Create Assets for a Realtor
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Type,isOwned,isSold")] Asset asset)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            if (ModelState.IsValid)
            {
                string json = JsonConvert.SerializeObject(asset); //Converting the input into a JSON object

                string urlForPost = server + "/" + realtorId + "/asset" + "?" + apiKey;

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(urlForPost, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(AssetIndex));
                }
                return RedirectToAction(nameof(AssetIndex));
            }
            return View(asset);
        }

        // Edit the Asset
        public async Task<IActionResult> Edit(int? id)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            if (id == null)
            {
                return NotFound();
            }
            var assetItem = await AssetExists(realtorId, (int)id);
            if (assetItem == null)
            {
                return NotFound();
            }
            return View(assetItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Type,isOwned,isSold")] Asset asset)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            if (id != asset.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string urlForPut = server + "/" + realtorId + "/asset/" + asset.Id + "?" + apiKey;

                string json = JsonConvert.SerializeObject(asset); 
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(urlForPut, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(AssetIndex));
                }
            }
            return View(asset);
        }

        // Delete the Asset
        public async Task<IActionResult> Delete(int? id)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            if (id == null)
            {
                return NotFound();
            }
            var assetItem = await AssetExists(realtorId, (int)id);
            if (assetItem == null)
            {
                return NotFound();
            }
            return View(assetItem);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int realtorId = (int)HttpContext.Session.GetInt32("realtorId");
            string urlForDelete = server + "/" + realtorId + "/asset/" + id + "?" + apiKey;
            await client.DeleteAsync(urlForDelete);
            return RedirectToAction(nameof(AssetIndex));
        }


        //Logout
        public IActionResult Logout()
        {
            //Clearing the variable that was stored in the session when the logout button is pressed
            HttpContext.Session.Clear();
            return RedirectToAction("First", "Home");
        }

        //We can you this method to the get the Asset of a specific Realtor ID
        private async Task<Asset> AssetExists(int realtorId, int assetId)
        {
            string url = server + "/" + realtorId + "/asset/" + assetId + "?" + apiKey;
         
            Asset item = new Asset();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                item = JsonConvert.DeserializeObject<Asset>(json);
            }
            return item;
        }
    }
}
