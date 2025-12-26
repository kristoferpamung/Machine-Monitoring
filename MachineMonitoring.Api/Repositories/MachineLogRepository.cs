using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MachineMonitoring.Api.Repositories;
public class MachineLogRepository : IMachineLogRepository
{
    private readonly MachineMonitoringDbContext _context;

    public MachineLogRepository(MachineMonitoringDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MachineLog log)
    {
        await _context.MachineLogs.AddAsync(log);
    }

    public async Task<List<MachineLog>> GetByMachineIdAsync(int machineId)
    {
        return await _context.MachineLogs
            .Include(machineLog => machineLog.Machine)
            .Include(machineLog => machineLog.Operator)
            .Where(machineLog => machineLog.MachineId == machineId)
            .OrderByDescending(machineLog => machineLog.LogTime)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
