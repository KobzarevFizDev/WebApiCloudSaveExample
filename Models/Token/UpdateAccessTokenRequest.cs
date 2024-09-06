public class UpdateAccessTokenRequest
{
    public string ExpiredAccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}