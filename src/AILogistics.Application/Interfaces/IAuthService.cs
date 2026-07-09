using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task LogoutAsync(RefreshTokenRequestDto request);

    }
}
