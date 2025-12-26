
using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MachineMonitoring.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachineLogsController : ControllerBase
{
    private readonly IMachineLogService _service;

    public MachineLogsController(IMachineLogService service)
    {
        _service = service;
    }

    // POST: api/machine-logs
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMachineLogDto dto)
    {
        await _service.CreateAsync(dto);
        return StatusCode(201);
    }

    // GET: api/machine-logs/machine/1
    [HttpGet("machine/{machineId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByMachine(int machineId)
    {
        var logs = await _service.GetByMachineAsync(machineId);
        return Ok(logs);
    }
}
