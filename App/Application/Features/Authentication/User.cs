using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication
{
    public class User
    {
        public User(
            string userName,
            string email,
            string passwordHash)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public User(
            int id,
            string userName,
            string email,
            string passwordHash)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
        }

        public int? Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
