using AILogistics.Application.DTOs;
using AILogistics.Application.DTOs.Authentication;
using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _passwordHasherMock.Object,
                _jwtTokenGeneratorMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ThrowsAuthenticationException()
        {
            //Arrange
            var request = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "test@gmail.com",
                Password = "Password321!",

            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("test@gmail.com"))
                .ReturnsAsync(true);

            //Act
            Func<Task> act = async () => await _authService.RegisterAsync(request);

            //Assert
            await act.Should().ThrowAsync<ConflictException>();
        }

        [Fact]
        public async Task RegisterAsync_ValidRequest_CreatesUser()
        {
            var request = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "test@gmail.com",
                Password = "Password321!",

            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("test@gmail.com"))
                .ReturnsAsync(false);

            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<User>(), request.Password))
                .Returns("hashed-password");

            await _authService.RegisterAsync(request);

            _userRepositoryMock
               .Verify(x => x.AddAsync(It.IsAny<User>()),
               Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ValidRequest_HashesPassword()
        {
            var request = new RegisterRequestDto
            {
                FullName = "Test User",
                Email = "test@gmail.com",
                Password = "Password321!",
            };

            _userRepositoryMock
                .Setup(x => x.EmailExistsAsync("test@gmail.com"))
                .ReturnsAsync(false);

            _passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<User>(), request.Password))
                .Returns("hashed-password");

            await _authService.RegisterAsync(request);

            _passwordHasherMock
                .Verify(x => x.HashPassword(It.IsAny<User>(), request.Password),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_InvalidEmail_ThrowsAuthenticationException()
        {
            var request = new LoginRequestDto
            {
                Email = "test@gmail.com",
                Password = "Password321!",
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email.Trim().ToLowerInvariant()))
                .ReturnsAsync((User?)null);

            Func<Task> act = async () => await _authService.LoginAsync(request);

            await act.Should().ThrowAsync<AuthenticationException>();
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ThrowsAuthenticationException()
        {
            var request = new LoginRequestDto
            {
                Email = "test@gmail.com",
                Password = "Password321!",
            };

            var user = new User
            {
                Id = 1,
                FullName = "Test User",
                Email = "test@gmail.com",
                PasswordHash = "hashed-password",
                Role = UserRole.Customer
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email.Trim().ToLowerInvariant()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyHashedPassword(It.IsAny<User>(),
                "hashed-password",
                request.Password))
                .Returns(PasswordVerificationResult.Failed);

            Func<Task> act = async () => await _authService.LoginAsync(request);

            await act.Should().ThrowAsync<AuthenticationException>();
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
        {
            var request = new LoginRequestDto
            {
                Email = "test@gmail.com",
                Password = "Password321!",
            };

            var user = new User
            {
                Id = 1,
                FullName = "Test User",
                Email = "test@gmail.com",
                PasswordHash = "hashed-password",
                Role = UserRole.Customer
            };

            var jwtToken = new JwtTokenResultDto
            {
                Token = "Token",
                ExpiresAt = DateTime.UtcNow,
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email.Trim().ToLowerInvariant()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyHashedPassword(user,
                "hashed-password"
                , request.Password))
                .Returns(PasswordVerificationResult.Success);

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(jwtToken);

            LoginResponseDto res = await _authService.LoginAsync(request);

            res.Should().NotBeNull();

            res.Token.Should().Be("Token");
            res.UserId.Should().Be(1);
            res.Email.Should().Be("test@gmail.com");
            res.FullName.Should().Be("Test User");
            res.Role.Should().Be(UserRole.Customer);

        }

        [Fact]
        public async Task LoginAsync_SuccessRehashNeeded_UpdatesPasswordHashAndReturnsLoginResponse()
        {
            var request = new LoginRequestDto
            {
                Email = "test@gmail.com",
                Password = "Password321!",
            };

            var user = new User
            {
                Id = 1,
                FullName = "Test User",
                Email = "test@gmail.com",
                PasswordHash = "hashed-password",
                Role = UserRole.Customer
            };

            var jwtToken = new JwtTokenResultDto
            {
                Token = "Token",
                ExpiresAt = DateTime.UtcNow,
            };

            _userRepositoryMock
                .Setup(x => x.GetByEmailAsync(request.Email.Trim().ToLowerInvariant()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(p => p.VerifyHashedPassword(user,
                "hashed-password",
                request.Password))
                .Returns(PasswordVerificationResult.SuccessRehashNeeded);

            _passwordHasherMock
                .Setup(u => u.HashPassword(user, request.Password))
                .Returns("Rehashed-password");

            _jwtTokenGeneratorMock
                .Setup(j => j.GenerateToken(user))
                .Returns(jwtToken);
            var res = await _authService.LoginAsync(request);

            _userRepositoryMock
                .Verify( x => x.UpdateAsync(It.Is<User>(u => u.PasswordHash == "Rehashed-password"))
                ,Times.Once());

            _jwtTokenGeneratorMock
                .Verify( j => j.GenerateToken(user),
                Times.Once());

            res.Should().NotBeNull();

            res.Token.Should().Be("Token");
            res.UserId.Should().Be(1);
            res.Email.Should().Be("test@gmail.com");
            res.FullName.Should().Be("Test User");
            res.Role.Should().Be(UserRole.Customer);
        }
    }
}
