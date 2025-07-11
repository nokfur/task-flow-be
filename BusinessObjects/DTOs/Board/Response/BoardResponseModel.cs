using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.DTOs.User.Response;

namespace BusinessObjects.DTOs.Board.Response
{
    public class BoardResponseModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ColumnCount { get; set; }

        public int TaskCount { get; set; }

        public int LabelCount { get; set; }

        public bool isOwn { get; set; }

        public ICollection<UserProfileResponseModel> Members { get; set; } = new List<UserProfileResponseModel>();
    }
}
