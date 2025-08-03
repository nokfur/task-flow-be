using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.Column.Response;
using BusinessObjects.DTOs.Label.Response;
using BusinessObjects.DTOs.User.Response;

namespace BusinessObjects.DTOs.Board.Response
{
    public class BoardDetailResponse
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UserRole { get; set; } = null!;

        public ICollection<ColumnDetailResponse> Columns { get; set; } = new List<ColumnDetailResponse>();

        public ICollection<LabelDetailResponse> Labels { get; set; } = new List<LabelDetailResponse>();

        public ICollection<UserProfileResponse> Members { get; set; } = new List<UserProfileResponse>();
    }
}
