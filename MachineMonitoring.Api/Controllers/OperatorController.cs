using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MachineMonitoring.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorsController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    // GET: api/machines
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var machines = await _operatorService.GetAllAsync();
        return Ok(machines);
    }

    // GET: api/machines/5
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var machine = await _operatorService.GetByIdAsync(id);
        if (machine == null)
            return NotFound();

        return Ok(machine);
    }

    // POST: api/machines
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateOperatorDto dto)
    {
        await _operatorService.CreateAsync(dto);
        return Created("", null);
    }

    // PUT: api/machines/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOperatorDto dto)
    {
        try
        {
            await _operatorService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/machines/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _operatorService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
