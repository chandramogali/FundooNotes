using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLayer.Servises
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IuserRepo repo;
        public UserBusiness(IuserRepo repo)
        {
            this.repo = repo;
        }
        public UserEntity UserRegistration(RegisterModel registerModel)
        {
            return repo.UserRegistration(registerModel);
        }

        public string UserLogin(LoginModel loginModel) { 
            return  repo.UserLogin(loginModel);
        }

        public bool Resetpassword(string email, string password, string ConfirmPasswor)
        {
               return repo.Resetpassword(email, password, ConfirmPasswor);
        }

        public ForgetPasswordModel ForgetPassword(String email)
        {
            return repo.ForgetPassword(email);
        }
        public bool CheckEmail(string email)
        {
            return repo.CheckEmail(email);
        }
        public bool CheckPassword(string password)
        {
            return repo.CheckPassword(password);
        }

        public IEnumerable<UserEntity> GetByname(string name)
        {
            return repo.GetByname(name);   
        }

        public List<UserEntity> GetAllusers()
        {
            return repo.GetAllusers();  
        }
        public UserEntity UserLoginForSession(string email, string password)
        {
            return repo.UserLoginForSession(email,password);
        }
        public int GetCount(int userId)
        {
            return repo.GetCount(userId);
        }

        public int GetColaboratorCount(int userId)
        {
            return repo.GetColaboratorCount(userId);
        }
        public int GetLableCount(int userId)
        {
            return repo.GetLableCount(userId);
        }
    }
}
