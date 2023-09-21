namespace Application.Features.Authentication
{
    public class User
    {
        private User()
        {
            Id = default!;
            UserName = default!;
            Email = default!;
            PasswordHash = default!;
        }
        public User(
            string userName,
            string email,
            string passwordHash)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
