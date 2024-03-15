using Claim.BindingMode;
using Claim.Data.Entities;
using DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApiAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("RegisterUser")]
    public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
    {
        try
        {
            var user = new AppUser() { FullName = model.FullName, UserName = model.Email, Email = model.Email, DateCreated = DateTime.UtcNow, DateModified = DateTime.UtcNow };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return await Task.FromResult("User has been added");
            }
            return await Task.FromResult(string.Join(',', result.Errors.Select(x => x.Description).ToArray()));
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ex.Message);
        }

    }
    [HttpGet("GetAllUsers")]
    public async Task<object> GetAllUsers()
    {
        try
        {
            var getUsers = _userManager.Users.Select(x => new UserDTO(x.FullName, x.Email, x.UserName, x.DateCreated));
            return await Task.FromResult(getUsers);
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<object> LoginUser([FromBody])
    {
        return Ok();
    }

}
