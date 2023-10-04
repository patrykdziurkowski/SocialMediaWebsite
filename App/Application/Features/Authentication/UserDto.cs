namespace Application.Features.Authentication
{
    public class UserDto
    {
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
