using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Persistencia.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration config;
        public LoginController(IConfiguration configuration){
            config = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] User usuario){
            IActionResult response = Unauthorized();
            if(Autenticar(usuario)){
                var tokenString = "xxxxx";
                var user = new User();
                user.Nombre = tokenString;
                user.Password = "";
                response =Ok( new {token = GenerarToken(user)} );
            }

            return response;
        }

        public bool Autenticar(User usuario){
            return true;
        }

        private string GenerarToken(User usuario){
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretThing!@#$%^&*()"));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var claims=new []
            {
                new Claim(JwtRegisteredClaimNames.Sub,usuario.Nombre),
            };
            var token=new JwtSecurityToken( 
                issuer: "http://localhost:5000",
                audience:"http://localhost:4200",
                claims:claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials:credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }

    }
}