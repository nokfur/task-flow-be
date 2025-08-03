using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Label.Request
{
    public class LabelAddRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;
    }
}
