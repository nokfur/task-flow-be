using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.WorkTask.Request;

namespace BusinessObjects.DTOs.Column.Request
{
    public class ColumnTemplateAddRequest
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public int Position { get; set; }

        public ICollection<TaskTemplateAddRequest> Tasks { get; set; } = new List<TaskTemplateAddRequest>();
    }
}
