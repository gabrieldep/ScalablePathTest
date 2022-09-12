using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.OM;
using ScalablePathTest.DTO;
using ScalablePathTest.Models;
using System.Net;

namespace ScalablePathTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ScalablePathDbContext _context;
        private static readonly RedisConnectionProvider _provider = new("redis://localhost:6379");

        public UsersController(ScalablePathDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            var users = _provider.RedisCollection<User>();
            if (users.Count() == 0)
            {
                var userFromDb = _context.Users
                    .Select(u => new UserDTO
                    {
                        dob = u.DateOfBirth.HasValue ? u.DateOfBirth.Value.ToString("yyyy-MM-dd") : null,
                        id = u.Id.ToString(),
                        lead_source = u.LeadSource,
                        name = u.Name
                    });
                return StatusCode((int)HttpStatusCode.OK, users);
            }
            return StatusCode((int)HttpStatusCode.OK, users);
        }

        [HttpDelete(Name = "DeleteUser")]
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return StatusCode((int)HttpStatusCode.BadRequest, new { message = "Id has no value" });

            var userToDelete = _context.Users.Find(id.Value);
            if (userToDelete == null)
                return StatusCode((int)HttpStatusCode.NotFound, new { message = "User not found" });

            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return StatusCode((int)HttpStatusCode.OK, new { message = "User removed!" });
        }

        [HttpPut(Name = "PutUser")]
        public async Task<IActionResult> PutAsync(int? id, [FromBody] UserDTO dto)
        {
            if (!id.HasValue)
                return StatusCode((int)HttpStatusCode.BadRequest, new { message = "Id has no value" });

            var user = _context.Users.Find(id.Value);
            if (user == null)
                return StatusCode((int)HttpStatusCode.NotFound, new { message = "User not found" });

            if (!DateTime.TryParse(dto.dob, out _))
                return StatusCode((int)HttpStatusCode.BadRequest, new { message = "Date in wrong format" });

            user.Name = string.IsNullOrEmpty(dto.name) ? user.Name : dto.name;
            user.DateOfBirth = string.IsNullOrEmpty(dto.dob) ? user.DateOfBirth : DateTime.Parse(dto.dob);
            user.LeadSource = string.IsNullOrEmpty(dto.lead_source) ? user.LeadSource : dto.lead_source;
            await _context.SaveChangesAsync();
            return StatusCode((int)HttpStatusCode.OK, new { message = "User updated!" });
        }
    }
}
