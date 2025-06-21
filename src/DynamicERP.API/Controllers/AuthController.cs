using DynamicERP.Application.Features.Auth.Commands;
using DynamicERP.Core.RequestModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DynamicERP.API.Controllers;

/// <summary>
/// Kimlik doğrulama işlemleri için controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcı girişi yapar
    /// </summary>
    /// <param name="request">Giriş bilgileri</param>
    /// <returns>JWT token ve kullanıcı bilgileri</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password,
            RememberMe = request.RememberMe
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Test endpoint - Sadece giriş yapmış kullanıcılar erişebilir
    /// </summary>
    /// <returns>Başarı mesajı</returns>
    [HttpGet("test")]
    [Authorize]
    public IActionResult Test()
    {
        return Ok(new { message = "Authentication başarılı! Bu endpoint'e sadece giriş yapmış kullanıcılar erişebilir." });
    }

    /// <summary>
    /// Kullanıcı bilgilerini getirir (giriş yapmış kullanıcının)
    /// </summary>
    /// <returns>Kullanıcı bilgileri</returns>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst("UserId")?.Value;
        var username = User.FindFirst("Username")?.Value;
        var email = User.FindFirst("Email")?.Value;

        return Ok(new
        {
            UserId = userId,
            Username = username,
            Email = email,
            Claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
} 