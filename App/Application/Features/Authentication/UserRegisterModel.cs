using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication
{
    public class UserRegisterModel
    {
        public UserRegisterModel(
            string userName,
            string email,
            string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
