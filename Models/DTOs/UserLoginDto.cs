using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.DTOs;

public class UserLoginDto
{

    [Required(ErrorMessage = "El campo username es requerido")]
    [DefaultValue("jhon_doe")]
    public string Username { get; set; } = string.Empty;

    [DefaultValue("password12345")]
    [Required(ErrorMessage = "El campo password es requerido")]
    public string Password { get; set; } = string.Empty;
}
