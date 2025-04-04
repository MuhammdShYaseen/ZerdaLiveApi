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
    public class CountriesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public CountriesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountrys()
        {
            return await _context.Countrys.ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await _context.Countrys.FindAsync(id);

            if (country == null)
            {
                return StatusCode(404);
            }

            return country;
        }

        // PUT: api/Countries/5
        [HttpPut("ConID={ConID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutCountry(int ConID, string UserName, string Password, Country country)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2 || UserRole == 3 || UserRole == 4)
            {
                if (ConID != country.CountryId)
                {
                    return StatusCode(400);
                }
                using (var HR = _context)
                {
                    var ss = HR.Countrys.First(a => a.CountryId == country.CountryId);
                    ss.CountryName = country.CountryName;
                    ss.CountryFlag = country.CountryFlag;
                    try
                    {
                        await HR.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CountryExists(ConID))
                        {
                            return StatusCode(404);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Ok();
            }
            return StatusCode(403);

        }

        // POST: api/Countries
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Country>> PostCountry(string UserName, string Password, Country country)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2 || UserRole == 3 || UserRole == 4)
            {

                using (var HR = _context)
                {
                    object SS = new Country { CountryName = country.CountryName, CountryFlag = country.CountryFlag };
                    HR.Add(SS);
                    await HR.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Countries/5
        [HttpDelete("ConID={ConID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteCountry(int ConID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var country = await _context.Countrys.FindAsync(ConID);
            if (country == null)
            {
                return StatusCode(404);
            }

            _context.Countrys.Remove(country);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool CountryExists(int id)
        {
            return _context.Countrys.Any(e => e.CountryId == id);
        }
    }
}
