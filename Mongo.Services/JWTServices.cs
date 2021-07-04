using Mongo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Mongo.DTOs;

namespace Mongo.Services
{
    public class JWTServices : IJWTServices
    {
        public string GenerateToken(UserDisplayDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("passkeywordsdfgdsfgfdsg"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            var token = new JwtSecurityToken(
                      issuer: "Issuer",
                      claims: claims,
                      expires: DateTime.Now.AddMinutes(300),
                      signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
