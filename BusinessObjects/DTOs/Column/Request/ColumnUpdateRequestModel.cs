﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Column.Request
{
    public class ColumnUpdateRequestModel
    {
        [Required]
        public string Title { get; set; } = null!;
    }
}
