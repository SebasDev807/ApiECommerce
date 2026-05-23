using System;
using System.Text.Json.Serialization;

namespace ApiEcommerce.Models.DTOs;



public class UserLoginResponseDto
{
    public UserResponseDto? User { get; set; }
    public string Token { get; set; } = string.Empty;
    public string? Message { get; set; }
    
    [JsonIgnore]
    public int StatusCode { get; set; }
}
