using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessObjects.DTOs.WorkTask.Request
{
    public class TaskAddRequestModel
    {
        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public string Priority { get; set; } = null!;

        public DateTime? DueDate { get; set; }
    }
}
