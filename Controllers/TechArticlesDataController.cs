using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechArticlesDataController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";

        public TechArticlesDataController(HttpClient client)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("techArticlesData/GetAllWithArticles");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techArticleData = JsonConvert.DeserializeObject<List<TechArticleDataModel>>(responseData);

                    return View(techArticleData);
                }
                else
                {
                    // Handle unsucessful API Response
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechArticleDataModel techArticleData)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techArticleData),
                    Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechArticlesData/Create", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Clear the Model
                    ModelState.Clear();

                    // Redirect to action after successful creation
                    return RedirectToAction("GetAll");
                }
                else
                {
                    // Log the response content if available
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HttpRequestException: {ex.Message}");

                // Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"TechArticlesData/{id}");

                if(response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Check if the response is an array or a single object
                    if (responseData.StartsWith("["))
                    {
                        var techArticlesData = JsonConvert.DeserializeObject<List<TechArticleDataModel>>(responseData);

                        // Find the article with the matching ID
                        var techArticleData = techArticlesData.FirstOrDefault(articleData => articleData.TechArticlesDataId == id);
                        ViewBag.TopicName = techArticleData.TechArticlesNames;

                        if(techArticleData == null)
                        {
                            //Article not found, handle appropriately (e.g., return not found view)
                            return NotFound();
                        }

                        // Return the view with the retrieved techArticleData for editing
                        return View(techArticleData);
                    }
                    else
                    {
                        // Deserialize the single object response into a TechArticleDataModel
                        var techArticleData = JsonConvert.DeserializeObject<TechArticleDataModel>(responseData);

                        // Return the view with the retrieved technical article for editing
                        return View(techArticleData);
                    }
                }
                else
                {
                    // Handle unsucessful API response
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HttpRequestException: {ex.Message}");

                // Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TechArticleDataModel techDataArticle)
        {
            try
            {
                // Serialize the updated TechArticleModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techDataArticle),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the technical article
                var response = await _httpClient.PutAsync($"TechArticlesData/Update", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to action after successful update
                    return RedirectToAction("GetAll");
                }
                else
                {
                    // Log the response content if available
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HttpRequestException: {ex.Message}");

                // Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetArticles()
        {
            // Fetch Data for the First Dropdown asynchronously
            var response = await _httpClient.GetAsync("TechArticle/All");
            response.EnsureSuccessStatusCode();

            var articleJson = await response.Content.ReadAsStringAsync();
            return Content(articleJson, "application/json");
        }
    }
}
