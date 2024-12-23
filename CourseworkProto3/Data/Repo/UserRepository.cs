using System;
using System.Collections.Generic;
using System.Text;
using Library.Data;
using Library.Models.DTO;
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

        public async Task<User> GetUser(string email, string password)
        {
            var user = await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task UpdateUser(EditUserRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == request.UserId);
            if (user != null)
            {
                user.Login = request.Login;
                user.Password = request.Password;
                user.Email = request.Email;
                user.Role = request.Role;
                user.Gender = request.Gender;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUser(int id)    
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<List<Permission>> GetUserPermissionsAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            return RolePermissions.Permissions[user.Role];
        }

        public User GetUserByToken(string token)
        {
            var id = JwtService.GetUserIdFromToken(token);
            var user = _context.Users
                            .AsNoTracking()
                            .FirstOrDefault(u => u.UserId == id);
            return user;
        }

        public User GetOwnerOfLibrary()
        {
            return _context.Users.FirstOrDefault(u => u.Role == Roles.Owner);
        }

        
        public async Task<List<Borrow>> GetBorrowersWithBorrows(){
            return await _context.Borrows
                                    .Include(b => b.Borrower)
                                    .AsNoTracking()
                                    .ToListAsync();
            
        }

        public async Task<List<Borrow>> GetLendersWithBorrows(){
            return await _context.Borrows
                                    .Include(b => b.Lender)
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<List<Borrow>> GetDebtorsWithBorrows(){
            return await _context.Borrows
                        .Include(b => b.Borrower)
                        .Where(b => b.BorrowEndDate < DateTime.Now)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public bool IsLoginTaken(string login)
        {
            return _context.Users.Any(u => u.Login == login);
        }

        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public Task<bool> IsEmailTakenExceptUser(string email, int userId)
        {
            return _context.Users.AnyAsync(u => u.Email == email && u.UserId != userId);
        }

        public bool IsPasswordCorrect(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }

        public bool IsUser(int id)
        {
            return _context.Users.Any(u => u.UserId == id);
        }

        
    }
}
