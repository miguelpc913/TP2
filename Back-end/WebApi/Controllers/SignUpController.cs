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

        // // GET: api/SignUp/5
        // [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
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

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
