using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class TechCategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";
        public TechCategoryController(HttpClient client)
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
                HttpResponseMessage response = await _httpClient.GetAsync("TechCategory/All");

                if(response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var techCategory = JsonConvert.DeserializeObject<List<TechCategoryModel>>(responseData);
                    return View(techCategory);
                }
                else
                {
                    // Handle unsuccessful API response
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                // Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechCategoryModel techCategory)
        {
            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techCategory), System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("TechCategory/Create", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Clear the Model
                    ModelState.Clear();

                    // Redirect to action after successful creation
                    return RedirectToAction("GetAll");
                }
                else
                {
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                // Internal Server Error
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                // Send a GET request to retrieve the category by ID
                HttpResponseMessage response = await _httpClient.GetAsync($"TechCategory/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into a TechCategoroy object
                    var techCategory = JsonConvert.DeserializeObject<TechCategoryModel>(responseData);

                    if (techCategory == null)
                    {
                        // Category not found, handle appropriately
                        return NotFound();
                    }

                    // Return the view with the retrieved category for editing
                    return View(techCategory);
                }
                else
                {
                    // Handle unsuccessful API response
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
        public async Task<IActionResult> Edit(int id, TechCategoryModel techCategory)
        {
            try
            {
                // Check if the provided ID matches the model ID
                if(id != techCategory.TechCategoryId)
                {
                    return BadRequest();
                }

                // Serialize the updated TechCategoryModel object into JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(techCategory),
                    Encoding.UTF8, "application/json");

                // Send a PUT request to update the techCategory
                var response = await _httpClient.PutAsync($"TechCategory/Update", jsonContent);

                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetAll");
                }
                else
                {
                    // Log the response Content if available
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode);
                }
            }
            catch(HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"HttpRequestException: {ex.Message}");

                // Internal Server Error
                return StatusCode(500);
            }
        }
    }
}
