using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.WorkTask.Response;
using BusinessObjects.Models;

namespace BusinessObjects.DTOs.Column.Response
{
    public class ColumnDetailResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int Position { get; set; }

        public ICollection<TaskDetailResponse> Tasks { get; set; } = new List<TaskDetailResponse>();
    }
}
