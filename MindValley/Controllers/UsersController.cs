using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity; // For password hashing
using Microsoft.EntityFrameworkCore;
using MindValley.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MindValley.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MindValleyContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public UsersController(MindValleyContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Users (Get all users)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // ✅ GET: api/Users/{id} (Get user by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new { message = "User not found." });

            return user;
        }

        // ✅ PUT: api/Users/{id} (Update user)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest(new { message = "User ID mismatch." });

            // Ensure password is not updated to plain text
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return NotFound(new { message = "User not found." });

            // Update user fields (except password)
            existingUser.Email = user.Email;
            existingUser.Name = user.Name;
            existingUser.Age = user.Age;
            existingUser.Gender = user.Gender;

            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound(new { message = "User not found." });
                else
                    throw;
            }

            return NoContent();
        }

        // ✅ POST: api/Users/register (User registration with hashed password)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (_context.Users == null)
                return NotFound(new { message = "User database not found." });

            // Check if the email is already in use
            var existingUser = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
            if (existingUser)
                return BadRequest(new { message = "Email is already in use." });

            var user = new User
            {
                Email = registerDto.Email,
                Name = registerDto.Name,
                Age = registerDto.Age,
                Gender = registerDto.Gender
            };

            // Hash the password properly
            user.Password = _passwordHasher.HashPassword(user, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Hashed Password: {user.Password}"); // <-- Debug output

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new
            {
                user.UserId,
                user.Email,
                user.Name,
                user.Age,
                user.Gender
            });
        }

        // ✅ POST: api/Users/authenticate (User login with password verification)
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            if (_context.Users == null)
                return NotFound(new { message = "User database not found." });

            // Find user by email
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                Console.WriteLine("❌ User not found with email: " + loginDto.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Verify hashed password against provided password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

            Console.WriteLine($"🔍 Hashed Password: {user.Password}");
            Console.WriteLine($"🔍 Entered Password: {loginDto.Password}");
            Console.WriteLine($"🔍 Verification Result: {result}");

            if (result != PasswordVerificationResult.Success)
            {
                Console.WriteLine("❌ Password verification failed.");
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new
            {
                user.UserId,
                user.Email,
                user.Name,
                user.Age,
                user.Gender
            });
        }

        // ✅ DELETE: api/Users/{id} (Delete user)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Helper: Check if a user exists by ID
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        // ✅ DTO: Data Transfer Object for login
        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // ✅ DTO: Data Transfer Object for registration
        public class RegisterDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string? Name { get; set; }
            public int? Age { get; set; }
            public string? Gender { get; set; }
        }
    }
}
