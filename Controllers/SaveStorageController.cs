using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SaveStorageController : ControllerBase
{
    public SaveStorageController()
    {

    }

    [Authorize]
    [HttpGet("getSave")]
    public IActionResult GetSaveOfPlayer()
    {
        var testPlayerSave = new PlayerSave
        {
            Nickname = "DefaultNickname",
            Money = 300,
            Level = 4
        };
        return Ok(testPlayerSave);
    }
}