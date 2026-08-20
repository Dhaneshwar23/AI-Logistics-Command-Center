using AILogistics.Application.Interfaces;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AILogistics.Infrastructure.Services
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return int.TryParse(value, out var userId) ? userId : null;
            }
        }

        public int? Role
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue(ClaimTypes.Role);

                return int.TryParse(value, out var role) ? role : null;
            }
        }

        public int? CustomerId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue("customerId");

                return int.TryParse(value, out var customerId) ? customerId : null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?
                                        .User?
                                        .Identity?
                                        .IsAuthenticated == true;
    }
}
