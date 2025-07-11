using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.WorkTask.Request
{
    public class TaskReorderRequestModel
    {
        [Required]
        public string TaskId { get; set; } = null!;

        [Required]
        public string SourceColumnId { get; set; } = null!;

        [Required]
        public string TargetColumnId { get; set; } = null!;

        [Required]
        public int TargetIndex { get; set; }
    }
}
