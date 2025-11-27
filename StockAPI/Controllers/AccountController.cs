using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Controllers;

[Route("/api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManger;
    private readonly ITokenService _tokenSerive;
    private readonly SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManger, ITokenService tokenSerive, SignInManager<AppUser> signInManager)
    {
        _userManger = userManger;
        _tokenSerive = tokenSerive;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email
            };
            var createdUser = await _userManger.CreateAsync(appUser, registerDto.Password);
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManger.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                    return Ok(
                        new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = _tokenSerive.CreateToken(appUser)
                        }
                    );
                else
                    return StatusCode(500, roleResult.Errors);
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _userManger.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.UserName.ToLower());
        if (user == null) return Unauthorized("Invalid UserName or password!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid username or password!");
        return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        Token = _tokenSerive.CreateToken(user)
                    }
                );
    }

}
