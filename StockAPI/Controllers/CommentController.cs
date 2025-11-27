using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comments")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var comments = await _commentRepository.GetAllAsync();
        var commentDto = comments.Select(comment => comment.ToCommentDto());
        return Ok(commentDto);
    }
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }
    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> CreateAsync([FromRoute] int stockId, [FromBody] CreateCommentDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock does not exists.");
        }
        var commentModel = createDto.ToCommentFromCreateDto(stockId);
        await _commentRepository.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var commentModel = await _commentRepository.UpdateAsync(id, updateDto);
        if (commentModel == null)
        {
            return NotFound();
        }
        return Ok(commentModel.ToCommentDto());
    }
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var commentModel = await _commentRepository.DeleteAsync(id);
        if (commentModel == null)
        {
            return NotFound();
        }
        return NoContent();
    }
}
