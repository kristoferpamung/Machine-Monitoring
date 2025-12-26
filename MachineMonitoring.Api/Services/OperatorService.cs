using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Models;
using MachineMonitoring.Api.Repositories;

namespace MachineMonitoring.Api.Services;

public class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _repository;

    public OperatorService(IOperatorRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OperatorDto>> GetAllAsync()
    {
        var operators = await _repository.GetAllAsync();

        return operators.Select(op => new OperatorDto
        {
            Id = op.Id,
            Name = op.Name
        }).ToList();
    }

    public async Task<OperatorDto?> GetByIdAsync(int id)
    {
        var op = await _repository.GetByIdAsync(id);
        if (op == null) return null;

        return new OperatorDto
        {
            Id = op.Id,
            Name = op.Name,
        };
    }

    public async Task CreateAsync(CreateOperatorDto dto)
    {
        var op = new Operator
        {
            Name = dto.Name,
        };

        await _repository.AddAsync(op);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UpdateOperatorDto dto)
    {
        var op = await _repository.GetByIdAsync(id);
        if (op == null)
            throw new Exception("Operator not found");

        op.Name = dto.Name;

        _repository.Update(op);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var op = await _repository.GetByIdAsync(id);
        if (op == null)
            throw new Exception("Operator not found");

        _repository.Delete(op);
        await _repository.SaveChangesAsync();
    }
}
