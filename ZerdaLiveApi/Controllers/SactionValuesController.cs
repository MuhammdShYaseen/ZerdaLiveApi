using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SactionValuesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        public IConfiguration Configuration { get; }
        public SactionValuesController(ZerdaLiveContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            Configuration = (JsonConfigurationExtensions.AddJsonFile(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()), "appsettings.json").Build());
        }

        // GET: api/SactionValues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SactionValue>>> GetSactionValues()
        {
            return await _context.SactionValues.ToListAsync();
        }

        // GET: api/SactionValues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SactionValue>> GetSactionValue(int id)
        {
            var sactionValue = await _context.SactionValues.FindAsync(id);

            if (sactionValue == null)
            {
                return NotFound();
            }

            return sactionValue;
        }

        // PUT: api/SactionValues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&Code={Code}")]
        public async Task<IActionResult> PutSactionValue(int ID, string Code, SactionValue sactionValue)
        {
            string DevCode = Configuration["StaticDeveloperCode:Codee"];
            if(Code != DevCode)
            {
                return StatusCode(403);
            }
            if (ID != sactionValue.SactionId)
            {
                return BadRequest();
            }

            _context.Entry(sactionValue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SactionValueExists(ID))
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

       
       
        private bool SactionValueExists(int id)
        {
            return _context.SactionValues.Any(e => e.SactionId == id);
        }
    }
}
