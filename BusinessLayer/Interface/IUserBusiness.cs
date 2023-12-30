using ModelLayer;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface IUserBusiness
    {
        UserEntity UserRegistration(RegisterModel registerModel);
        string UserLogin(LoginModel loginModel);
        bool Resetpassword(string email, string password, string ConfirmPasswor);
        bool CheckEmail(string email);
        bool CheckPassword(string password);

        ForgetPasswordModel ForgetPassword(String email);
        //string GenerateToken(int UserId, string userEmail);

        IEnumerable<UserEntity> GetByname(string name);
        List<UserEntity> GetAllusers();
        UserEntity UserLoginForSession(string email, string password);
        int GetCount(int userId);
        int GetColaboratorCount(int userId);
        int GetLableCount(int userId);
    }
}