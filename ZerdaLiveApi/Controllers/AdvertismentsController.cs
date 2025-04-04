using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Attributes;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public AdvertismentsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Advertisments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Advertisment>>> GetAdvertisments()
        {
            return await _context.Advertisments.ToListAsync();
        }

        // GET: api/Advertisments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Advertisment>> GetAdvertisment(int id)
        {
            var advertisment = await _context.Advertisments.FindAsync(id);

            if (advertisment == null)
            {
                return NotFound();
            }

            return advertisment;
        }

        // PUT: api/Advertisments/5
        [HttpPut("AdID={AdID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutAdvertisment(int AdID, string UserName, string Password, Advertisment advertisment)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (AdID != advertisment.AdsId)
            {
                return StatusCode(400);
            }

            _context.Entry(advertisment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertismentExists(AdID))
                {
                    return StatusCode(404);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Advertisments
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Advertisment>> PostAdvertisment(string UserName, string Password, Advertisment advertisment)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            _context.Advertisments.Add(advertisment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvertisment", new { id = advertisment.AdsId }, advertisment);
        }

        // DELETE: api/Advertisments/5
        [HttpDelete("AdID={AdID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteAdvertisment(int AdID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var advertisment = await _context.Advertisments.FindAsync(AdID);
            if (advertisment == null)
            {
                return StatusCode(400);
            }

            _context.Advertisments.Remove(advertisment);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool AdvertismentExists(int id)
        {
            return _context.Advertisments.Any(e => e.AdsId == id);
        }
    }
}
