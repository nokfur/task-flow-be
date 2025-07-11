using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public partial class Board
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string OwnerId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsTemplate { get; set; }

        public virtual ICollection<Column> Columns { get; set; } = new List<Column>();

        public virtual ICollection<Label> Labels { get; set; } = new List<Label>();

        public virtual ICollection<User> Members { get; set; } = new List<User>();

        public virtual ICollection<BoardMember> BoardMembers { get; set; } = new List<BoardMember>();

    }
}
