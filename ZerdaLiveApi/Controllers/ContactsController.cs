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
    public class ContactsController : ControllerBase
    {
        private readonly ZerdaLiveContext _context;
        private readonly UserRole URole;
        public ContactsController(ZerdaLiveContext context, UserRole URolee)
        {
            _context = context;
            URole = URolee;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return StatusCode(404);
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("CoID={CoID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> PutContact(int CoID, string UserName, string Password, Contact contact)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            if (CoID != contact.AccountId)
            {
                return StatusCode(400);
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(CoID))
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

        // POST: api/Contacts

        [HttpPost("UserName={UserName}&Password={Password}")]
        public async Task<ActionResult<Contact>> PostContact(string UserName, string Password, Contact contact)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.AccountId }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("CoID={CoID}&UserName={UserName}&Password={Password}")]
        public async Task<IActionResult> DeleteContact(int CoID, string UserName, string Password)
        {
            int UserRole = URole.CheckUserRolee(UserName, Password);
            if (UserRole != 1)
            {
                return StatusCode(403);
            }
            var contact = await _context.Contacts.FindAsync(CoID);
            if (contact == null)
            {
                return StatusCode(404);
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return StatusCode(204);
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.AccountId == id);
        }
    }
}
