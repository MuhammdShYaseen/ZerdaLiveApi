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
    public class FilmSnCategoriesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public FilmSnCategoriesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/FilmSnCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmSnCategory>>> GetFilmSnCategories()
        {
            return await _context.FilmSnCategories.ToListAsync();
        }

        // GET: api/FilmSnCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmSnCategory>> GetFilmSnCategory(int id)
        {
            var filmSnCategory = await _context.FilmSnCategories.FindAsync(id);

            if (filmSnCategory == null)
            {
                return StatusCode(404);
            }

            return filmSnCategory;
        }

        // PUT: api/FilmSnCategories/5
        [HttpPut("CatID={CatID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutFilmSnCategory(int CatID, string UserName, string Password, FilmSnCategory filmSnCategory)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                if (CatID != filmSnCategory.CatId)
                {
                    return StatusCode(400);
                }

                using (var Zcontext = _context)
                {
                    var ss = Zcontext.FilmSnCategories.First(a => a.CatId == filmSnCategory.CatId);
                    ss.CatName = filmSnCategory.CatName;
                    try
                    {
                        await Zcontext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FilmSnCategoryExists(CatID))
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

        // POST: api/FilmSnCategories
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<FilmSnCategory>> PostFilmSnCategory(string UserName, string Password, FilmSnCategory filmSnCategory)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                using (var Zcontext = _context)
                {
                    object SS = new FilmSnCategory { CatName = filmSnCategory.CatName };
                    Zcontext.Add(SS);
                    await Zcontext.SaveChangesAsync();
                }
                return Ok();
               
            }
            return StatusCode(403);
        }

        // DELETE: api/FilmSnCategories/5
        [HttpDelete("CatID={CatID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteFilmSnCategory(int CatID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var filmSnCategory = await _context.FilmSnCategories.FindAsync(CatID);
            if (filmSnCategory == null)
            {
                return StatusCode(404);
            }

            _context.FilmSnCategories.Remove(filmSnCategory);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool FilmSnCategoryExists(int id)
        {
            return _context.FilmSnCategories.Any(e => e.CatId == id);
        }
    }
}
