using Newtonsoft.Json;

namespace Application.Features.Authentication
{
    public class UserDto
    {
        [JsonConstructor]
        public UserDto(
            string id,
            string userName,
            string email)
        {
            Id = Guid.Parse(id);
            UserName = userName;
            Email = email;
        }

        public UserDto(User user)
        {
            Id = user.Id.Value;
            UserName = user.UserName;
            Email = user.Email;
        }

        public Guid Id { get; }
        public string UserName { get; }
        public string Email { get; }
    }
}
