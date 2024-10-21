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

        public async Task<User> Register(string login, string password, Gender gender)
        {
            var user = new User
            {
                Login = login,
                Password = password,
                AccessRights = /*(AccessRights)Enum.Parse(typeof(AccessRights), accessRights)*/ Roles.Default,
                Gender = gender
            };
            await _userRepository.AddUser(user);
            return user;
        }

        public async Task<string> Login(string login, string password)
        {
            var user = await _userRepository.GetUser(login, password);
            if (user == null)
                throw new Exception("Wrong login or password");
            return JwtService.GenerateToken(user);
        }
        
        public bool IsSignedIn(string token){
            var user = _userRepository.GetUserByToken(token);
            return user != null;
        }
    }
}
