using MachineMonitoring.Api.Models;

namespace MachineMonitoring.Api.Repositories;
public interface IMachineRepository
{
    Task<List<Machine>> GetAllAsync();
    Task<Machine?> GetByIdAsync(int id);
    Task AddAsync(Machine machine);
    void Update(Machine machine);
    void Delete(Machine machine);
    Task SaveChangesAsync();
}
