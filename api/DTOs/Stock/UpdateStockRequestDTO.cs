using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDTO
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        [MaxLength(35, ErrorMessage = "Company name cannot exceed 35 characters")]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(16, ErrorMessage = "Industry cannot exceed 16 characters")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, 1000000000000)]
        public long MarketCap { get; set; }
    }
}