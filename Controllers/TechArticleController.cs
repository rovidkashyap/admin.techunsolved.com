using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechArticleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";

        public TechArticleController(HttpClient client)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("TechArticle/AllWithTopics");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techArticle = JsonConvert.DeserializeObject<List<TechArticleModel>>(responseData);

                    return View(techArticle);
                }
                else
                {
                    // Handle unsucessful API response
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
        public async Task<IActionResult> Create(TechArticleModel techArticle)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techArticle),
                    Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechArticle/Create", jsonContent);

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
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                // Send a GET request to retrieve the tech article by ID
                HttpResponseMessage response = await _httpClient.GetAsync($"TechArticle/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Check if the response is an array or a single object
                    if (responseData.StartsWith("["))
                    {
                        // If the response is an array, deserialize it into a list of TechArticleModel
                        var techArticles = JsonConvert.DeserializeObject<List<TechArticleModel>>(responseData);

                        // Find the article with the matching ID
                        var techArticle = techArticles.FirstOrDefault(article => article.TechArticleId == id);
                        ViewBag.TopicName = techArticle.TechTopicName;

                        if (techArticle == null)
                        {
                            // Article not found, handle appropriately (e.g., return not found view)
                            return NotFound();
                        }

                        // Return the view with the retrieved techArticle for editing
                        return View(techArticle);
                    }
                    else
                    {
                        // Deserialize the single object response into a TechArticleModel
                        var techArticle = JsonConvert.DeserializeObject<TechArticleModel>(responseData);

                        // Return the view with the retrieved technical article for editing
                        return View(techArticle);
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
        public async Task<IActionResult> Edit(int id, TechArticleModel techArticle)
        {
            try
            {
                // Serialize the updated TechArticleModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techArticle),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the technical article
                var response = await _httpClient.PutAsync($"TechArticle/Update", jsonContent);

                if(response.IsSuccessStatusCode)
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
        // Method to Fetch the TechModule and TechTopic Names
        public async Task<IActionResult> GetModules()
        {
            // Fetch Data for the first dropdown asynchronously
            var reponse = await _httpClient.GetAsync("TechModule/All");
            reponse.EnsureSuccessStatusCode();

            var modulesJson = await reponse.Content.ReadAsStringAsync();
            return Content(modulesJson, "application/json");

        }

        [HttpGet]
        public async Task<IActionResult> GetTopics(int moduleId)
        {
            // Fetch Data for the first dropdown asynchronously
            var reponse = await _httpClient.GetAsync($"TechTopic/{moduleId}");
            reponse.EnsureSuccessStatusCode();

            var topicJson = await reponse.Content.ReadAsStringAsync();
            return Content(topicJson, "application/json");

        }
    }
}
