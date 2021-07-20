using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using Persistencia.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    [Route("gastos")]
    [ApiController]
    public class GastosController : ControllerBase
    {
        private readonly GastoDbContext _context;
        private GastoService gastoService;
        private TokenServices tokenServices;
        private CategoriaService categoriaService;
        public GastosController(IConfiguration config , GastoDbContext context)
        {
            _context = context;
            gastoService = new GastoService(context);
            categoriaService  = new CategoriaService(context);
            tokenServices  = new TokenServices(config);
        }

        // GET: gastos
        [HttpGet]
        [Authorize]
        public IActionResult GetGastos()
        {
            User user = tokenServices.findUserByToken(Request , _context);
            if (user is null) return Unauthorized();
            return Ok(user.Gastos);
        }

        //GET: gastos/categorias
        [Authorize]
        [HttpGet("categorias")]
        public IActionResult GetCategoria()
        {
            User user = tokenServices.findUserByToken(Request, _context);
            if (user is null) return Unauthorized();
            return Ok(user.Categorias);
        }

        [HttpGet("buscar")]
        [Authorize]
        public IActionResult GetGastoCategoria(string categoria, string description)
        {
            User user = tokenServices.findUserByToken(Request, _context);
            if (user is null) return Unauthorized();
            return  Ok(gastoService.SearchCategoryAndDescription(categoria, description , user).ToList());
        }


        // GET: gastos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gasto>> GetGasto(int id)
        {
            Gasto gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return NotFound();
            
            return gasto;
        }

        // PUT: gastos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGasto(int id, Gasto gasto)
        {
            User user = tokenServices.findUserByTokenNoTracking(Request, _context);
    
            if (user == null) Unauthorized();
            gasto.UserId = user.Id;
            gasto.categoria.UserId = user.Id;
            if (gastoService.isInvalidGasto(gasto)) return BadRequest();
            categoriaService.CategoriaCreateOrFind(gasto , user);
            _context.Entry(gasto).State = EntityState.Modified;
            try
            {
                Gasto OldGasto =  gastoService.SearchForOldGasto(gasto);
                await _context.SaveChangesAsync();
                User userUpdated = tokenServices.findUserByTokenNoTracking(Request, _context);
                if(categoriaService.removeCategoriaIfUnusedAfterUpdate(OldGasto , gasto , userUpdated)) await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!gastoService.GastoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: gastos
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Gasto>> PostGasto(Gasto gasto)
        {
            if(gastoService.isInvalidGasto(gasto)) return BadRequest();
            User user = tokenServices.findUserByToken(Request, _context);
            if (user == null) return Unauthorized();
            
            categoriaService.CategoriaCreateOrFind(gasto , user);

            _context.Gastos.Add(gasto);
            user.Gastos.Add(gasto);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetGasto", new { id = gasto.Id }, gasto);
        }

        // DELETE: gastos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGasto(int id)
        {
            Gasto gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)  return NotFound();
            User user = tokenServices.findUserByToken(Request, _context);
            if (user == null) return Unauthorized();
            categoriaService.deleteIfUnused(gasto , user);
            _context.Gastos.Remove(gasto);
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
