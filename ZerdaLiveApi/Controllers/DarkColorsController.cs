using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DarkColorsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public DarkColorsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/DarkColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DarkColor>>> GetDarkColors()
        {
            return await _context.DarkColors.ToListAsync();
        }

        // GET: api/DarkColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DarkColor>> GetDarkColor(int id)
        {
            var darkColor = await _context.DarkColors.FindAsync(id);

            if (darkColor == null)
            {
                return NotFound();
            }

            return darkColor;
        }

        // PUT: api/DarkColors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutDarkColor(int ID, string UserName, string Password, DarkColor darkColor)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if(UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != darkColor.ControllId)
            {
                return BadRequest();
            }

            _context.Entry(darkColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DarkColorExists(ID))
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

        private bool DarkColorExists(int id)
        {
            return _context.DarkColors.Any(e => e.ControllId == id);
        }
    }
}
