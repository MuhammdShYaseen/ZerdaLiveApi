using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZerdaLiveApi.Attributes;
using ZerdaLiveApi.Helpper;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeysController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ApiKeysController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/ApiKeys
        [HttpGet("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<IEnumerable<ApiKey>>> GetApiKeys(string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            
            var ss = await _context.ApiKeys.ToListAsync();
            return  ss;
        }
        [HttpGet("APIKEY={APIKEY}")]
        public async Task <ActionResult<IEnumerable<ApiKey>>> IsApiKeyExist(string APIKEY)
        {
            //var ipaddress = Request.HttpContext.Connection.RemoteIpAddress;
            var ss = await _context.ApiKeys.Where(c => c.ApiKey1 == APIKEY).ToListAsync();
            if (!ss.Any())
            {
                return StatusCode(404);
            }
            else
            {
                return ss;
            }
             
        }

        // GET: api/ApiKeys/5
        [HttpGet("AID={AID}&UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<ApiKey>> GetApiKey(int AID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var apiKey = await _context.ApiKeys.FindAsync(AID);

            if (apiKey == null)
            {
                return StatusCode(404);
            }

            return apiKey;
        }

        // PUT: api/ApiKeys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("AID={AID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutApiKey(int AID, string UserName, string Password, ApiKey apiKey)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (AID != apiKey.ApiKeyId)
            {
                return StatusCode(400);
            }
            using (_context)
            {
                var ss = _context.ApiKeys.First(a => a.ApiKeyId == apiKey.ApiKeyId);
                ss.ApiKey1 = apiKey.ApiKey1;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApiKeyExists(AID))
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

        // POST: api/ApiKeys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<ApiKey>> PostApiKey(string UserName, string Password, ApiKey apiKey)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                 return StatusCode(403);
            }
            using (var ZContext = _context)
            {
                object SS = new ApiKey { ApiKey1 = apiKey.ApiKey1 };
                ZContext.Add(SS);
                await ZContext.SaveChangesAsync();
            }
            return Ok();
        }

        // DELETE: api/ApiKeys/5
        [HttpDelete("AID={AID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteApiKey(int AID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var apiKey = await _context.ApiKeys.FindAsync(AID);
            if (apiKey == null)
            {
                return StatusCode(404);
            }

            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApiKeyExists(int id)
        {
            return _context.ApiKeys.Any(e => e.ApiKeyId == id);
        }
    }
}
