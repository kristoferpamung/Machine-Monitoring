using MachineMonitoring.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MachineMonitoring.Api.Services;

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Generate JWT token for authenticated user using Microsoft Identity
        /// Creates digital ID card with user claims and security features
        /// </summary>
        public string GenerateToken(ApplicationUser user, List<string> roles)
        {
            try
            {
                // Build claims - personal details for the digital ID card
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim("full_name", user.FullName)
                };

                // Add role claims for authorization
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Configure token security
                var secretKey = _configuration["JWT:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expirationTime = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JWT:ExpirationInMinutes"] ?? "60"));

                // Create and sign the token
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: credentials
                );

                _logger.LogInformation("JWT token generated successfully for user {Email}", user.Email);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for user {Email}", user.Email);
                throw;
            }
        }

        /// <summary>
        /// Validate JWT token and extract user information
        /// Verifies digital ID card authenticity and expiration
        /// </summary>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration["JWT:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured");
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Token validation failed");
                return null;
            }
        }
    }