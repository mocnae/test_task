using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Net.Http.Headers;
using Client.DTOs;
using System.Text;

namespace Client.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly HttpClient _httpClient;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5000/");

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IActionResult> Index()
    {
        bool res = Request.Cookies.ContainsKey("JwtToken");

        string jwtToken = GetTokenAndAddToRequest();

        HttpResponseMessage response = await _httpClient.GetAsync("/api/product/");

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();

            List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(content);

            var categoryDictionary = await DictCategories();

            foreach (var product in products)
            {
                product.CategoryName = categoryDictionary[product.CategoryId];
            }

            List<CategoryDto> list = new List<CategoryDto>();

            foreach (var item in categoryDictionary)
            {
                CategoryDto d = new CategoryDto {Id = item.Key, Description = item.Value };
                list.Add(d);
            }

            ViewBag.Products = products;
            ViewBag.Categories = (FilterByCategoryViewModel)list;
            ViewBag.User = Request.Cookies["Role"];
        }
        else
        {
            // Обработка случая, когда получен ответ с ошибкой (например, 404 Not Found)
            // ...
        }
        return View();
    }

    // [HttpGet]

    [HttpGet("editproduct/{id}")]
    public async Task<IActionResult> EditProduct(int id)
    {
        string jwtToken = GetTokenAndAddToRequest();

        HttpResponseMessage response = await _httpClient.GetAsync($"/api/product/{id}");

        string content = await response.Content.ReadAsStringAsync();

        ProductDto product = JsonConvert.DeserializeObject<ProductDto>(content);

        ProductViewModel model = (ProductViewModel)product;

        ViewBag.User = Request.Cookies["Role"];
        ViewBag.Product = product;

        return View(model);
    }

    [HttpPost("editproduct/{id}")]
    public async Task<IActionResult> EditProduct(int id, ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            ProductDto product = (ProductDto)model;
            
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"/api/product/{id}", product);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Ошибка при изменении продукта, попробуйте заполнить поля верно.");
            }
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Ошибка при изменении продукта, попробуйте заполнить поля верно.");
        }

        return View(model);
    }

    [HttpGet("add")]
    public async Task<IActionResult> AddProduct()
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
            ModelState.AddModelError("", "Ошибка при изменении продукта, попробуйте заполнить поля верно.");
        }

        return View();
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProduct(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            ProductDto product = (ProductDto)model;

            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.PostAsync("api/product/", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
        
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                
            }
        }

        return View(model);
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        string jwtToken = GetTokenAndAddToRequest();

        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/product/{id}");
    
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return View();
    }

    // [HttpGet("filter")]
    // public async Task<IActionResult> FilterByCategory()
    // {
    //     string jwtToken = GetTokenAndAddToRequest();

    //     HttpResponseMessage response = await _httpClient.GetAsync("/api/category/");

    //     if (response.IsSuccessStatusCode)
    //     {
    //         string content = await response.Content.ReadAsStringAsync();

    //         List<CategoryDto> categories = JsonConvert.DeserializeObject<List<CategoryDto>>(content);
        
    //         ViewBag.User = Request.Cookies["Role"];

    //         FilterByCategoryViewModel model = (FilterByCategoryViewModel)categories;
            
    //         return PartialView("FilterByCategory", model);
    //     }
    //     else
    //     {
    //         ModelState.AddModelError("", "Ошибка при изменении продукта, попробуйте заполнить поля верно.");
    //     }

    //     return View();
    // }

    [HttpPost("categoryFilter")]
    public async Task<IActionResult> FilterByCategory(FilterByCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync($"api/product/prods/{model.CategoryId}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(content);

                var categoryDictionary = await DictCategories();

                foreach (var product in products)
                {
                    product.CategoryName = categoryDictionary[product.CategoryId];
                }

                List<CategoryDto> list = new List<CategoryDto>();

                foreach (var item in categoryDictionary)
                {
                    CategoryDto d = new CategoryDto {Id = item.Key, Description = item.Value };
                    list.Add(d);
                }

                ViewBag.Products = products;
                ViewBag.Categories = (FilterByCategoryViewModel)list;
                ViewBag.User = Request.Cookies["Role"];
            }
        }
        return View("Index");
    }

    [HttpPost("FindProducts")]
    public async Task<IActionResult> FindProducts(FindViewModel model)
    {
        if (ModelState.IsValid)
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync($"api/product/find/{model.Str}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<ProductDto> products = JsonConvert.DeserializeObject<List<ProductDto>>(content);

                var categoryDictionary = await DictCategories();

                foreach (var product in products)
                {
                    product.CategoryName = categoryDictionary[product.CategoryId];
                }

                List<CategoryDto> list = new List<CategoryDto>();

                foreach (var item in categoryDictionary)
                {
                    CategoryDto d = new CategoryDto {Id = item.Key, Description = item.Value };
                    list.Add(d);
                }

                ViewBag.Products = products;
                ViewBag.Categories = (FilterByCategoryViewModel)list;
                ViewBag.User = Request.Cookies["Role"];
            }
        }

        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
