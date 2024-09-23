public class PlayerGameData
{
    public string Login { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public int Money { get; set; }
    public int Level { get; set; }
    public PlayerSkin Skin { get; set; } = null!;
}