using System.Net.Http.Headers;
using Client.DTOs;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryController(ILogger<ProductController> logger)
        {
            _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000/");

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/category/");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<CategoryDto> categories = JsonConvert.DeserializeObject<List<CategoryDto>>(content);

                ViewBag.Categories = categories;
                ViewBag.User = Request.Cookies["Role"];
            }
            else
            {
                return View("TryAgain");
            }
            return View();
        }

        [HttpGet("editcategory/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            string jwtToken = GetTokenAndAddToRequest();

            CategoryViewModel model = new CategoryViewModel();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/category/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                CategoryDto category = JsonConvert.DeserializeObject<CategoryDto>(content);

                model = (CategoryViewModel)category;
            }
            return View(model);
        }

        [HttpPost("editcategory/{id}")]
        public async Task<IActionResult> EditCategory(int id, CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                string jwtToken = GetTokenAndAddToRequest();

                CategoryDto category = (CategoryDto)model;

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/category/{id}", category);
            
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet("deletecategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/category/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(); 
        }

        [HttpGet("addcategory")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost("addcategory")]
        public async Task<IActionResult> AddCategory(CategoryViewModel model)
        {
            CategoryDto category = new CategoryDto();

            if (ModelState.IsValid)
            {
                category = (CategoryDto)model;

                string jwtToken = GetTokenAndAddToRequest();

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/api/category/", category);
            
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        public string GetTokenAndAddToRequest()
        {
            string jwtToken = Request.Cookies["JwtToken"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            return jwtToken;
        }

        public async Task<Dictionary<int, string>> DictCategories()
        {
            Dictionary<int, string> Dict = new Dictionary<int, string>();

            HttpResponseMessage response = await _httpClient.GetAsync("api/category/");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<CategoryDto> categories = JsonConvert.DeserializeObject<List<CategoryDto>>(content);

                foreach (var category in categories){
                    Dict.Add(category.Id, category.Description);
                }
            }
            else
            {
                
            }

            return Dict;
        }
    }
}