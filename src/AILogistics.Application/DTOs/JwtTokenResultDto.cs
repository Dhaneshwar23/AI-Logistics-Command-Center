using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.DTOs
{
    public class JwtTokenResultDto
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
