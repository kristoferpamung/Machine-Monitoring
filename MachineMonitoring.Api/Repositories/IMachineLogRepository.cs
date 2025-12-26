using MachineMonitoring.Api.Models;

namespace MachineMonitoring.Api.Repositories;

public interface IMachineLogRepository
{
    Task AddAsync(MachineLog log);
    Task<List<MachineLog>> GetByMachineIdAsync(int machineId);
    Task SaveChangesAsync();
}