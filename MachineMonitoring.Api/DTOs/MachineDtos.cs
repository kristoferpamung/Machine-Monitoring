using System.ComponentModel.DataAnnotations;

namespace MachineMonitoring.Api.DTOs;

public class CreateMachineDto
{
    [Required(ErrorMessage = "Machine name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Machine name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
}

public class UpdateMachineDto
{
    [Required(ErrorMessage = "Machine name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Machine name must be between 3 and 100 characters")]
    public string Name { get; set; } = string.Empty;
}

public class MachineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}