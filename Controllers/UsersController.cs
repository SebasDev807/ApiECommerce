using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Models.DTOs;
using ApiEcommerce.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;

        public UsersController(
            IUserRepository userRepository,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpPost("create-account", Name = "Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
        {
            if (!_userRepository.IsUniqueUser(createUserDto.Username))
            {
                return Conflict(new { message = "El nombre de usuario ya existe" });
            }


            var user = await _userRepository.Register(createUserDto);

            return StatusCode(StatusCodes.Status201Created, user);

        }
        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var response = await _userRepository.Login(userLoginDto);

            if (response.StatusCode == StatusCodes.Status400BadRequest)
                return BadRequest(response);

            if (response.StatusCode == StatusCodes.Status404NotFound)
                return NotFound(response);

            if (response.StatusCode == StatusCodes.Status401Unauthorized)
                return Unauthorized(response);

            return Ok(response);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var userDb = _userRepository.GetUser(id);

            if (userDb == null)
                return NotFound(new { message = $"El usuario con Id {id} no existe" });

            var user = _mapper.Map<UserResponseDto>(userDb);
            return Ok(user);
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var usersDb = _userRepository.GetUsers();
            var users = usersDb.Select(_mapper.Map<UserResponseDto>);
            return Ok(users);
        }

    }

}
