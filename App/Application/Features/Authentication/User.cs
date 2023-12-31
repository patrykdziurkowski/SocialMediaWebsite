﻿namespace Application.Features.Authentication
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
            Id = new UserId();
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public UserId Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
