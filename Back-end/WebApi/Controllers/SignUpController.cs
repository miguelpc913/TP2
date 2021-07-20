using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Models;

namespace WebApi.Controllers
{
    [Route("sign-up")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly GastoDbContext _context;

        public SignUpController(GastoDbContext context)
        {
            _context = context;
        }

        // GET: api/SignUp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("username-used")]
        public bool usernameIsUsed(User userCred)
        {
            return _context.Users.Any( user => user.Username.Equals(userCred.Username));
        }

        // POST: api/SignUp
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
