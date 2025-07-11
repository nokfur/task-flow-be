using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Label.Request;

namespace BusinessObjects.DTOs.WorkTask.Request
{
    public class TaskTemplateAddRequestModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public int Position { get; set; }

        [Required]
        public string Priority { get; set; } = null!;

        public ICollection<LabelTemplateAddRequestModel> Labels { get; set; } = new List<LabelTemplateAddRequestModel>();
    }
}
