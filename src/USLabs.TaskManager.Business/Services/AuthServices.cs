using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using USLabs.TaskManager.Business.Services.Interfaces;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;
using USLabs.TaskManager.Shared.DTOs.Auth;

namespace USLabs.TaskManager.Business.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthServices(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(loginDTO.Email);
                if (user == null || !user.IsActive)
                {
                    return null;
                }

                //Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
                {
                    return null;
                }

                //Generate jwt token
                var token = GenerateJwtToken(user);
                var userDTO = _mapper.Map<UserDTO>(user);
                return new AuthResponseDTO
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    User = userDTO
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuthResponseDTO?> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                if (await _userRepository.ExistsEmailAsync(registerDTO.Email))
                {
                    return null; //Email already exists
                }

                //mapping dto to entity
                var user = _mapper.Map<User>(registerDTO);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

                var createdUser = await _userRepository.CreateUserAsync(user);

                var token = GenerateJwtToken(createdUser);
                var userDto = _mapper.Map<UserDTO>(createdUser);
           
                return new AuthResponseDTO
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    User = userDto
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    return null;
                }

                return _mapper.Map<UserDTO>(user);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ValidateUserAsync(Guid userId)
        {
            try
            {
                return await _userRepository.ExistsUserAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }



        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = Encoding.ASCII.GetBytes(secretKey!);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new("userId", user.Id.ToString()),
                new("email", user.Email!)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}