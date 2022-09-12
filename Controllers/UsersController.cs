using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public UsersController(ScalablePathDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            var users = _context.Users
                .ToArray()
                .Select(u => new UserDTO
                {
                    dob = u.DateOfBirth.ToString("yyyy-MM-dd"),
                    id = u.Id.ToString(),
                    lead_source = u.LeadSource,
                    name = u.Name
                });
            return StatusCode((int)HttpStatusCode.OK, users);
        }
    }
}
