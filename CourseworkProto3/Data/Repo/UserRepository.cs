using System;
using System.Collections.Generic;
using System.Text;
using Library.Data;
using Library.Models.Entities;
using Library.Services;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repo
{
    public class UserRepository
    {
        private readonly LibraryContext _context;
        public UserRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string login, string password)
        {
            var user = await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            return user;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id){
            return await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return RolePermissions.Permissions[user.AccessRights];
        }

        public User GetUserByToken(string token){
            var id = JwtService.GetUserIdFromToken(token);
            var user = _context.Users
                            .AsNoTracking()
                            .FirstOrDefault(u => u.UserId == id);
            return user;
        }

        public bool IsLoginTaken(string login){
            return _context.Users.Any(u => u.Login == login);
        }

        public bool IsPasswordCorrect(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            return user != null;
        }
    }
}
