using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Label.Response;
using BusinessObjects.Models;

namespace BusinessObjects.DTOs.WorkTask.Response
{
    public class TaskDetailResponseModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Priority { get; set; } = null!;

        public int Position { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<LabelDetailResponseModel> Labels { get; set; } = new List<LabelDetailResponseModel>();
    }
}
