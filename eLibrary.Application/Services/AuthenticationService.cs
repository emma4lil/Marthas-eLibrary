using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using eLibrary.Domain.Entities;
using Auth0.ManagementApi;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Azure;
using eLibrary.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace eLibrary.Application.Services
{
    public interface IAuthenticationServices
    {
        Task<(bool success, string message)> CreateUserAsync(UserCredentials credentials);
    }

    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly LibraryDbContext _context;
        private readonly AuthenticationApiClient _apiClient;

        public AuthenticationServices(IConfiguration configuration, LibraryDbContext context)
        {
            _context = context;
            _apiClient = new AuthenticationApiClient(new Uri($"https://{configuration["Auth0:Domain"]}"));
        }

        /// <summary>
        /// Creates a default profile for new users
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<(bool success, string message)> CreateUserAsync(UserCredentials credentials)
        {
            var userinfo = await _apiClient.GetUserInfoAsync(credentials.Token);
            if (userinfo == null) return (false, "Please Sign up again");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userinfo.Email);

            if (user != null) return (false, "Profile exists");

            var newUser = new User
            {
                Email = userinfo.Email,
                FirstName = userinfo.FirstName,
                Lastname = userinfo.LastName
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return (true, "Profile created");
        }
    }
}

public class UserCredentials
{
    public string Token { get; set; }
}