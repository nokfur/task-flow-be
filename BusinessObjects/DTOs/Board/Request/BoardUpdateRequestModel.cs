using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Board.Request
{
    public class BoardUpdateRequestModel
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}
