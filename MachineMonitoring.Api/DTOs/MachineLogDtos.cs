using MachineMonitoring.Api.Enums;

namespace MachineMonitoring.Api.DTOs;

public class CreateMachineLogDto
{
    public int MachineId { get; set; }
    public int? OperatorId { get; set; }
    public int ProducedPerMinute { get; set; }
    public MachineStatus MachineStatus { get; set; }
    public decimal Temperature { get; set; }
}

public class MachineLogDto
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public string MachineName { get; set; } = "";
    public int? OperatorId { get; set; }
    public string? OperatorName { get; set; }
    public int ProducedPerMinute { get; set; }
    public MachineStatus MachineStatus { get; set; }
    public decimal Temperature { get; set; }
    public DateTime LogTime { get; set; }
}