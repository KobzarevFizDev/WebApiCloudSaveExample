using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

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

    private string GetHashOfPassword(string password)
    {
        using (SHA256 sh256 = SHA256.Create())
        {
            byte[] passwordStringAsByteArray = Encoding.UTF8.GetBytes(password);
            byte[] passwordHash = sh256.ComputeHash(passwordStringAsByteArray);
            string passwordHashAsString = Convert.ToBase64String(passwordHash);
            return passwordHashAsString;
        }
    }


    [HttpPost("SignUp")]
    public ActionResult<AuthorizationResponse> SignUp([FromForm] AuthenticationRequest authenticationRequest)
    {
        string login = authenticationRequest.Login;
        string password = authenticationRequest.Password;
        string passwordHash = GetHashOfPassword(password);

        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            return StatusCode(409);
        }
        else
        {
            _playersSaveRepository.Create(login, passwordHash);

            string generatedAccessToken = _tokenService.GenerateAccessToken(authenticationRequest.Login);
            string generatedRefreshToken = _tokenService.GenerateRefreshToken(login);

            var responce = new AuthorizationResponse
            {
                AccessToken = generatedAccessToken,
                RefreshToken = generatedRefreshToken
            };

            return responce;
        }
    }

    // todo: обновить refresh токен и вернуть пару (акксесс и рефреш токены)
    [HttpPost("SignIn")]
    public ActionResult<AuthorizationResponse> SignIn([FromForm] AuthenticationRequest authenticationRequest)
    {
        string login = authenticationRequest.Login;
        string password = authenticationRequest.Password;

        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            PlayerSave save = _playersSaveRepository.GetPlayerSaveByLogin(login);
            string originPasswordHash = save.PasswordHash;
            string currentPasswordHash = GetHashOfPassword(password);

            if (originPasswordHash.Equals(currentPasswordHash))
            {
                string newAccessToken = _tokenService.GenerateAccessToken(authenticationRequest.Login);
                string newRefreshToken = _tokenService.GenerateRefreshToken(login);

                var responce = new AuthorizationResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };

                return responce;
            }
            else
            {
                return StatusCode(401);
            }
        }
        else
        {
            return NotFound();
        }
    }
}