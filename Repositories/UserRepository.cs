using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repositories.Interfaces;
using ApiEcommerce.Utils;
using Microsoft.EntityFrameworkCore;

//Mi repositorio
namespace ApiEcommerce.Repositories;

public class UserRepository : IUserRepository
{
    public readonly ApplicationDbContext _db;
    private readonly TokenService _tokenService;
    private string? secretKey;

    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException("La clave secreta para JWT no está configurada.");
        }

        _tokenService = new TokenService(secretKey);
    }
    /// <summary>
    /// Obtener un usuario
    /// </summary>
    /// <param name="id">id del usuario a encontrar</param>
    /// <returns>usuario</returns>
    public User? GetUser(int id)
    {
        return _db.Users.FirstOrDefault(user => user.Id == id);
    }

    /// <summary>
    /// Retornar todos los usuarios
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    public ICollection<User> GetUsers()
    {
        return [.. _db.Users.OrderBy(user => user.Username)];
    }

    /// <summary>
    /// Verificar si un usuario ya existe
    /// </summary>
    /// <param name="username">nombre de usuario a verificar</param>
    /// <returns>true si el usuario existe</returns>
    public bool IsUniqueUser(string username)
    {
        return !_db.Users.Any(user => user.Username.ToLower().Trim() == username.ToLower().Trim());

    }

    /// <summary>
    /// Iniciar Sesion con Usuario
    /// </summary>
    /// <param name="userLoginDto">Credenciales para inicio de sesion</param>
    /// <returns>usuario y token de ingreso</returns>
    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        if (string.IsNullOrEmpty(userLoginDto.Username))
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = "El nombre de usuario es requerido",
                StatusCode = StatusCodes.Status400BadRequest
            };


        var user = await _db.Users.FirstOrDefaultAsync(
            user => user.Username.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());

        if (user == null)
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = "Usuario no encontrado",
                StatusCode = StatusCodes.Status404NotFound
            };

        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            return new UserLoginResponseDto()
            {
                User = null,
                Token = "",
                Message = "Credenciales incorrectas",
                StatusCode = StatusCodes.Status401Unauthorized
            };

        var token = _tokenService.GenerateToken(user.Id, user.Username);

        return new UserLoginResponseDto()
        {
            Token = token,
            User = new UserResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
            },

            Message = "Inicio de sesión exitoso",
            StatusCode = StatusCodes.Status200OK
        };

    }

    /// <summary>
    /// Registrar un usuario
    /// </summary>
    /// <param name="createUserDto">Credenciales de usuario</param>
    /// <returns>Usuario Creado</returns>
    public async Task<UserLoginResponseDto> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, 10);

        var user = new User
        {
            Username = createUserDto.Username,
            Password = encriptedPassword,
            Name = createUserDto.Name,
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return await Login(new UserLoginDto()
        {
            Username = createUserDto.Username,
            Password = createUserDto.Password
        });
    }
}
