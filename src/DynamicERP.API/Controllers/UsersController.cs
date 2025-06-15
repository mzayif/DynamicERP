using DynamicERP.Core.Entities;
using DynamicERP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DynamicERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user, [FromQuery] string? password)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(user, password);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("external")]
    public async Task<ActionResult<User>> CreateExternal(User user, [FromQuery] string externalId, [FromQuery] string provider)
    {
        try
        {
            var createdUser = await _userService.CreateExternalUserAsync(user, externalId, provider);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        try
        {
            await _userService.UpdateUserAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPost("validate")]
    public async Task<ActionResult<bool>> ValidateCredentials([FromQuery] string email, [FromQuery] string password)
    {
        var isValid = await _userService.ValidateCredentialsAsync(email, password);
        return Ok(isValid);
    }

    [HttpPost("validate-external")]
    public async Task<ActionResult<bool>> ValidateExternalCredentials([FromQuery] string externalId, [FromQuery] string provider)
    {
        var isValid = await _userService.ValidateExternalCredentialsAsync(externalId, provider);
        return Ok(isValid);
    }
} 