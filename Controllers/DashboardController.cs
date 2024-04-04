using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using admin.techunsolved.com.Models;

namespace admin.techunsolved.com.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string BaseUrl = "https://api.techunsolved.com/api/";
        public async Task<IActionResult> Index()
        {
            // Fetch the Topic count
            int csharparticleCount = await GetTopicCount("c-sharp");
            int mvcArticleCount = await GetTopicCount("mvc");
            int webapiArticleCount = await GetTopicCount("web-api");
            int javaArticleCount = await GetTopicCount("java");
            int bootstrapArticleCount = await GetTopicCount("bootstrap");
            int reactArticleCount = await GetTopicCount("reactjs");

            // Pass the count to the view
            ViewBag.csharp_ArticleCount = csharparticleCount;
            ViewBag.mvc_ArticleCount = mvcArticleCount;
            ViewBag.webapi_ArticleCount = webapiArticleCount;
            ViewBag.java_ArticleCount = javaArticleCount;
            ViewBag.bootstrap_ArticleCount = bootstrapArticleCount;
            ViewBag.react_ArticleCount = reactArticleCount;

            // Fetch the Module Count
            int dotnet_Count = await GetModuleCount("dotnet-development");
            int frontend_Count = await GetModuleCount("front-end-development");
            int javascript_Count = await GetModuleCount("javascript-frameworks");
            int cloud_Count = await GetSubCategoryCount("cloud-development");

            int total_Count = (dotnet_Count + frontend_Count + javascript_Count + cloud_Count);

            // Pass the count to the view
            ViewBag.dotnet_Count = dotnet_Count;
            ViewBag.frontend_Count = frontend_Count;
            ViewBag.javascript_Count = javascript_Count;
            ViewBag.cloud_Count = cloud_Count;
            ViewBag.totalCourse_Count = total_Count;

            return View();
        }

        [HttpGet]
        public async Task<int> GetTopicCount(string name)
        {
            int techArticleCount = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"TechTopic/count?name={name}");

                if (res.IsSuccessStatusCode)
                {
                    string responseContent = await res.Content.ReadAsStringAsync();

                    if (!int.TryParse(responseContent, out techArticleCount))
                    {
                        throw new Exception("Failed to parse the count from the API response.");
                    }
                }
                else
                {
                    techArticleCount = 0;
                }

                return techArticleCount;
            }
        }

        [HttpGet]
        public async Task<int> GetModuleCount(string name)
        {
            int techModuleCount = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"TechModule?name={name}");

                if (res.IsSuccessStatusCode)
                {
                    string responseContent = await res.Content.ReadAsStringAsync();

                    if (!int.TryParse(responseContent, out techModuleCount))
                    {
                        throw new Exception("Failed to parse the count from the API response.");
                    }
                }
                else
                {
                    throw new Exception("Failed to fetch data from the API.");
                }

                return techModuleCount;
            }
        }

        [HttpGet]
        public async Task<int> GetSubCategoryCount(string name)
        {
            int techSubCategoryCount = 0;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync($"TechSubCategory?name={name}");

                if (res.IsSuccessStatusCode)
                {
                    string responseContent = await res.Content.ReadAsStringAsync();

                    if (!int.TryParse(responseContent, out techSubCategoryCount))
                    {
                        throw new Exception("Failed to parse the count from the API response.");
                    }
                }
                else
                {
                    throw new Exception("Failed to fetch data from the API.");
                }

                return techSubCategoryCount;
            }
        }
    }
}
