using Data;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public sealed class UserRepository(
    AppDbContext appDbContext
)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<User> CreateAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await _appDbContext.Users.FindAsync(id);
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task UpdateAsync(User user)
    {
        _appDbContext.Users.Update(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _appDbContext.Users.Remove(user);
        await _appDbContext.SaveChangesAsync();
    }
}
