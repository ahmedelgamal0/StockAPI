using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;
// This attribute sets the URL for this controller.
// All methods in this class will be prefixed with "https"//[your-domain]/api/stock"
[Route("api/stock")]

// This enables several helpful API-specific features, like automatic
// model binding and error handling.
[ApiController]
public class StockController : ControllerBase
{
    // A private, "read-only" field to hold our database context.
    // "readonly" means it can only be set in the constructor.
    private readonly IStockRepository _stockRepository;
    // This is the controller's "Constructor".
    // It uses "Dependency Injection" to "ask for" the database context
    // (ApplicationDBContext) from the .NET service container.
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] StockQueryObject queryObject)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        // 1. Go to the database (via the context).
        // 2. Access the "Stocks" table (DbSet).
        // 3. Execute the query and get all stocks as a List.
        var stocks = await _stockRepository.GetAllAsync(queryObject);
        var stockDto = stocks.Select(s => s.ToStockDto());

        // 4. Return an "HTTP 200 OK" response,
        //    with the list of stocks in the response body (as JSON).
        return Ok(stocks);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var stock = await _stockRepository.GetByIdAsync(id);
        if (stock == null)
        {
            return NotFound();
        }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var stockModel = stockDto.ToStockFromCreateDto();
        await _stockRepository.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var stockModel = await _stockRepository.UpdateAsync(id, updateDto);
        if (stockModel == null)
        {
            return NotFound();
        }
        return Ok(stockModel.ToStockDto());
    }
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var stockModel = await _stockRepository.DeleteAsync(id);
        if (stockModel == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}

