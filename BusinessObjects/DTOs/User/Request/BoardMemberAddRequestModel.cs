using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class BoardMemberAddRequestModel
    {
        [Required]
        public string BoardId { get; set; } = null!;

        [Required]
        public string MemberId { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;
    }
}
