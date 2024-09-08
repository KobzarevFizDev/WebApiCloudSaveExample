using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    public TokenController(IOptions<DatabaseSettings> databaseSettings)
    {

    }

    [HttpPost("UpdateAccessToken")]
    public IActionResult UpdateAccessToken([FromBody] UpdateAccessTokenRequest updateAccessTokenRequest)
    {
        return Ok();
    }
}