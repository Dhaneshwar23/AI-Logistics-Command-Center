using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.Authentication
{
    public class RegisterRequestDto
    {
        public string FullName {  get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; }

    }
}
