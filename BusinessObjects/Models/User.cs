using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class User
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Board> Boards { get; set; } = new List<Board>();

        public virtual ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();

    }
}
