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
    public class FilmsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public FilmsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Films
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return StatusCode(404);
            }

            return film;
        }

        // PUT: api/Films/5
        [HttpPut("FID={FID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutFilm(int FID, string UserName, string Password, Film film)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                if (FID != film.FilmId)
                {
                    return StatusCode(400);
                }
                using (var ZContext = _context)
                {
                    var ss = ZContext.Films.First(a => a.FilmId == film.FilmId);
                    ss.FilmeName = film.FilmeName;
                    ss.FilmCat = film.FilmCat;
                    ss.FilmCountry = film.FilmCountry;
                    ss.FilmUrl = film.FilmUrl;
                    ss.FilmLang = film.FilmLang;
                    ss.FilmDuration = film.FilmDuration;
                    ss.FilmImage = film.FilmImage;
                    ss.FilmDis = film.FilmDis;
                    ss.IsNew = film.IsNew;
                    ss.IsTop = film.IsTop;
                    try
                    {
                        await ZContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FilmExists(FID))
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

        // POST: api/Films
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Film>> PostFilm(string UserName, string Password, Film film)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 4)
            {
                using (var ZContext = _context)
                {
                    object SS = new Film { FilmeName = film.FilmeName, FilmCat = film.FilmCat, FilmCountry = film.FilmCountry, FilmUrl = film.FilmUrl, FilmDis = film.FilmDis, FilmDuration = film.FilmDuration, FilmImage = film.FilmImage, FilmLang = film.FilmLang, IsTop = film.IsTop, IsNew = film.IsNew };
                    ZContext.Add(SS);
                    await ZContext.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Films/5
        [HttpDelete("FID={FID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteFilm(int FID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if(UserRole == 1 || UserRole == 4)
            {
                var film = await _context.Films.FindAsync(FID);
                if (film == null)
                {
                    return StatusCode(404);
                }
                _context.Films.Remove(film);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
            
        }

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.FilmId == id);
        }
    }
}
