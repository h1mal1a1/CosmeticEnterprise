using CosmeticEnterpriseBack.DTO.Auth;
using CosmeticEnterpriseBack.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticEnterpriseBack.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Регистрирует пользователя
    /// </summary>
    /// <param name="request">Логин/пароль пользователя</param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Ok();
    }
    
    /// <summary>
    /// Выполняет вход пользователя и возвращает JWT токен.
    /// </summary>
    /// <param name="request">Логин и пароль пользователя</param>
    /// <returns>AccessToken и Refresh tokens</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshAsync(request);
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var idUserClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(idUserClaim) || !long.TryParse(idUserClaim, out var idUser))
            return Unauthorized();
        var response = await _authService.GetMeAsync(idUser);
        return Ok(response);
    }

}