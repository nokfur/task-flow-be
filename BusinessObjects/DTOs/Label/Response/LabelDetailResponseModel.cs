using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Label.Response
{
    public class LabelDetailResponseModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Color { get; set; } = null!;
    }
}
