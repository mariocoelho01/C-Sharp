using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Extensions;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Services;

public class TokenServices
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();//created Handler token snapshot
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);//converted JwtKet for bytes
        var claims = user.GetClaims();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8), 
         SigningCredentials = new SigningCredentials(
             new SymmetricSecurityKey(key), 
             SecurityAlgorithms.HmacSha256Signature)
        };//Contains all token information 
        var token = tokenHandler.CreateToken(tokenDescriptor);//Created token
        return tokenHandler.WriteToken(token);//this will return a string
    }
}