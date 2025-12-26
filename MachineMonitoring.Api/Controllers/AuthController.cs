using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Models;
using MachineMonitoring.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MachineMonitoring.Api.Controllers;


/// <summary>
/// Authentication Controller using Microsoft Identity
/// Handles user registration, login, and profile management
/// Acts as security checkpoint for application access
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IJwtTokenService tokenService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Register new user account using Microsoft Identity
    /// Provides enterprise-grade security for account creation
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email is already registered." });
            }

            // Create new user with Identity validation and security
            var newUser = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Registration failed", errors });
            }

            // Assign default User role
            await _userManager.AddToRoleAsync(newUser, "User");

            _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

            return CreatedAtAction(nameof(GetProfile), new { }, new
            {
                message = "User registered successfully",
                userId = newUser.Id,
                email = newUser.Email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", registerDto.Email);
            return StatusCode(500, new { message = "Registration failed due to server error" });
        }
    }

    /// <summary>
    /// Login endpoint using Microsoft Identity SignInManager
    /// Validates credentials and issues JWT tokens with security features
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt with non-existent email: {Email}", loginDto.Email);
                return Unauthorized(new { message = "Invalid credentials." });
            }

            // Use Identity SignInManager for password validation with security policies
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed login attempt for user: {Email}", loginDto.Email);

                if (result.IsLockedOut)
                    return Unauthorized(new { message = "Account is locked out." });

                return Unauthorized(new { message = "Invalid credentials." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles.ToList());

            _logger.LogInformation("User {Email} logged in successfully", user.Email);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                Email = user.Email ?? "",
                FullName = user.FullName,
                Roles = roles.ToList(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
            return StatusCode(500, new { message = "Login failed due to server error" });
        }
    }

    /// <summary>
    /// Get current user profile information
    /// Requires valid JWT token for access
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new UserProfileDTO
            {
                Id = user.Id,
                Email = user.Email ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedAt = user.CreatedAt,
                Roles = roles.ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");
            return StatusCode(500, new { message = "Failed to retrieve profile" });
        }
    }
}
