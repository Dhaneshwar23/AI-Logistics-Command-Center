using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            string userEmail = request.Email.Trim().ToLowerInvariant();

            User? user = await _userRepository.GetByEmailAsync(userEmail);
            if (user == null)
            {
                throw new AuthenticationException("Invalid email or password.");
            }
            else
            {
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    throw new AuthenticationException("Invalid email or password.");
                }
                else if (result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

                    await _userRepository.UpdateAsync(user);
                }

                JwtTokenResultDto jwtToken = _jwtTokenGenerator.GenerateToken(user);

                LoginResponseDto response = new LoginResponseDto
                {
                    Token = jwtToken.Token,
                    ExpiresAt = jwtToken.ExpiresAt,
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role
                };

                return response;

            }

        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            bool emailExists = await _userRepository.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                throw new ConflictException("User exists");
            }
            else
            {

                User user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email.Trim().ToLowerInvariant(),
                    Role = UserRole.Customer,
                    CreatedAt = DateTime.UtcNow,

                };
                string? hashedPassword = _passwordHasher.HashPassword(user, request.Password);

                user.PasswordHash = hashedPassword;

                await _userRepository.AddAsync(user);
            }
        }
    }
}
