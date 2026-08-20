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
        [Required, StringLength(200, MinimumLength = 2)]
        public string FullName {  get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(128, MinimumLength = 12)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$",
            ErrorMessage = "Password must contain upper-case, lower-case, numeric, and special characters.")]
        public string Password { get; set; }

        public int CustomerId { get; set; }

    }
}
