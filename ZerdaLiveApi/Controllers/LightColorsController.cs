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
    public class LightColorsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public LightColorsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/LightColors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LightColor>>> GetLightColors()
        {
            return await _context.LightColors.ToListAsync();
        }

        // GET: api/LightColors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LightColor>> GetLightColor(int id)
        {
            var lightColor = await _context.LightColors.FindAsync(id);

            if (lightColor == null)
            {
                return NotFound();
            }

            return lightColor;
        }

        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutLightColor(int ID, string UserName, string Password, LightColor lightColor)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != lightColor.ControllId)
            {
                return BadRequest();
            }

            _context.Entry(lightColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LightColorExists(ID))
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

        private bool LightColorExists(int id)
        {
            return _context.LightColors.Any(e => e.ControllId == id);
        }
    }
}
