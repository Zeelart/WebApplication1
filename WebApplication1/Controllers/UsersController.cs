using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly SupaBaseContext _context;
        private readonly ILogger<UsersController>? _logger;

        public UsersController(SupaBaseContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var users = await _context.GetAllUsers();
            return Ok(users.Select(UserResponse.FromModel));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await _context.GetUserById(id);
            return user == null ? NotFound() : Ok(UserResponse.FromModel(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request)
        {
            var user = request.ToModel();
            var createdUser = await _context.CreateUser(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser?.Id }, createdUser != null ? UserResponse.FromModel(createdUser) : null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            var existingUser = await _context.GetUserById(id);
            if (existingUser == null) return NotFound();

            request.ApplyTo(existingUser);
            await _context.UpdateUser(existingUser);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.DeleteUser(id);
            return result ? NoContent() : NotFound();
        }
    }
}