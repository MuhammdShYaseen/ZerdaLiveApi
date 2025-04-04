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
    public class AppImagesLightsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public AppImagesLightsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/AppImagesLights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppImagesLight>>> GetAppImagesLights()
        {
            return await _context.AppImagesLights.ToListAsync();
        }

        // GET: api/AppImagesLights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppImagesLight>> GetAppImagesLight(int id)
        {
            var appImagesLight = await _context.AppImagesLights.FindAsync(id);

            if (appImagesLight == null)
            {
                return NotFound();
            }

            return appImagesLight;
        }

        // PUT: api/AppImagesLights/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutAppImagesLightint (int ID, string UserName, string Password, AppImagesLight appImagesLight)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != appImagesLight.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(appImagesLight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppImagesLightExists(ID))
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

        private bool AppImagesLightExists(int id)
        {
            return _context.AppImagesLights.Any(e => e.ImageId == id);
        }
    }
}
