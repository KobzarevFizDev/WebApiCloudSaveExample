using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics;
using System.Linq;

public class PlayersSaveRepository
{
    private IMongoCollection<PlayerSave> _players;
    public PlayersSaveRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _players = database.GetCollection<PlayerSave>(databaseSettings.Value.CollectionName);
    }

    public void Create(string login, string hashOfPassword)
    {
        if (ExistPlayerWithThisLogin(login))
            throw new InvalidOperationException("Player already exist!");

        PlayerSave newPlayer = new PlayerSave
        {
            Login = login,
            Nickname = "EnterYourNickname",
            Money = 100,
            Level = 1,
            PasswordHash = hashOfPassword,
            RefreshToken = "NullRefreshToken",
        };

        _players.InsertOne(newPlayer);
    }

    public void SetRefreshToken(string login,
                                string refreshToken,
                                DateTime refreshTokenExpirationTime)
    {
        if (ExistPlayerWithThisLogin(login) == false)
            throw new InvalidOperationException($"Player with login = {login} don't exist");

        var filter = Builders<PlayerSave>
                        .Filter.Eq(p => p.Login, login);
        var setRefreshToken = Builders<PlayerSave>
                        .Update.Set(p => p.RefreshToken, refreshToken);
        var setRefreshTokenExpirationTime = Builders<PlayerSave>
                        .Update.Set(p => p.RefreshTokenExpirationTime, refreshTokenExpirationTime);

        _players.UpdateOne(filter, setRefreshToken);
        _players.UpdateOne(filter, setRefreshTokenExpirationTime);
    }

    public void UpdateAmountOfMoney(string login, int newAmountOfMoney)
    {
        var filter = Builders<PlayerSave>.Filter.Eq(p => p.Login, login);
        var update = Builders<PlayerSave>.Update.Set(p => p.Money, newAmountOfMoney);
        _players.UpdateOne(filter, update);
    }

    public void UpdateNickname(string login, string newNickname)
    {
        var filter = Builders<PlayerSave>.Filter.Eq(p => p.Login, login);
        var update = Builders<PlayerSave>.Update.Set(p => p.Nickname, newNickname);
        _players.UpdateOne(filter, update);
    }

    public void UpdateLevelOfPlayer(string login, int newLevel)
    {
        var filter = Builders<PlayerSave>.Filter.Eq(p => p.Login, login);
        var update = Builders<PlayerSave>.Update.Set(p => p.Level, newLevel);
        _players.UpdateOne(filter, update);
    }

    public bool ExistPlayerWithThisLogin(string login)
    {
        var findedPlayer = _players
                .AsQueryable()
                .FirstOrDefault(p => p.Login == login);

        return findedPlayer == null ? false : true;
    }


    public PlayerSave GetPlayerSaveByLogin(string login)
    {
        if (ExistPlayerWithThisLogin(login) == false)
            throw new InvalidOperationException($"Player with login = {login} don't exist");

        var findedPlayer = _players
            .AsQueryable()
            .FirstOrDefault(p => p.Login == login);

        return findedPlayer;
    }
}