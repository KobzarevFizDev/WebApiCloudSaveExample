using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    public TokenController()
    {

    }

    [HttpPost("UpdateAccessToken")]
    public IActionResult UpdateAccessToken([FromBody] UpdateAccessTokenRequest updateAccessTokenRequest)
    {
        return Ok();
    }
}