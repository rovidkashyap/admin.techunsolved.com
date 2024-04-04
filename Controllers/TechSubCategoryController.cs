using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechSubCategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";

        public TechSubCategoryController(HttpClient client)
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
                HttpResponseMessage response = await _httpClient.GetAsync("TechSubCategory/All");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techSubCategory = JsonConvert.DeserializeObject<List<TechSubCategoryModel>>(responseData);
                    return View(techSubCategory);
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
            // Fetch Category Names from the API
            var techCategories = await GetCategoryNames();

            // Check if tech categories collection is not null and not empty
            if (techCategories != null && techCategories.Any())
            {
                // Pass Category Names to the View
                ViewBag.CategoryNames = techCategories;
                
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechSubCategoryModel techSubcategory)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techSubcategory),
                    Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechSubCategory/Create", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Clear the Model
                    ModelState.Clear();

                    // Redirect to action after sucsessful creation
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
                // Send a GET request to retrieve the tech article by ID
                HttpResponseMessage response = await _httpClient.GetAsync($"TechSubCategory/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Check if the response is an array or a single object
                    if (responseData.StartsWith("["))
                    {
                        // If the response is an array, deserialize it into a list of TechSubCategoryModel
                        var techSubCategory = JsonConvert.DeserializeObject<List<TechSubCategoryModel>>(responseData);

                        // Find the article with the matching ID
                        var techsubCat = techSubCategory.FirstOrDefault(subCategory => subCategory.TechSubcategoryId == id);
                        ViewBag.TopicName = techsubCat.CategoryName;

                        if (techsubCat == null)
                        {
                            // Article not found, handle appropriately (e.g., return not found view)
                            return NotFound();
                        }

                        // Return the view with the retrieved techSubCategory for editing
                        return View(techsubCat);
                    }
                    else
                    {
                        // Deserialize the single object response into a TechSubCategoryModel
                        var techsubCategory = JsonConvert.DeserializeObject<TechSubCategoryModel>(responseData);

                        // Return the view with the retrieved technical article for editing
                        return View(techsubCategory);
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
        public async Task<IActionResult> Edit(int id, TechSubCategoryModel techSubCategory)
        {
            try
            {
                // Serialize the updated TechArticleModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techSubCategory),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the technical article
                var response = await _httpClient.PutAsync($"TechSubCategory/Update", jsonContent);

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

        // Method to fetch category names from the API
        private async Task<List<TechCategoryModel>> GetCategoryNames()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("TechCategory/All");

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<TechCategoryModel>>(responseData);
                return categories;
            }
            else
            {
                return new List<TechCategoryModel>();
            }
        }
    }
}
