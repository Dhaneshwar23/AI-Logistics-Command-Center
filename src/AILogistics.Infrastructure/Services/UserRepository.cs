using AILogistics.Application.Exceptions;
using AILogistics.Application.Interfaces;
using AILogistics.Domain.Entities;
using AILogistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILogistics.Infrastructure.Services
{

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            User? existingUser = await _context.Users.FirstOrDefaultAsync(u=> u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.PasswordHash = user.PasswordHash;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException("User not found");
            }
        }
    }
}
