using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock-user")]

public class StockUserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManger;
    private readonly IStockRepository _stockRepository;
    private readonly IStockUserRepository _stockUserRepository;
    public StockUserController(UserManager<AppUser> userManger, IStockRepository stockRepository, IStockUserRepository stockUserRepository)
    {
        _userManger = userManger;
        _stockRepository = stockRepository;
        _stockUserRepository = stockUserRepository;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetStockUser()
    {
        var username = User.GetUserName();
        var appUser = await _userManger.FindByNameAsync(username);
        var stockUser = await _stockUserRepository.GetStockUser(appUser);
        return Ok(stockUser);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateStockUser(string symbol)
    {
        var username = User.GetUserName();
        var appUser = await _userManger.FindByNameAsync(username);

        var stock = await _stockRepository.GetBySymbolAsync(symbol);
        if (stock == null) return BadRequest("Stock Not Found");

        var stockuser = await _stockUserRepository.GetStockUser(appUser);
        if (stockuser.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Stock Already Exists.");

        var stockUserModel = new StockUser
        {
            StockId = stock.Id,
            AppUserId = appUser.Id

        };
        await _stockUserRepository.CreateAsync(stockUserModel);
        if (stockUserModel == null)
        {
            return StatusCode(500, "Couldn't create stockuser model");
        }
        else
        {
            return Created();
        }
    }
}
