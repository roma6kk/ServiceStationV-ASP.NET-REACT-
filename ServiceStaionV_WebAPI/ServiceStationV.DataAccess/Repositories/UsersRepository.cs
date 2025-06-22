using Microsoft.EntityFrameworkCore;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private ServiceStationVDBContext _context;
        public UsersRepository(ServiceStationVDBContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber
            };
            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByPhone(string phone)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Where(u => u.PhoneNumber == phone)
                .Select(u => new { u.Id, u.UserName, u.Email, u.PhoneNumber, u.PasswordHash })
                .FirstOrDefaultAsync();

            if (userEntity == null)
            {
                throw new ArgumentException($"User with phone number '{phone}' not found.");
            }

            return User.Create(userEntity.Id, userEntity.UserName, userEntity.Email, userEntity.PhoneNumber, userEntity.PasswordHash);
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Where(u => u.PhoneNumber == email)
                .Select(u => new { u.Id, u.UserName, u.Email, u.PhoneNumber, u.PasswordHash })
                .FirstOrDefaultAsync();

            if (userEntity == null)
            {
                throw new ArgumentException($"User with email '{email}' not found.");
            }

            return User.Create(userEntity.Id, userEntity.UserName, userEntity.Email, userEntity.PhoneNumber, userEntity.PasswordHash);
        }

        public async Task<bool> ExistedEmailOrPhone(string email, string phoneNuber)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .Where(u => u.PhoneNumber == phoneNuber || u.Email == email)
                .Select(u => new { u.Id, u.UserName, u.Email, u.PhoneNumber, u.PasswordHash })
                .FirstOrDefaultAsync();

            if (userEntity == null)
            {
                return false;
            }

            return true;
        }

        public async Task<User?> GetById(Guid id)
        {
            var userEntity =  await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
            if ((userEntity == null))
            {
                return null;
            }
            return User.Create(userEntity.Id, userEntity.UserName, userEntity.Email, userEntity.PhoneNumber, userEntity.PasswordHash);
        }
    }
}
