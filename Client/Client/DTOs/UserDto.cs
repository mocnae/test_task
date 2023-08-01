namespace Client.DTOs
{
    public class UserDto
    {
        public bool IsBlocked { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
    }
}