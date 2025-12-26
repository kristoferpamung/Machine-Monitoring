using MachineMonitoring.Api.DTOs;

namespace MachineMonitoring.Api.Services;

public interface IOperatorService
{
    Task<List<OperatorDto>> GetAllAsync();
    Task<OperatorDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateOperatorDto dto);
    Task UpdateAsync(int id, UpdateOperatorDto dto);
    Task DeleteAsync(int id);
}