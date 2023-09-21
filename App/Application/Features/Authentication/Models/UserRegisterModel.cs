namespace Application.Features.Authentication.Models
{
    public class UserRegisterModel
    {
        public UserRegisterModel()
        {

        }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
