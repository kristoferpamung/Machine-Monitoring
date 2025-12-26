
using System.ComponentModel.DataAnnotations;

namespace MachineMonitoring.Api.Models;

public class Machine
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Machine name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Machine name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    public ICollection<MachineLog> MachineLogs = new List<MachineLog>();
}