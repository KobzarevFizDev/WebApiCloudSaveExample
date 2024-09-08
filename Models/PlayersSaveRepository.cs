using Microsoft.Extensions.Options;
using MongoDB.Driver;
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

    // todo: Необходимо хешировать пароли и класть в базу данных
    public void Create(string login, string password)
    {
        if (ExistPlayerWithThisLogin(login) == false)
            throw new InvalidOperationException("Player already exist!");

        PlayerSave newPlayer = new PlayerSave
        {
            Login = login,
            Nickname = "Default",
            Money = 100,
            Level = 1
        };

        _players.InsertOne(newPlayer);
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
        if (ExistPlayerWithThisLogin(login))
            throw new InvalidOperationException($"Player with login = {login} don't exist");

        var findedPlayer = _players
            .AsQueryable()
            .FirstOrDefault(p => p.Login == login);

        return findedPlayer;
    }
}