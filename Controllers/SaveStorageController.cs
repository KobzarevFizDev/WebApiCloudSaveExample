using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class SaveStorageController : ControllerBase
{
    private PlayersSaveRepository _playersSaveRepository;
    public SaveStorageController(PlayersSaveRepository playersSaveRepository)
    {
        _playersSaveRepository = playersSaveRepository;
    }

    [HttpGet("GetDefaultSave")]
    public IActionResult GetDefaultSaveOfPlayer()
    {
        var testPlayerSave = new PlayerSave
        {
            Nickname = "DefaultNickname",
            Money = 300,
            Level = 4
        };
        return Ok(testPlayerSave);
    }

    [HttpGet("GetSaveOfPlayer/{login}")]
    public ActionResult<PlayerSave> GetSaveOfPlayer(string login)
    {
        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            PlayerSave playerSave = _playersSaveRepository.GetPlayerSaveByLogin(login);
            return playerSave;
        }
        else
        {
            return NotFound();
        }
    }
}