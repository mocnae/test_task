using System.Net.Http.Headers;
using System.Text;
using Client.DTOs;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _apiClient;

        public AccountController()
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri("http://localhost:5000/"); // Укажите URL вашего API
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = await _apiClient.PostAsync("api/account/login", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var userDto = JsonConvert.DeserializeObject<UserDto>(content);

                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddDays(7);

                        Response.Cookies.Append("JwtToken", userDto.Token, option);
                        Response.Cookies.Append("Role", userDto.Role, option);

                        return RedirectToAction("Index", "Product");
                    }
                    else if (((int)response.StatusCode) == 416)
                    {
                        return View("BannedView");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка при входе. Проверьте введенные данные.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при входе. Попробуйте позже.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new RegisterDto {UserName = model.UserName, Email = model.Email, Password = model.Password1};

                    HttpResponseMessage response = await _apiClient.PostAsync("api/account/register", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

                    if(response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var userDto = JsonConvert.DeserializeObject<UserDto>(content);

                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddDays(7);

                        Response.Cookies.Append("JwtToken", userDto.Token, option);
                        Response.Cookies.Append("Role", userDto.Role, option);

                        return RedirectToAction("Index", "Product");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ошибка при входе. Попробуйте позже.");
                }
            }

            return View(model);
        } 

        private void SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(7);

            Response.Cookies.Append(key, value, option);
        }
    }
}