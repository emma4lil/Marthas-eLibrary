using eLibrary.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLibrary.Controllers;

[Route("api/iam")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationServices _authenticationServices;

    public AuthenticationController(IAuthenticationServices authenticationServices)
    {
        _authenticationServices = authenticationServices;
    }

    /// <summary>
    /// Callback to create default profile for new user
    /// </summary>
    /// <returns></returns>
    [HttpGet("auth")]
    [Authorize]
    public async Task<IActionResult> Authorize()
    {
        var authHeader = HttpContext.Request.Headers["Authorization"]
            .ToString();
        if (!authHeader.StartsWith("Bearer")) return BadRequest("Not Authenticated");
        var token = authHeader.Substring(7);

        var cred = new UserCredentials();
        cred.Token = token;
        var cp = User;
        var result = await _authenticationServices.CreateUserAsync(cred);
        return Ok(result.message);
    }
}