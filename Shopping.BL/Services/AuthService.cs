using Microsoft.AspNetCore.Identity;
using Shopping.DAL;
using Shopping.Interfaces;
using Shopping.Shared;
using System.Net;

namespace Shopping.BL;

public class AuthService
    (UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService) 
    : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<ResultDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
                return (-1).ToOperationResult(failureMessage:"Email already exists!");

            User user = new()
            {
                UserName = registerDto.Username,
                FullName = registerDto.FullName,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (-1).ToOperationResult(failureMessage: errors);
            }
            if (!await _roleManager.RoleExistsAsync(Role.Customer.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Role.Customer.ToString()));
            }

            await _userManager.AddToRoleAsync(user, Role.Customer.ToString());

            return true.ToOperationResult("User created successfully!");
        }
        catch (Exception)
        {
            return false.ToOperationResult(HttpStatusCode.InternalServerError, "An error occurred during registration.");
            //throw;
        }
    }
    public async Task<ResultDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return false.ToOperationResult(HttpStatusCode.Unauthorized,"Invalid username or password!");
                
            var token = _tokenService.GenerateJwtToken(user);

            return true.ToOperationResult(HttpStatusCode.OK, data:token);

        }
        catch (Exception ex)
        {
            return false.ToOperationResult(HttpStatusCode.InternalServerError, "An error occurred during login.");
            //throw;
        }
    }
}

