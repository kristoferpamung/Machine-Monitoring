using System.ComponentModel.DataAnnotations;

namespace MachineMonitoring.Api.Models;

public class Operator
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Operator name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Operator name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    public ICollection<MachineLog> MachineLogs = new List<MachineLog>();
}