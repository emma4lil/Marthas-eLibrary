using eLibrary.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly AuthenticationServices _authenticationServices;
        public Authentication(AuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        /// <summary>
        /// Testing
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserCredentials> Authorize()
        {
            var cred = new UserCredentials();
            var result = await _authenticationServices.CreateUserAsync(cred);
            return result;
        }
    }
}
