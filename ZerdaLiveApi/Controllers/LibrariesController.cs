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
    public class LibrariesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public LibrariesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Libraries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibrarys()
        {
            return await _context.Librarys.ToListAsync();
        }

        // GET: api/Libraries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetLibrary(int id)
        {
            var library = await _context.Librarys.FindAsync(id);

            if (library == null)
            {
                return StatusCode(404);
            }

            return library;
        }

        // PUT: api/Libraries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutLibrary(int ID,string UserName,string Password, Library library)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (ID != library.LibraryId)
            {
                return StatusCode(400);
            }

            _context.Entry(library).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibraryExists(ID))
                {
                    return StatusCode(404);
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(204);
        }

        // POST: api/Libraries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Library>> PostLibrary(string UserName, string Password, Library library)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            _context.Librarys.Add(library);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLibrary", new { id = library.LibraryId }, library);
        }

        // DELETE: api/Libraries/5
        [HttpDelete("ID={ID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteLibrary(int ID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var library = await _context.Librarys.FindAsync(ID);
            if (library == null)
            {
                return StatusCode(404);
            }

            _context.Librarys.Remove(library);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool LibraryExists(int id)
        {
            return _context.Librarys.Any(e => e.LibraryId == id);
        }
    }
}
