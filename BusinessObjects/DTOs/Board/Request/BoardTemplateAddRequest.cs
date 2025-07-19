using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Column.Request;
using BusinessObjects.DTOs.Label.Request;

namespace BusinessObjects.DTOs.Board.Request
{
    public class BoardTemplateAddRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<ColumnTemplateAddRequest> Columns { get; set; } = new List<ColumnTemplateAddRequest>();

        public ICollection<LabelTemplateAddRequest> Labels { get; set; } = new List<LabelTemplateAddRequest>();
    }
}
