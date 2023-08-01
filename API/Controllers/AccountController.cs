using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _service;

        public AccountController(UserManager<AppUser> userManager, TokenService service)
        {
            _userManager = userManager;
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto) 
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            
            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            var roles = await _userManager.GetRolesAsync(user);

            var role = roles.Any() ? roles[0] : "No Roles";
            
            if(result)
            {
                
                if (user.IsBlocked) return StatusCode(416);
                
                return new UserDto
                {
                    Token = _service.CreateToken(user, role),
                    Id = user.Id,
                    Role = role,
                    UserName = user.UserName,
                    IsBlocked = user.IsBlocked
                };
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            const string DefaultRole = "user";

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("Username is already taken");
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                IsBlocked = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            await _userManager.AddToRoleAsync(user, DefaultRole);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    IsBlocked = user.IsBlocked,
                    Token = _service.CreateToken(user, DefaultRole),
                    UserName = user.UserName,
                    Role = DefaultRole,
                    Id = user.Id
                };
            }

            return BadRequest("Problem registering user");
        }
    }
}