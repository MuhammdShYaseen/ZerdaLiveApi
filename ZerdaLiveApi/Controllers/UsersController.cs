using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Attributes;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{   
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public UsersController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Users
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string UserName, string Password)
        {
           
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            return await _context.Users.ToListAsync();
        }

        [HttpGet("Buffer={Buffer}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<User>>> IsUsernameAndPasswordVaild(string UserName, string Password)
        {
            //var ipaddress = Request.HttpContext.Connection.RemoteIpAddress;
            var ss = await _context.Users.Where(c => c.UserName == UserName && c.Password == Password).ToListAsync();
            if (!ss.Any())
            {
                return StatusCode(404);
            }
            else
            {
                return ss;
            }

        }

        // GET: api/Users/5
        [HttpGet("UID={UID}&UserName={UserName}&Password={Password}")]
        public async Task <ActionResult<User>> GetUser(string UserName, string Password)
        {
            var ss = _context;
            var FindUser =await ss.Users.Where(c => c.UserName.Equals(UserName) && c.Password.Equals(Password)).ToListAsync();
            //List<User> Fin = new();
            //Fin =  FindUser.ToList();
            var user = new User();
            foreach (var u in FindUser)
            {
                user.Password = u.Password;
                user.Role = u.Role;
                user.UserName = u.UserName;
                user.UserId = u.UserId;
                if (user == null)
                {
                    return StatusCode(404);
                }
            }
            

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UID={UID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutUser(int UID, string UserName, string Password, User user)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }

            if (UID != user.UserId)
            {
                return StatusCode(400);
            }
            using (_context)
            {
                var ss = _context.Users.First(a => a.UserId == user.UserId);
                ss.UserName = user.UserName;
                ss.Password = user.Password;
                ss.Role = user.Role;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(UID))
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

        // POST: api/Users
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<User>> PostUser(string UserName, string Password, User user)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var SearchResult = _context.Users.Where(c=> c.UserName.Equals(user.UserName)).ToList().Any();
            if(SearchResult == false)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(400);
            }
            
        }

        // DELETE: api/Users/5
        [HttpDelete("UID={UID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteUser(int UID,string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var user = await _context.Users.FindAsync(UID);
            if (user == null)
            {
                return StatusCode(404);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
