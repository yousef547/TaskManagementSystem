using Asp.Versioning;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Common.Dtos.Requests.Auth;
using TaskManagement.Application.Common.Dtos.Response;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Common.Responses;
using TaskManagement.Infrastructure.Identity;

namespace TaskManagement.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {

            var existingUser =
                await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return BadRequest(
            ApiResponse<string>.FailureResponse(
                "Email already exists"));
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(
                user,
                request.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors
           .Select(x => x.Description)
           .ToList();

                return BadRequest(
                    ApiResponse<string>.FailureResponse(
                        "Registration failed",
                        errors));
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email!,
                roles);
            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                Roles = roles
            };


            return Ok(
                ApiResponse<AuthResponseDto>.SuccessResponse(
                    response,
                    "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogintDto request)
        {
            var user =
                await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Unauthorized(
        ApiResponse<string>.FailureResponse(
            "Invalid credentials"));
            }

            var isPasswordCorrect =
                await _userManager.CheckPasswordAsync(
                    user,
                    request.Password);

            if (!isPasswordCorrect)
            {
                return Unauthorized(
          ApiResponse<string>.FailureResponse(
              "Invalid credentials"));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email!,
                roles);

            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                Roles = roles
            };

            return Ok(
                ApiResponse<AuthResponseDto>.SuccessResponse(
                    response,
                    "Login successful"));
        }

    }
}
