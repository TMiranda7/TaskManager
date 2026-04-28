using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.Tasks.Requests;

public class CreateTaskRequest
{
    [Required(ErrorMessage = "O Título deve ser informado")]
    [StringLength(35, ErrorMessage = "Deve conter no maximo 35 caracteres")]
    public string Title { get; set; } = string.Empty;
}