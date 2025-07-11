using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class BoardMember
    {
        public string BoardId { get; set; } = null!;

        public string MemberId { get; set; } = null!;

        public string Role {  get; set; } = null!;

        public virtual Board Board { get; set; } = null!;

        public virtual User Member { get; set; } = null!;
    }
}
