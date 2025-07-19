using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.User.Request
{
    public class UserChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; } = null!;

        [Required] 
        public string NewPassword { get;set; } = null!;
    }
}
