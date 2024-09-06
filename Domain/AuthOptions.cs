using System.Text;
using Microsoft.IdentityModel.Tokens;

public class AuthOptions
{
    public const string ISSUER = "SumoTatamiBD"; // издатель токена
    public const string AUDIENCE = "SumoTatamiClient"; // потребитель токена
    const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));

    public static string Algoritm => SecurityAlgorithms.HmacSha256;
}