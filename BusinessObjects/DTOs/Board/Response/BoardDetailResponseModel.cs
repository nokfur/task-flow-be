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
    public class BoardDetailResponseModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<ColumnDetailResponseModel> Columns { get; set; } = new List<ColumnDetailResponseModel>();

        public ICollection<LabelDetailResponseModel> Labels { get; set; } = new List<LabelDetailResponseModel>();

        public ICollection<UserProfileResponseModel> Members { get; set; } = new List<UserProfileResponseModel>();
    }
}
