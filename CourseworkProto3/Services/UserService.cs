using System;
using System.Collections.Generic;
using System.Text;
using Library.Models.Entities;
using Library.Data.Repo;
using Library.Models.DTO;
using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly LibraryContext _context;
        public UserService(UserRepository userRepository, LibraryContext libraryContext)
        {
            _userRepository = userRepository;
            _context = libraryContext;
        }

        public async Task<User> Register(RegisterUserRequest request)
        {
            var user = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                Gender = request.Gender
            };
            await _userRepository.AddUser(user);
            return user;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetUser(email, password);
            if (user == null)
                throw new Exception("Wrong email or password");
            return JwtService.GenerateToken(user);
        }
        
        
        public bool IsSignedIn(string token){
            var user = _userRepository.GetUserByToken(token);
            return user != null;
        }
    }
}
