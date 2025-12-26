using System.Security.Claims;
using MachineMonitoring.Api.Models;

namespace MachineMonitoring.Api.Services;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user, List<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}