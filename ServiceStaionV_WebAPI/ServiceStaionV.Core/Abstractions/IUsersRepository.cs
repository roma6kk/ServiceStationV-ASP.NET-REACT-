﻿using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface IUsersRepository
    {
        public  Task Add(User user);
        public  Task<bool> ExistedEmailOrPhone(string email, string phone);
        public Task<User> GetByEmail(string email);
        public Task<User> GetByPhone(string phone);
        public Task<User?> GetById(Guid id);

    }
}