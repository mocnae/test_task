using Domain;

namespace API.DTOs
{
    public class UserDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }

        public static explicit operator UserDto(AppUser v)
        {
            return new UserDto
            {
                UserName = v.UserName,
                Id = v.Id,
                IsBlocked = v.IsBlocked
            };
        }
    }
}