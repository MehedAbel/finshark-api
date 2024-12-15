using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepo.GetAllAsync();
            var commentsDTO = comments.Select(c => c.ToCommentDTO());
            return Ok(commentsDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null) {
                return NotFound();
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDTO commentDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            if (!await _stockRepo.StockExists(stockId)) {
                return BadRequest("Stock does not exist.");
            }

            var commentModel = commentDTO.ToCommentFromCreateDTO(stockId);
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, commentModel.ToCommentDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDTO updateDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var comment = await _commentRepo.UpdateAsync(id, updateDTO.ToCommentFromUpdateDTO());

            if (comment == null) {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var commentModel = await _commentRepo.DeleteAsync(id);

            if (commentModel == null) {
                return NotFound();
            }

            return Ok(commentModel);
        }

    }
}