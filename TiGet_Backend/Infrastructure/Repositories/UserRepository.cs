using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await dbContext.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Func<User, bool> predicate)
        {
            return await dbContext.Users.AnyAsync(u => predicate(u));
        }

    }
}
