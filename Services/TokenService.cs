using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
public class TokenService
{
    public TokenService()
    {

    }

    public string GenerateAccessToken(string login)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, login)
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(40)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), AuthOptions.Algoritm)
            );
        string accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        byte[] refreshTokenAsByteArray = new byte[32];
        var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(refreshTokenAsByteArray);
        string refreshToken = Convert.ToBase64String(refreshTokenAsByteArray);
        return refreshToken;
    }

    public string GetLoginByExpiredAccessToken(string expiredAccessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(expiredAccessToken, tokenValidationParameters, out SecurityToken expiredAccessTokenAsSecurityToken);
        var jwtSecurityToken = expiredAccessTokenAsSecurityToken as JwtSecurityToken;
        string login = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        if (login == null)
            throw new SecurityTokenException("Invalid token");

        return login;
    }
}