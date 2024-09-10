using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class PlayerSave
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Login { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public int Money { get; set; }
    public int Level { get; set; }
    public string PasswordHash { get; set; } = null!;
}