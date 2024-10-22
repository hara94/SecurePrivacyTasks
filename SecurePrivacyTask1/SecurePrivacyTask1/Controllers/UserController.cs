using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SecurePrivacyTask1.Models;
using SecurePrivacyTask1.Models.DTO;
using SecurePrivacyTask1.Repository;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMongoRepository<User> _repository;

    public UsersController(IMongoRepository<User> repository)
    {
        _repository = repository;
    }

    // GET: api/users
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsers()
    {
        // Retrieve the logged-in user's ID from the session
        var currentUserId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User not logged in.");
        }

        // Fetch users created by the logged-in user and project the result without password hash
        var users = await _repository.FindAsync(u => u.CreatedByUserId == currentUserId);
        var result = users.Select(user => new
        {
            id = user.Id,
            user.UserName,
            user.Email,
            user.Phone,
            user.Address,
            user.City,
            user.ConsentGiven,
            user.CanCreateUsers,
            user.CanDeleteUsers,
            user.CanEditUsers
        });

        return Ok(result);
    }


    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (await _repository.UserExists(registerDto.UserName))
        {
            return BadRequest(new { message = "Username is already taken" });  // Return JSON error response
        }

        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),  // Hash password
            ConsentGiven = registerDto.ConsentGiven
        };

        await _repository.AddAsync(user);

        // Return a JSON success response
        return Ok(new { success = true, message = "User registered successfully" });
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _repository.GetByUsernameAsync(loginDto.UserName);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials.");
        }

        // Store the user ID in the session after successful login
        HttpContext.Session.SetString("UserId", user.Id);

        // Issue authentication cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuthenticationScheme");

        await HttpContext.SignInAsync("MyCookieAuthenticationScheme", new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties
        {
            IsPersistent = true,  // This sets a persistent cookie, so the session lasts beyond browser restarts
            ExpiresUtc = DateTime.UtcNow.AddMinutes(30)  // Expiration for the cookie
        });

        return Ok(new { success = true, message = "Logged in successfully", userId = user.Id });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Clear the session and the authentication cookie
        HttpContext.Session.Clear();
        HttpContext.SignOutAsync("MyCookieAuthenticationScheme");

        return Ok(new { success = true, message = "Logged out successfully" });
    }

    // POST: api/users
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateUser([FromBody] UserDto newUserDto)
    {
        var currentUserId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User not logged in.");
        }

        var currentUser = await _repository.GetByIdAsync(currentUserId);

        if (!currentUser.CanCreateUsers)
        {
            return Forbid("You do not have permission to create users.");
        }

        var newUser = new User
        {
            UserName = newUserDto.UserName,
            Email = newUserDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("randomPassword"),
            ConsentGiven = newUserDto.ConsentGiven,
            CanCreateUsers = newUserDto.CanCreateUsers,
            CanDeleteUsers = newUserDto.CanDeleteUsers,
            CanEditUsers = newUserDto.CanEditUsers,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = currentUserId
        };

        try
        {
            await _repository.AddAsync(newUser);
            return Ok(new { success = true, message = "User created successfully" });
        }
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            // Handle duplicate key error (violating unique index constraint)
            return BadRequest(new { success = false, message = "A user with the same username or email already exists." });
        }
        catch (Exception)
        {
            // Log the exception if needed
            return StatusCode(500, new { success = false, message = "An unexpected error occurred. Please try again." });
        }
    }

    [HttpGet("is-authenticated")]
    [Authorize] 
    public IActionResult IsAuthenticated()
    {
        // If the request reaches here, the user is authenticated
        return Ok(true);
    }


    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] User updatedUser)
    {
        var currentUserId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User not logged in.");
        }

        var currentUser = await _repository.GetByIdAsync(currentUserId);

        if (currentUser == null || !currentUser.CanEditUsers)
        {
            return Forbid();  // Deny the update if the user doesn't have permission
        }

        if (id != updatedUser.Id)
        {
            return BadRequest();
        }

        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _repository.UpdateAsync(id, updatedUser);
        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        await _repository.DeleteAsync(id);
        // Ensure all backups and logs are also purged
        return NoContent();
    }

    [HttpPost("save-consent")]
    public async Task<IActionResult> SaveConsent([FromBody] ConsentDto consentDto)
    {
        // Retrieve the logged-in user's ID from the session
        var currentUserId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User not logged in.");
        }

        // Fetch the current user from the repository
        var user = await _repository.GetByIdAsync(currentUserId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Update the user's consent status
        user.ConsentGiven = consentDto.ConsentGiven;

        // Save the updated user consent status
        await _repository.UpdateAsync(currentUserId, user);

        return Ok(new { success = true, message = "Consent updated successfully" });
    }

    [HttpGet("access-my-data")]
    [Authorize]
    public async Task<IActionResult> AccessMyData()
    {
        // Retrieve the logged-in user's ID from the session
        var currentUserId = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized("User not logged in.");
        }

        // Fetch the user data from the repository using the current user's ID
        var user = await _repository.GetByIdAsync(currentUserId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Construct a response with personal data but exclude sensitive information like password hash
        var userData = new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.Phone,
            user.Address,
            user.City,
            user.ConsentGiven,
            user.CanCreateUsers,
            user.CanDeleteUsers,
            user.CanEditUsers,
            user.CreatedAt,
            user.CreatedByUserId
        };

        return Ok(userData); // Return the user's personal data
    }


}
