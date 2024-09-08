using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private PlayersSaveRepository _playersSaveRepository;
    private TokenService _tokenService;
    public AccountController(TokenService tokenService, PlayersSaveRepository playersSaveService)
    {
        _tokenService = tokenService;
        _playersSaveRepository = playersSaveService;
    }

    [HttpPost("SignUp")]
    public IActionResult SignUp([FromForm] AuthenticationRequest loginModel)
    {
        string generatedAccessToken = _tokenService.GenerateAccessToken(loginModel.Login);
        string generatedRefreshToken = _tokenService.GenerateRefreshToken();

        _playersSaveRepository.Create(loginModel.Login, loginModel.Password);

        return Ok(new AuthenticationResponse
        {
            AccessToken = generatedAccessToken,
            RefreshToken = generatedRefreshToken
        });
    }
}