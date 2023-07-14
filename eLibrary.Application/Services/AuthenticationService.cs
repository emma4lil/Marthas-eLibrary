using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLibrary.Domain.Entities;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace eLibrary.Application.Services
{
    public class AuthenticationServices
    {
        private readonly FirebaseAuth
            _firebaseAuth;

        public AuthenticationServices(FirebaseApp firebaseApp)
        {
            _firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        }

        public async Task<UserCredentials> CreateUserAsync(UserCredentials credentials)
        {
            var result = await _firebaseAuth.VerifyIdTokenAsync(credentials.Token);
            if (result == null) return null;

            var newUser = new User
            {
                Email = result.Claims["email"].ToString()
            };
            return null;
        }


    }
}

public class UserCredentials
{
    public string Token { get; set; }
}