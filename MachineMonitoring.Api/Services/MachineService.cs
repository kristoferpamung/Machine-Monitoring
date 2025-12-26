using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Models;
using MachineMonitoring.Api.Repositories;

namespace MachineMonitoring.Api.Services;

public class MachineService : IMachineService
{
    private readonly IMachineRepository _repository;

    public MachineService(IMachineRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MachineDto>> GetAllAsync()
    {
        var machines = await _repository.GetAllAsync();

        return machines.Select(m => new MachineDto
        {
            Id = m.Id,
            Name = m.Name
        }).ToList();
    }

    public async Task<MachineDto?> GetByIdAsync(int id)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null) return null;

        return new MachineDto
        {
            Id = machine.Id,
            Name = machine.Name,
        };
    }

    public async Task CreateAsync(CreateMachineDto dto)
    {
        var machine = new Machine
        {
            Name = dto.Name,
        };

        await _repository.AddAsync(machine);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UpdateMachineDto dto)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null)
            throw new Exception("Machine not found");

        machine.Name = dto.Name;

        _repository.Update(machine);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var machine = await _repository.GetByIdAsync(id);
        if (machine == null)
            throw new Exception("Machine not found");

        _repository.Delete(machine);
        await _repository.SaveChangesAsync();
    }
}
