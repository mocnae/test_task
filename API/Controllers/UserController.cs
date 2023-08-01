using API.DTOs;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserDto createDto)
        {
            var user = new AppUser{ UserName = createDto.UserName, Email = createDto.Email };
            
            await _userManager.CreateAsync(user, createDto.Password);

            await _userManager.AddToRoleAsync(user, createDto.Role);

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserDto> UserList = new List<UserDto>();
            
            foreach (var user in users)
            {
                UserDto u = (UserDto)user;
                var roles = await _userManager.GetRolesAsync(user);
                string Role = roles.Any() ? roles[0] : "No Roles";
                u.Role = Role;

                UserList.Add(u);
            }

            return UserList;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserDetails(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(string id, AppUser user)
        {
            var _user = await _userManager.FindByIdAsync(id);
            
            user.Id = id;

            _mapper.Map(user, _user);

            await _userManager.UpdateAsync(_user);

            return Ok();

        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{id}")]
        public async Task<ActionResult> ChangePassword(string id, PasswordDto password)
        {
            var user = await _userManager.FindByIdAsync(id);

            await _userManager.RemovePasswordAsync(user);

            var res = await _userManager.AddPasswordAsync(user, password.Password);

            if (res.Succeeded)
            {
                return Ok();
            }
            
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("block/{id}")]
        public async Task<ActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user.IsBlocked) user.IsBlocked = false;
            else user.IsBlocked = true;

            await _userManager.UpdateAsync(user);

            return Ok();
        }
    }
}