using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MachineMonitoring.Api.Repositories;


public class MachineRepository : IMachineRepository
{
    private readonly MachineMonitoringDbContext _context;

    public MachineRepository(MachineMonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<List<Machine>> GetAllAsync()
    {
        return await _context.Machines.ToListAsync();
    }

    public async Task<Machine?> GetByIdAsync(int id)
    {
        return await _context.Machines.FindAsync(id);
    }

    public async Task AddAsync(Machine machine)
    {
        await _context.Machines.AddAsync(machine);
    }

    public void Update(Machine machine)
    {
        _context.Machines.Update(machine);
    }

    public void Delete(Machine machine)
    {
        _context.Machines.Remove(machine);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}