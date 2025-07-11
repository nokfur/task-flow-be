using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class TaskLabel
    {
        public string TaskId { get; set; } = null!;

        public string LabelId { get; set; } = null!;

        public virtual WorkTask Task { get; set; } = null!;

        public virtual Label Label { get; set; } = null!;
    }
}
