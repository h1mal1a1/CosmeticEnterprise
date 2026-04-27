using CosmeticEnterpriseBack.Configuration;
using CosmeticEnterpriseBack.DTO.Auth;
using CosmeticEnterpriseBack.Interfaces;
using CosmeticEnterpriseBack.Services.Auth;
using CosmeticEnterpriseBack.Services.CurrentUser;
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
    private readonly IAuthCookieService _authCookieService;
    private readonly ICurrentUserService _currentUser;

    public AuthController(
        IAuthService authService,
        IAuthCookieService authCookieService,
        ICurrentUserService currentUser)
    {
        _authService = authService;
        _authCookieService = authCookieService;
        _currentUser = currentUser;
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
        var authResponse = await _authService.LoginAsync(request);
        _authCookieService.AppendAuthCookies(Response, authResponse);
        return Ok(new { message = "Login successful" });
    }

    /// <summary>
    /// Выполняет обвновление токенов
    /// </summary>
    /// <returns></returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies[AuthCookieNames.RefreshToken];
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Unauthorized();

        var authResponse = await _authService.RefreshAsync(refreshToken);
        _authCookieService.AppendAuthCookies(Response, authResponse);
        return Ok(new { message = "Tokens refreshed" });
    }

    /// <summary>
    /// Позволяет пользователю стать не авторизированным 
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        _authCookieService.DeleteAuthCookies(Response);
        return Ok(new { message = "Logout successful" });
    }

    /// <summary>
    /// Позволяет получить id, username и role пользователя
    /// </summary>
    /// <returns></returns>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        if (!_currentUser.UserId.HasValue)
            return Unauthorized();

        var response = await _authService.GetMeAsync(_currentUser.UserId.Value);
        return Ok(response);
    }

    /// <summary>
    /// Обновляет email и телефон текущего пользователя
    /// </summary>
    /// <param name="request">Новые email и телефон</param>
    /// <returns>Обновленные данные пользователя</returns>
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        if (!_currentUser.UserId.HasValue)
            return Unauthorized();

        var response = await _authService.UpdateProfileAsync(_currentUser.UserId.Value, request);
        return Ok(response);
    }
}