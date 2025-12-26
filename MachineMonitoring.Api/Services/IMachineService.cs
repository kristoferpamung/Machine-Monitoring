using MachineMonitoring.Api.DTOs;

namespace MachineMonitoring.Api.Services;

public interface IMachineService
{
    Task<List<MachineDto>> GetAllAsync();
    Task<MachineDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateMachineDto dto);
    Task UpdateAsync(int id, UpdateMachineDto dto);
    Task DeleteAsync(int id);
}