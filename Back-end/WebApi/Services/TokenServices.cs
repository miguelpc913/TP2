using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Persistencia.Models;
using System.Linq;
using Persistencia;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Services 
{
    public class TokenServices
    {
        private readonly IConfiguration config;
        public TokenServices(IConfiguration configuration){
            config = configuration;
        }
        public bool tokenInvalid(string token)
        {
            if (string.IsNullOrEmpty(token)) return true;

            var jwtToken = new JwtSecurityToken(token);
            return (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);

        }
        public string GenerarToken(User usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,usuario.Username),
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string formatAuth(HttpRequest request)
        {
            var auth = request.Headers.First(keyValue => keyValue.Key.Equals("Authorization")).Value.ToString();
            if (auth.Contains("Bearer "))
            {
                string token = auth.Replace("Bearer ", "");
                if (!tokenInvalid(token)) return token;
            }
            return "invalid";
        }
        public User findUserByToken(HttpRequest request , GastoDbContext context)
        {
            string token = formatAuth(request);
            if(!token.Equals("invalid")){
               User user = context.Users.FirstOrDefault(user => user.Token.Equals(token));
               return user;
            }
            return null;
        }

        public User findUserByTokenNoTracking(HttpRequest request, GastoDbContext context)
        {
            string token = formatAuth(request);
            if (!token.Equals("invalid"))
            {
                User user = context.Users.AsNoTracking().Where(user => user.Token.Equals(token))
                .Include(user => user.Gastos ).ThenInclude(gasto => gasto.categoria).AsNoTracking()
                .Include(user => user.Categorias ).AsNoTracking()
                .First();
                return user;
            }
            return null;
        }

        public int findUserIdByToken(HttpRequest request, GastoDbContext context)
        {
            string token = formatAuth(request);
            if (!token.Equals("invalid"))
            {
                User user = context.Users.FirstOrDefault(user => user.Token.Equals(token));
                return user.Id;
            }
            return -1;
        }
    }
}