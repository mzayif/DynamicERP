using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DynamicERP.Core.Interfaces.Services;
using DynamicERP.Core.Models;
using DynamicERP.Core.ResponseModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DynamicERP.Application.Services;

/// <summary>
/// JWT işlemleri için service implementasyonu
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Kullanıcı bilgilerine göre JWT token oluşturur
    /// </summary>
    /// <param name="user">Kullanıcı bilgileri</param>
    /// <returns>JWT token</returns>
    public string GenerateToken(UserResponseModel user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("UserId", user.Id.ToString()),
            new("Username", user.Username),
            new("Email", user.Email)
        };

        // Kullanıcı rollerini ekle (gelecekte role sistemi eklendiğinde)
        // claims.Add(new Claim(ClaimTypes.Role, user.Role));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Refresh token oluşturur
    /// </summary>
    /// <param name="userId">Kullanıcı ID</param>
    /// <returns>Refresh token</returns>
    public string GenerateRefreshToken(Guid userId)
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = Convert.ToBase64String(randomNumber);
        
        // Gerçek uygulamada refresh token'ı veritabanında saklanmalı
        // Burada şimdilik sadece token oluşturuyoruz
        
        return refreshToken;
    }

    /// <summary>
    /// JWT token'ı doğrular
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Token geçerli mi?</returns>
    public bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// JWT token'dan kullanıcı ID'sini çıkarır
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Kullanıcı ID</returns>
    public Guid? GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return null;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// JWT token'dan kullanıcı email'ini çıkarır
    /// </summary>
    /// <param name="token">JWT token</param>
    /// <returns>Kullanıcı email</returns>
    public string? GetUserEmailFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            return emailClaim?.Value;
        }
        catch
        {
            return null;
        }
    }
} 