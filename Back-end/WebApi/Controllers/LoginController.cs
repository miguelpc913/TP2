using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Persistencia.Models;
using System.Text.Json;
using WebApi.Services;
using Persistencia;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly GastoDbContext _context;
        private TokenServices tokenServices;

        public LoginController(IConfiguration configuration , GastoDbContext context){
            config = configuration;
            _context = context;
            tokenServices = new TokenServices(config);
        }

        [HttpGet("ValidateToken")]
        public bool isTokenValid()
        {
            var user = tokenServices.findUserByToken(Request , _context);
            if(user == null || tokenServices.tokenInvalid(user.Token)) return false;
           
            return true;
        }
 
 
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] User userCred)
        {
            
            User user = _context.Users.FirstOrDefault(user => user.Username.Equals(userCred.Username));

            if( user == null || !BCrypt.Net.BCrypt.Verify(userCred.Password , user.Password)) return Unauthorized();

            var token = tokenServices.GenerarToken(userCred);     
            user.Token = token;
            token = JsonSerializer.Serialize(token); 
            _context.SaveChanges();
            
            return Ok(token);
        }

    }
}