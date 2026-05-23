namespace ApiEcommerce.Models.DTOs;

public class UserRegisterDto
{
    
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public required string Username { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
}
