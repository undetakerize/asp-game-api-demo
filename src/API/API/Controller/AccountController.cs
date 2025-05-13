using GameService.Application.Common;
using GameService.Application.Features.Account.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Account_LoginAccountDto = GameService.Application.Common.Account.LoginAccountDto;
using Account_RegisterAccountDto = GameService.Application.Common.Account.RegisterAccountDto;

namespace GameService.API.Controller;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Account_LoginAccountDto dto)
    {
        if(!ModelState.IsValid) return BadRequest();
        var result = await _mediator.Send(new CommandLoginAccount(dto.Email, dto.Password));
        return result.ToHttpResult();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Account_RegisterAccountDto dto)
    {
        var result = await _mediator.Send(new CommandRegisterAccount(dto.Username, dto.Password, dto.Email));
        return result.ToHttpResult();
    }
}