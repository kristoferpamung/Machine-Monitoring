using MachineMonitoring.Api.Models;

namespace MachineMonitoring.Api.Repositories;
public interface IOperatorRepository
{
    Task<List<Operator>> GetAllAsync();
    Task<Operator?> GetByIdAsync(int id);
    Task AddAsync(Operator op);
    void Update(Operator op);
    void Delete(Operator op);
    Task SaveChangesAsync();
}