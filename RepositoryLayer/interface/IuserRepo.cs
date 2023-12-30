using ModelLayer;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.@interface
{
    public interface IuserRepo
    {
        bool CheckEmail(String email);
        bool CheckPassword(string password);
        UserEntity UserRegistration(RegisterModel registerModel);
        string UserLogin(LoginModel loginModel);
        bool Resetpassword(string email, string password, string ConfirmPasswor);
        ForgetPasswordModel ForgetPassword(String email);
        //string GenerateToken(int UserId, string userEmail);
        int GetCount(int userId);

        List<UserEntity> GetAllusers();
        IEnumerable<UserEntity> GetByname(string name);
        UserEntity UserLoginForSession(string email, string password);
        int GetColaboratorCount(int userId);
        int GetLableCount(int userId);
    }
}