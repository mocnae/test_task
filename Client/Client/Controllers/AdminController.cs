using System.Net.Http.Headers;
using Client.DTOs;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly HttpClient _httpClient;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5000/");

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/user/");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<UserDto> users = JsonConvert.DeserializeObject<List<UserDto>>(content);

                ViewBag.Users = users;
                ViewBag.User = Request.Cookies["Role"];
            }
            else
            {
                
            }
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string jwtToken = GetTokenAndAddToRequest();

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/api/user/{id}", new PasswordDto { Password = model.Password });

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                
            }

            return View(model);
        }

        [HttpGet("block/{id}")]
        public async Task<IActionResult> BlockUser(string id)
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/user/block/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpGet("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/user/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpGet("addUser")]
        public async Task<IActionResult> AddUser()
        {
            string jwtToken = GetTokenAndAddToRequest();

            HttpResponseMessage response = await _httpClient.GetAsync("/api/role/");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                List<RoleDto> roles = JsonConvert.DeserializeObject<List<RoleDto>>(content);

                ViewBag.Roles = roles;
            }

            return View();
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string jwtToken = GetTokenAndAddToRequest();

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/user/", model);

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

        public string GetTokenAndAddToRequest()
        {
            string jwtToken = Request.Cookies["JwtToken"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            return jwtToken;
        }
    }
}