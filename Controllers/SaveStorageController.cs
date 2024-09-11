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

    [Authorize]
    [HttpGet("GetSaveOfPlayer/{login}")]
    public ActionResult<PlayerGameData> GetSaveOfPlayer(string login)
    {
        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            PlayerSave playerSave = _playersSaveRepository.GetPlayerSaveByLogin(login);
            return new PlayerGameData
            {
                Login = playerSave.Login,
                Nickname = playerSave.Nickname,
                Money = playerSave.Money,
                Level = playerSave.Money
            };
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("UpdateNickname/{login}/{newNickname}")]
    public IActionResult UpdateNickname(string login, string newNickname)
    {
        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            _playersSaveRepository.UpdateNickname(login, newNickname);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("UpdateAmountOfMoney/{login}/{amountOfMoney}")]
    public IActionResult UpdateAmountOfMoney(string login, int amountOfMoney)
    {
        if (_playersSaveRepository.ExistPlayerWithThisLogin(login))
        {
            _playersSaveRepository.UpdateAmountOfMoney(login, amountOfMoney);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}