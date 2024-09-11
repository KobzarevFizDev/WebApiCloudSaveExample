using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private PlayersSaveRepository _saveRepository;
    private TokenService _tokenService;
    public TokenController(PlayersSaveRepository saveRepository,
                           TokenService tokenService)
    {
        _saveRepository = saveRepository;
        _tokenService = tokenService;
    }

    [HttpPost("UpdateAccessToken")]
    public IActionResult UpdateAccessToken([FromBody] UpdateAccessTokenRequest updateAccessTokenRequest)
    {
        string expiredAccessToken = updateAccessTokenRequest.ExpiredAccessToken;
        string refreshToken = updateAccessTokenRequest.RefreshToken;
        string login = _tokenService.GetLoginByExpiredAccessToken(expiredAccessToken);

        if (_saveRepository.ExistPlayerWithThisLogin(login) == false)
            return NotFound();
        else
        {
            if (_tokenService.CheckRefreshToken(login, refreshToken))
            {
                return Ok();
            }
            else
            {
                return StatusCode(401);
            }
        }
    }
}