using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Repositories;
using ServiceStationV.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;
        public UsersService(IUsersRepository usersRepository,
                            IPasswordHasher passwordHasher,
                            IJwtProvider jwtProvider)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<User?> Register(string username, string email, string phone, string password)
        {
            if (await _usersRepository.ExistedEmailOrPhone(email, phone))
                throw new InvalidOperationException("Пользователь с таким email или телефоном уже существует.");
            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(Guid.NewGuid(), username, email, phone, hashedPassword);

            await _usersRepository.Add(user);

            return user;
        }

        public async Task<string> Login(string phoneNumber, string password)
        {
            var user = await _usersRepository.GetByPhone(phoneNumber);

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
    }
}
