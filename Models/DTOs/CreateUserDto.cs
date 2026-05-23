using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage = "El campo username es requerido")]
    [DefaultValue("jhon_doe")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo name es requerido")]
    [DefaultValue("John Doe")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo password es requerido")]
    [DefaultValue("password12345")]
    public string Password { get; set; } = string.Empty;

    // [Required(ErrorMessage = "El campo role es requerido")]
    // public string Role { get; set; } = string.Empty;
}