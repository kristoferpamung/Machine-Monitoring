using MachineMonitoring.Api.Data;
using MachineMonitoring.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MachineMonitoring.Api.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly MachineMonitoringDbContext _context;

    public OperatorRepository(MachineMonitoringDbContext context)
    {
        _context = context;
    }

    public async Task<List<Operator>> GetAllAsync()
    {
        return await _context.Operators.ToListAsync();
    }

    public async Task<Operator?> GetByIdAsync(int id)
    {
        return await _context.Operators.FindAsync(id);
    }

    public async Task AddAsync(Operator op)
    {
        await _context.Operators.AddAsync(op);
    }

    public void Update(Operator op)
    {
        _context.Operators.Update(op);
    }

    public void Delete(Operator op)
    {
        _context.Operators.Remove(op);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}