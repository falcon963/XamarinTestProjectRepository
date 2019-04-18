﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core.Models;

namespace TestProject.Core.Repositorys.Interfaces
{
    public interface ILoginRepository
        : IBaseRepository<User>
    {
        void SetLoginAndPassword(string login, string password);
        User CheckAccountAccess(string login, string password);
        bool CheckValidLogin(string login);
        int GetSocialAccount(User user);
        void ChangePassword(int userId, string password);
        void ChangeImage(int userId, string imagePath);
    }
}
