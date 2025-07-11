using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Label
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string BoardId { get; set; } = null!;

        public virtual Board Board { get; set; } = null!;

        public virtual ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();

        public virtual ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
