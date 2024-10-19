using Microsoft.AspNetCore.Mvc;
using SecurePrivacyTask1.Models;
using SecurePrivacyTask1.Repository;

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
    public async Task<IActionResult> GetUsers()
    {
        var users = await _repository.GetAllAsync();
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("User is null.");
        }

        if(!user.ConsentGiven)
        {
            return BadRequest("User consent is required.");
        }

        // Insert the user (MongoDB will generate the Id)
        await _repository.AddAsync(user);

        // After insertion, the `user.Id` will be populated with the generated ObjectId.
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] User user)
    {
        var existingUser = await _repository.GetByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        // Update fields
        existingUser.UserName = user.UserName;
        existingUser.Password = user.Password;
        existingUser.Email = user.Email;
        existingUser.Phone = user.Phone;
        existingUser.Address = user.Address;
        existingUser.City = user.City;
        existingUser.ConsentGiven = user.ConsentGiven;

        await _repository.UpdateAsync(id, existingUser);
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
}
