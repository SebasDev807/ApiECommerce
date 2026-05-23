using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Utils;

public class TokenService
{
    private readonly string _secretKey;

    public TokenService(string secretKey)
    {
        _secretKey = secretKey;
    }

    /// <summary>
    /// Genera un JSON Web Token (JWT) firmado para autenticar al usuario.
    /// </summary>
    /// <param name="userId">Identificador único del usuario que se incluirá en el payload del token.</param>
    /// <param name="username">Nombre de usuario que se incluirá como claim en el payload del token.</param>
    /// <returns>Token JWT como cadena de texto lista para enviar al cliente.</returns>
    public string GenerateToken(int userId, string username)
    {
        var handlerToken = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
              new Claim("id", userId.ToString()),
              new Claim(ClaimTypes.Name, username)
            ]),

            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handlerToken.CreateToken(tokenDescriptor);
        return handlerToken.WriteToken(token);


    }

}
