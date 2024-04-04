using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechModuleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";

        public TechModuleController(HttpClient client)
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
                HttpResponseMessage response = await _httpClient.GetAsync("TechModule/All");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techModule = JsonConvert.DeserializeObject<List<TechModuleModel>>(responseData);

                    return View(techModule);
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
            // Fetch SubCategory Names from the API
            var techSubCategories = await GetSubCategoryNames();

            // Check if Subcategories collection is not null and not empty
            if(techSubCategories != null && techSubCategories.Any())
            {
                ViewBag.SubCategoryNames = techSubCategories;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechModuleModel techModule)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techModule),
                    Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechModule/Create", jsonContent);

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
                // Send a GET request to retrieve the tech module by ID
                HttpResponseMessage response = await _httpClient.GetAsync($"TechModule/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Check if the response is an array or a single object
                    if (responseData.StartsWith("["))
                    {
                        // If the response is an array, deserialize it into a list of TechArticleModel
                        var techModules = JsonConvert.DeserializeObject<List<TechModuleModel>>(responseData);

                        // Find the article with the matching ID
                        var techModule = techModules.FirstOrDefault(module => module.TechModulesId == id);
                        ViewBag.TopicName = techModule.TechSubCategoryName;

                        if (techModule == null)
                        {
                            // Article not found, handle appropriately (e.g., return not found view)
                            return NotFound();
                        }

                        // Return the view with the retrieved techArticle for editing
                        return View(techModule);
                    }
                    else
                    {
                        // Deserialize the single object response into a TechArticleModel
                        var techModule = JsonConvert.DeserializeObject<TechModuleModel>(responseData);

                        // Return the view with the retrieved techModule for editing
                        return View(techModule);
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
        public async Task<IActionResult> Edit(int id, TechModuleModel techModule)
        {
            try
            {
                // Serialize the updated TechModuleModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techModule),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the techModule
                var response = await _httpClient.PutAsync($"TechModule/Update", jsonContent);

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

        // Method to fetch the SubCategory names from the API
        private async Task<List<TechSubCategoryModel>> GetSubCategoryNames()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("TechSubCategory/All");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var subCategories = JsonConvert.DeserializeObject<List<TechSubCategoryModel>>(responseData);

                return subCategories;
            }
            else
            {
                return new List<TechSubCategoryModel>();
            }
        }
    }
}
