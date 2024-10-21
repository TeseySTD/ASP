using System;
using System.Collections.Generic;
using System.Text;
using Library.Models.Entities;
using Library.Data.Repo;

namespace Library.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Register(string login, string email, string password, Gender gender)
        {
            var user = new User
            {
                Login = login,
                Email = email,
                Password = password,
                Role = /*(AccessRights)Enum.Parse(typeof(AccessRights), accessRights)*/ Roles.Default,
                Gender = gender
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
