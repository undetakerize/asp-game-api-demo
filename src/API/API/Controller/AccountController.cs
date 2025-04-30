using GameService.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Account_LoginAccountDto = GameService.Application.Common.Account.LoginAccountDto;
using Account_RegisterAccountDto = GameService.Application.Common.Account.RegisterAccountDto;
using LoginAccountDto = GameService.Application.Common.Account.LoginAccountDto;
using RegisterAccountDto = GameService.Application.Common.Account.RegisterAccountDto;

namespace GameService.API.Controller;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Account_LoginAccountDto dto)
    {
        if(!ModelState.IsValid) return BadRequest();
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if(user == null) return Unauthorized("Invalid username!");
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if(!result.Succeeded) return BadRequest("Invalid username or password!");
        return Ok(new
        {
            Token = _tokenService.GenerateToken(user)
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Account_RegisterAccountDto dto)
    {
        try
        {
            if(!ModelState.IsValid) return BadRequest();
            var appUser = new AppUser
            {
                Email = dto.Email,
                UserName = dto.Username,
            };
            if(dto.Password == null) return BadRequest("Password is required");
            var createUser = await _userManager.CreateAsync(appUser, dto.Password);
            if (!createUser.Succeeded) return StatusCode(500, createUser.Errors);
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            return roleResult.Succeeded ? Ok(new
                {
                    username = appUser.UserName,
                    email = appUser.Email,
                    token = _tokenService.GenerateToken(appUser)
                })
             : StatusCode(500, roleResult.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}