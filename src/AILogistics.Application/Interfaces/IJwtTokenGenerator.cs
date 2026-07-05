using AILogistics.Application.DTOs;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        JwtTokenResultDto GenerateToken(User user);
    }
}
