using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.User.Request;

namespace BusinessObjects.DTOs.Board.Request
{
    public class BoardAddRequestModel
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? TemplateId { get; set; }

        public ICollection<MemberAddRequestModal> BoardMembers { get; set; } = new List<MemberAddRequestModal>();
    }
}
