using System.Security.Claims;
using API.Controllers.Users.InputModels;
using API.Controllers.Users.ViewModels;
using API.Services;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Users
{
  [AllowAnonymous]
  [ApiController]
  [Route("api/[controller]")]

  public class AccountController : ControllerBase
  {
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    public AccountController(
        UserManager<User> userManager,
        TokenService tokenService
    )
    {
      _userManager = userManager;
      _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserViewModel>> Login(LoginUserInputModel input)
    {
      var user = await _userManager.FindByEmailAsync(input.Email);

      if (user == null) return Unauthorized();

      var result = await _userManager.CheckPasswordAsync(user, input.Password);

      if (!result) return Unauthorized();

      return new UserViewModel
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user),
      };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserViewModel>> Register(RegisterUserInputModel input)
    {
      if (await _userManager.Users.AnyAsync(user => user.UserName == input.Username))
      {
        return BadRequest("Username is already taken");
      }

      if (await _userManager.Users.AnyAsync(user => user.Email == input.Email))
      {
        return BadRequest("Email is already taken");
      }

      var user = input.ToUserEntity();

      var result = await _userManager.CreateAsync(user, input.Password);

      if (!result.Succeeded) return BadRequest(result.Errors);

      return new UserViewModel
      {
        Username = input.Username,
        Token = _tokenService.CreateToken(user)
      };
    }

    [HttpGet]
    public async Task<ActionResult<UserViewModel>> GetCurrentUser()
    {
      var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

      return new UserViewModel
      {
        Username = user.UserName,
        Token = _tokenService.CreateToken(user)
      };

    }
  }
}