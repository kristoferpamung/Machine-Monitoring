using MachineMonitoring.Api.Enums;

namespace MachineMonitoring.Api.Models;

public class MachineLog
{
    public int Id { get; set; }
    public int MachineId { get; set; }
    public Machine Machine { get; set; } = null!;
    public int? OperatorId { get; set; }
    public Operator? Operator { get; set; }
    public int ProducedPerMinute { get; set; }
    public MachineStatus MachineStatus { get; set; }
    public decimal Temperature { get; set; }
    public DateTime LogTime { get; set; }
}