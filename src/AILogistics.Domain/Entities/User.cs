using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Domain.Entities
{
    public enum UserRole
    {
        Admin = 0,
        Manager = 1,
        Customer = 2
    }
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }   
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
