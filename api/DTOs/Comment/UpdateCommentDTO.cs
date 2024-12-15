using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Comment
{
    public class UpdateCommentDTO
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters long")]
        [MaxLength(280, ErrorMessage = "Title must be at most 280 characters long")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(2, ErrorMessage = "Content must be at least 2 characters long")]
        [MaxLength(500, ErrorMessage = "Content must be at most 500 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}