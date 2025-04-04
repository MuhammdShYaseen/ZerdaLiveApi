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
    public class AppImagesDarksController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public AppImagesDarksController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/AppImagesDarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppImagesDark>>> GetAppImagesDarks()
        {
            return await _context.AppImagesDarks.ToListAsync();
        }

        // GET: api/AppImagesDarks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppImagesDark>> GetAppImagesDark(int id)
        {
            var appImagesDark = await _context.AppImagesDarks.FindAsync(id);

            if (appImagesDark == null)
            {
                return NotFound();
            }

            return appImagesDark;
        }

        // PUT: api/AppImagesDarks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutAppImagesDark(int ID, string UserName, string Password, AppImagesDark appImagesDark)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != appImagesDark.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(appImagesDark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppImagesDarkExists(ID))
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

        private bool AppImagesDarkExists(int id)
        {
            return _context.AppImagesDarks.Any(e => e.ImageId == id);
        }
    }
}
