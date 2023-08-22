using Application.Features.Authentication.Interfaces;
using Application.Features.Authentication.Models;
using FluentResults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication
{
    public class SignInManager : ISignInManager
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IUserRepository _userRepository;
        private readonly ISecretHasher _secretHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInManager(
            IConnectionFactory connectionFactory,
            IUserRepository userRepository,
            ISecretHasher secretHasher,
            IHttpContextAccessor httpContextAccessor)
        {
            _connectionFactory = connectionFactory;
            _userRepository = userRepository;
            _secretHasher = secretHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Attempts to log in the user with given login credentials
        /// </summary>
        /// <param name="inputUser">User's login information</param>
        /// <returns></returns>
        public async Task<Result> SignIn(UserLoginModel inputUser)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            Result<User> result = await _userRepository.GetUserByUserNameAsync(inputUser.UserName!);
            if (result.IsFailed)
            {
                return Result.Fail(result.Errors.First().Message);
            }
            User foundUser = result.Value;

            bool passwordIsNotMatching = !_secretHasher.Verify(inputUser.Password!, foundUser.PasswordHash);
            if (passwordIsNotMatching)
            {
                return Result.Fail("No user with such password and username combination was found.");
            }

            ClaimsIdentity identity = new(GetUserClaims(foundUser), /*Explicit*/CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new(identity);

            HttpContext context = _httpContextAccessor.HttpContext!;
            await context.SignInAsync(principal);

            return Result.Ok();

        }

        /// <summary>
        /// Signs the user out from a given HttpContext
        /// </summary>
        public async Task SignOut()
        {
            HttpContext context = _httpContextAccessor.HttpContext!;
            await context.SignOutAsync();
        }


        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)

            };

            return claims;
        }

    }
}
