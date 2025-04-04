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
    public class CategoriesController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public CategoriesController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategorys()
        {
            return await _context.Categorys.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categorys.FindAsync(id);

            if (category == null)
            {
                return StatusCode(404);
            }

            return category;
        }


        // PUT: api/Categories/5
        [HttpPut("CatID={CatID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutCategory(int CatID,string UserName,string Password, Category category)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {

                if (CatID != category.CatgoryId)
                {
                    return StatusCode(400);
                }
                using (var HR = _context)
                {
                    var ss = HR.Categorys.First(a => a.CatgoryId == category.CatgoryId);
                    ss.CatgoryName = category.CatgoryName;
                    try
                    {
                        await HR.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoryExists(CatID))
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


        // POST: api/Categories
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Category>> PostCategory(string UserName, string Password, Category category)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                using (var HR = _context)
                {
                     object SS = new Category {CatgoryName = category.CatgoryName };
                     HR.Add(SS);
                     await HR.SaveChangesAsync();
                }
                return Ok();
            }
            return StatusCode(403);
        }

        // DELETE: api/Categories/5
        [HttpDelete("CatID={CatID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteCategory(int CatID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole == 1 || UserRole == 2)
            {
                var Cat = await _context.Categorys.FindAsync(CatID);
                if (Cat == null)
                {
                    return StatusCode(404);
                }
                _context.Categorys.Remove(Cat);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(403);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categorys.Any(e => e.CatgoryId == id);
        }
    }
}
