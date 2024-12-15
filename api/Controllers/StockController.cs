using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using api.Mappers;
using api.DTOs.Stock;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Helpers;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var stocks = await _stockRepo.GetAllAsync(query);
            var stocksDTO = stocks.Select(s => s.ToStockDTO());

            return Ok(stocksDTO);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null) {
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDTO());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var stockModel = await _stockRepo.UpdateAsync(id, updateDTO.ToStockFromUpdateDTO());

            if (stockModel == null) {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var stockModel = await _stockRepo.DeleteAsync(id);

            if (stockModel == null) {
                return NotFound();
            }

            return NoContent();
        }
    }
}