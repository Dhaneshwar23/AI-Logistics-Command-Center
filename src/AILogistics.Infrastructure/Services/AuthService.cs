using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using AILogistics.Infrastructure.Authentication;
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
        private readonly JwtOptions _jwtOptions;

        public AuthService(IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            IOptions<JwtOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtOptions = jwtOptions.Value;
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

                string generatedRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                RefreshToken refreshToken = CreateRefreshToken(user, generatedRefreshToken);

                await _userRepository.AddRefreshTokenAsync(refreshToken);
                await _userRepository.SaveChangesAsync();

                LoginResponseDto response = new LoginResponseDto
                {
                    Token = jwtToken.Token,
                    ExpiresAt = jwtToken.ExpiresAt,
                    RefreshToken = generatedRefreshToken,
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

        private RefreshToken CreateRefreshToken(User user, string generatedRefreshToken)
        {
            RefreshToken rfToken = new RefreshToken
            {
                Token = generatedRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays),
                UserId = user.Id,
                RevokedAt = null,
                ReplacedByToken = null,
                CreatedAt = DateTime.UtcNow
            };

            return rfToken;
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            RefreshToken? refreshToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);

            if (refreshToken == null)
            {
                throw new AuthenticationException("Invalid refresh token.");
            }
            else if (refreshToken.IsExpired)
            {
                throw new AuthenticationException($"Refresh token has expired.");
            }
            else if (refreshToken.IsRevoked)
            {
                throw new AuthenticationException($"Refresh token has been revoked. ");
            }
            else
            {
                User user = refreshToken.User;

                string newGeneratedRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                refreshToken.RevokedAt = DateTime.UtcNow;
                refreshToken.ReplacedByToken = newGeneratedRefreshToken;

                JwtTokenResultDto jwtToken = _jwtTokenGenerator.GenerateToken(user);

                RefreshToken newRefreshToken = CreateRefreshToken(user, newGeneratedRefreshToken);

                await _userRepository.AddRefreshTokenAsync(newRefreshToken);
                await _userRepository.SaveChangesAsync();

                LoginResponseDto response = new LoginResponseDto
                {
                    Token = jwtToken.Token,
                    ExpiresAt = jwtToken.ExpiresAt,
                    RefreshToken = newGeneratedRefreshToken,
                    UserId = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role,
                };

                return response;
            }
        }

        public async Task LogoutAsync(RefreshTokenRequestDto request)
        {
            RefreshToken? refreshToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);
            if (refreshToken == null)
            {
                throw new AuthenticationException("Invalid refresh token. ");
            }
            else if(refreshToken.IsExpired)
            {
                throw new AuthenticationException("Refresh token has expired. ");
            }
            else if (refreshToken.IsRevoked)
            {
                throw new AuthenticationException("Refresh token has already been revoked. ");
            }
            else
            {
                refreshToken.RevokedAt = DateTime.UtcNow;
                await _userRepository.SaveChangesAsync();
            }

        }
    }
}
