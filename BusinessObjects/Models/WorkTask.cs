using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class WorkTask
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int Position { get; set; }

        public string Priority { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ColumnId { get; set; } = null!;

        public virtual Column Column {  get; set; } = null!;

        public virtual ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();

        public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
    }
}
