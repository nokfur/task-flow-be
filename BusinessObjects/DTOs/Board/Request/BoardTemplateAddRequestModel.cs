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
    public class BoardTemplateAddRequestModel
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<ColumnTemplateAddRequestModel> Columns { get; set; } = new List<ColumnTemplateAddRequestModel>();

        public ICollection<LabelTemplateAddRequestModel> Labels { get; set; } = new List<LabelTemplateAddRequestModel>();
    }
}
