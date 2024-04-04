using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechTopicController : Controller
    {
        private readonly HttpClient _httpClient;
        private const int PageSize = 20; // Number of records per page
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";

        public TechTopicController(HttpClient client)
        {
            _httpClient = client;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 20)
        {
            try
            {
                // Get total count from seperate API endpoint
                HttpResponseMessage countReponse;
                try
                {
                    countReponse = await _httpClient.GetAsync("TechTopic/allCount");
                }
                catch(HttpRequestException)
                {
                    // Handle HttpRequestException, return 0 count if connection to API is not available
                    ViewBag.TechTopics = new List<TechTopicModel>();
                    ViewBag.TotalCount = 0;
                    ViewBag.TotalPages = 0;
                    ViewBag.CurrentPage = 0;
                    ViewBag.StartPage = 0;
                    ViewBag.EndPage = 0;
                    return View(new List<TechTopicModel>());
                }

                if(!countReponse.IsSuccessStatusCode)
                {
                    // Handle unsuccessfil response for count endpoint
                    return StatusCode((int)countReponse.StatusCode);
                }

                var countData = await countReponse.Content.ReadAsStringAsync();
                var totalCount = JsonConvert.DeserializeObject<int>(countData);
        
                HttpResponseMessage response = await _httpClient.GetAsync($"TechTopic/GetAllTopics?page={page}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techTopic = JsonConvert.DeserializeObject<List<TechTopicModel>>(responseData);

                    // Calculate total pages
                    var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                    // Calculate StartPage and EndPage for Pagination
                    var maxPagesToShow = 20;
                    var startPage = Math.Max(1, page - (maxPagesToShow / 2));
                    var endPage = Math.Min(totalPages, startPage + maxPagesToShow - 1);

                    // pass the pagination metadata and data to the view via ViewBag
                    ViewBag.TechTopics = techTopic;
                    ViewBag.TotalCount = totalCount;
                    ViewBag.TotalPages = totalPages;
                    ViewBag.CurrentPage = page;
                    ViewBag.StartPage = startPage;
                    ViewBag.EndPage = endPage;

                    return View(techTopic);
                }
                else
                {
                    // Handle unsucessful API response
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetTopics(int techModulesId)
        {
            try
            {
                if(techModulesId == null)
                {
                    //Return an empty view if no module is selected
                    return View(new List<TechTopicModel>());
                }

                HttpResponseMessage response = await _httpClient.GetAsync($"TechTopic/{techModulesId}");

                if(response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techTopic = JsonConvert.DeserializeObject<List<TechTopicModel>>(responseData);
                   
                    return View(techTopic);
                }
                else
                {
                    // Handle unsuccessful API response
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch Modules Names from the API
            var techModule = await GetModuleNames();

            // Check if Subcategories collection is not null and not empty
            if (techModule != null && techModule.Any())
            {
                ViewBag.ModuleNames = techModule;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechTopicModel techTopic)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techTopic),
                    Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechTopic/Create", jsonContent);

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
                HttpResponseMessage response = await _httpClient.GetAsync($"TechTopic/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Check if the response is an array or a single object
                    if (responseData.StartsWith("["))
                    {
                        // If the response is an array, deserialize it into a list of TechTopicModel
                        var techTopics = JsonConvert.DeserializeObject<List<TechTopicModel>>(responseData);

                        // Find the article with the matching ID
                        var techTopic = techTopics.FirstOrDefault(topic => topic.TechTopicId == id);
                        ViewBag.TopicName = techTopic.TechModulesName;

                        if (techTopic == null)
                        {
                            // Article not found, handle appropriately (e.g., return not found view)
                            return NotFound();
                        }

                        // Return the view with the retrieved techArticle for editing
                        return View(techTopic);
                    }
                    else
                    {
                        // Deserialize the single object response into a TechTopicModel
                        var techTopic = JsonConvert.DeserializeObject<TechTopicModel>(responseData);

                        // Return the view with the retrieved techTopic for editing
                        return View(techTopic);
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
        public async Task<IActionResult> Edit(int id, TechTopicModel techTopic)
        {
            try
            {
                // Serialize the updated TechTopicModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techTopic),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the techTopic
                var response = await _httpClient.PutAsync($"TechTopic/Update", jsonContent);

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

        // Method to fetch the Module names from the API
        private async Task<List<TechModuleModel>> GetModuleNames()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("TechModule/All");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var techModule = JsonConvert.DeserializeObject<List<TechModuleModel>>(responseData);

                return techModule;
            }
            else
            {
                return new List<TechModuleModel>();
            }
        }
    }
}
