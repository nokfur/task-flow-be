using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Column
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public int Position { get; set; }

        public string BoardId { get; set; } = null!;

        public virtual Board Board { get; set; } = null!;

        public virtual ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
    }
}
