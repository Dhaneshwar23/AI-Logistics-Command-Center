using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs.Authentication
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
