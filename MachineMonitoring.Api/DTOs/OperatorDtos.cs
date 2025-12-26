using System.ComponentModel.DataAnnotations;

namespace MachineMonitoring.Api.DTOs;

public class CreateOperatorDto
{
    [Required(ErrorMessage = "Operator name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Operator name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;
}

public class UpdateOperatorDto
{
    [Required(ErrorMessage = "Operator name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Operator name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;
}

public class OperatorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}