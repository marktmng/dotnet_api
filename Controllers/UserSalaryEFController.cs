using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetAPI.Data;
using DotnetAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSalaryEfController : ControllerBase
    {
        private readonly DataContextEF _context;

        public UserSalaryEfController(DataContextEF context)
        {
            _context = context;
        }

        // GET: api/UserSalaryEf
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSalary>>> GetUserSalaries()
        {
            return await _context.UserSalaries.ToListAsync();
        }

        // GET: api/UserSalaryEf/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserSalary>> GetUserSalary(int id)
        {
            var userSalary = await _context.UserSalaries.FindAsync(id);

            if (userSalary == null)
            {
                return NotFound();
            }

            return userSalary;
        }

        // POST: api/UserSalaryEf
        [HttpPost]
        public async Task<ActionResult<UserSalary>> PostUserSalary(UserSalary userSalary)
        {
            _context.UserSalaries.Add(userSalary);
            await _context.SaveChangesAsync();

            // Return a created response with the location of the newly created entity
            return CreatedAtAction("GetUserSalary", new { id = userSalary.UserId }, userSalary);
        }

        // PUT: api/UserSalaryEf/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserSalary(int id, UserSalary userSalary)
        {
            if (id != userSalary.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userSalary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserSalaryExists(id))
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

        // DELETE: api/UserSalaryEf/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSalary(int id)
        {
            var userSalary = await _context.UserSalaries.FindAsync(id);
            if (userSalary == null)
            {
                return NotFound();
            }

            _context.UserSalaries.Remove(userSalary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserSalaryExists(int id)
        {
            return _context.UserSalaries.Any(e => e.UserId == id);
        }
    }
}
