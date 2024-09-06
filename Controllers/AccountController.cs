using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{

    private TokenService _tokenService;
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("SignUp")]
    public IActionResult SignUp([FromForm] AuthenticationRequest loginModel)
    {
        string generatedAccessToken = _tokenService.GenerateAccessToken(loginModel.Login);
        string generatedRefreshToken = _tokenService.GenerateRefreshToken();
        return Ok(new AuthenticationResponse
        {
            AccessToken = generatedAccessToken,
            RefreshToken = generatedRefreshToken
        });
    }
}