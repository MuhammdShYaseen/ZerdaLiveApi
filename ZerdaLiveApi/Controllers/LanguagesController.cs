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
    public class LanguagesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public LanguagesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Languages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Language>>> GetLanguages()
        {
            return await _context.Languages.ToListAsync();
        }

        // GET: api/Languages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Language>> GetLanguage(int id)
        {
            var language = await _context.Languages.FindAsync(id);

            if (language == null)
            {
                return StatusCode(404);
            }

            return language;
        }

        // PUT: api/Languages/5
        [HttpPut("LangID={LangID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutLanguage(int LangID, string UserName, string Password, Language language)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole <=0)
            {
                return StatusCode(403);
            }
            if (LangID != language.LangId)
            {
                return StatusCode(400);
            }

            using (var Zcontext = _context)
            {
                var ss = Zcontext.Languages.First(a => a.LangId == language.LangId);
                ss.LangName = language.LangName;
                try
                {
                    await Zcontext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(LangID))
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

        // POST: api/Languages
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Language>> PostLanguage(string UserName, string Password, Language language)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole <=0)
            {
                return StatusCode(403);
            }
            using (var Zcontext = _context)
            {
                object SS = new Language { LangName = language.LangName };
                Zcontext.Add(SS);
                await Zcontext.SaveChangesAsync();
            }
            return Ok();
        }

        // DELETE: api/Languages/5
        [HttpDelete("LangID={LangID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteLanguage(int LangID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var language = await _context.Languages.FindAsync(LangID);
            if (language == null)
            {
                return StatusCode(404);
            }

            _context.Languages.Remove(language);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool LanguageExists(int id)
        {
            return _context.Languages.Any(e => e.LangId == id);
        }
    }
}
