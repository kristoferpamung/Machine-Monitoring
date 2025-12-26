using MachineMonitoring.Api.DTOs;

namespace MachineMonitoring.Api.Services;
public interface IMachineLogService
{
    Task CreateAsync(CreateMachineLogDto dto);
    Task<List<MachineLogDto>> GetByMachineAsync(int machineId);
}
