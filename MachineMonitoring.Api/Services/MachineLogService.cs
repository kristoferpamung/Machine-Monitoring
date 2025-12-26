using MachineMonitoring.Api.DTOs;
using MachineMonitoring.Api.Hubs;
using MachineMonitoring.Api.Models;
using MachineMonitoring.Api.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace MachineMonitoring.Api.Services;

public class MachineLogService : IMachineLogService
{
    private readonly IMachineLogRepository _logRepo;
    private readonly IMachineRepository _machineRepo;

    private readonly IHubContext<MachineLogHub> _hubContext;

    public MachineLogService(
        IMachineLogRepository logRepo,
        IMachineRepository machineRepo,
        IHubContext<MachineLogHub> hubContext)
    {
        _logRepo = logRepo;
        _machineRepo = machineRepo;
        _hubContext = hubContext;
    }

    public async Task CreateAsync(CreateMachineLogDto dto)
    {
        var machine = await _machineRepo.GetByIdAsync(dto.MachineId);
        if (machine == null)
            throw new Exception("Machine not found");

        var log = new MachineLog
        {
            MachineId = dto.MachineId,
            OperatorId = dto.OperatorId,
            ProducedPerMinute = dto.ProducedPerMinute,
            MachineStatus = dto.MachineStatus,
            Temperature = dto.Temperature,
            LogTime = DateTime.UtcNow
        };

        await _logRepo.AddAsync(log);
        await _logRepo.SaveChangesAsync();

        // Broadcast after create log
        await _hubContext.Clients.All.SendAsync(
        "ReceiveMachineLog",
        new
        {
            log.Id,
            log.MachineId,
            MachineName = log.Machine.Name,
            log.OperatorId,
            OperatorName = log.Operator?.Name,
            log.ProducedPerMinute,
            log.MachineStatus,
            log.Temperature,
            log.LogTime
        });
    }

    public async Task<List<MachineLogDto>> GetByMachineAsync(int machineId)
    {
        var logs = await _logRepo.GetByMachineIdAsync(machineId);

        return logs.Select(x => new MachineLogDto
        {
            Id = x.Id,
            MachineId = x.MachineId,
            MachineName = x.Machine.Name,
            OperatorId = x.Operator?.Id,
            OperatorName = x.Operator?.Name,
            ProducedPerMinute = x.ProducedPerMinute,
            MachineStatus = x.MachineStatus,
            Temperature = x.Temperature,
            LogTime = x.LogTime
        }).ToList();
    }
}
