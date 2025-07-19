using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Column.Response;

namespace BusinessObjects.DTOs.Board.Response
{
    public class BoardTemplateResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<string> Columns { get; set; } = new List<string>();
    }
}
