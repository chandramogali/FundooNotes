using RepositoryLayer.context;
using RepositoryLayer.entity;
using ModelLayer;

using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.@interface;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.servises
{
    public class userRepo : IuserRepo
    {
        private readonly FundooContext fundooContext;
        private readonly IConfiguration _config;

        public userRepo(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;
           this._config = config;
        }



        public UserEntity UserRegistration(RegisterModel registerModel)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.FirstName = registerModel.FirstName;
            userEntity.LastName = registerModel.LastName;
            userEntity.Email = registerModel.Email;
            var PasswordHash = EncodePassword(registerModel.Password);
            userEntity.Password = PasswordHash;

            fundooContext.User.Add(userEntity);
            fundooContext.SaveChanges();
            return userEntity;
        }

        public static string EncodePassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string UserLogin(LoginModel loginModel)
        {

           string  encyptedPass= EncodePassword(loginModel.Password);
            UserEntity user =fundooContext.User.FirstOrDefault(u => u.Email== loginModel.Email && u.Password== encyptedPass);
            if (user != null)
            {
                string token = GenerateToken(user.UserId, user.Email);
                return token;
            }
            return null;
        }

        public UserEntity UserLoginForSession(string email, string password)
        {
            string encyptedPass = EncodePassword(password);
            var user=fundooContext.User.Where(x=>x.Email==email && x.Password == encyptedPass).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return null;

        }

        public bool CheckEmail(String email)
        {

            var result = fundooContext.User.Where(e => e.Email == email).FirstOrDefault();

            if (result != null )
            {
                return true;
            }
            return false;
        }

        public bool CheckPassword(string password)
        {
            string encyptedPass = EncodePassword(password);
            var result = fundooContext.User.FirstOrDefault(x => x.Password == encyptedPass);

            if (result != null  )
            {
                return true;
            }
            return false;
        }

        public ForgetPasswordModel ForgetPassword(String email)
        {
            try
            {
                var result = fundooContext.User.FirstOrDefault(e => e.Email == email);

                if (result != null)
                {
                    ForgetPasswordModel forgetPassword = new ForgetPasswordModel();
                    forgetPassword.Email = result.Email;
                    forgetPassword.UserId = result.UserId;
                    forgetPassword.Token = GenerateToken(result.UserId, result.Email);
                    return forgetPassword;

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateToken(long UserId, string userEmail)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new []
            {
                new Claim("Email",userEmail), 
                new Claim("UserId", UserId.ToString()) 
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool Resetpassword(string email, string password, string ConfirmPasswor)
        {
            try
            {
                if (password.Equals(ConfirmPasswor))
                {
                    var user = fundooContext.User.FirstOrDefault(x => x.Email == email);
                    if (user != null)
                    {
                        var PasswordHash = EncodePassword(password);
                        user.Password = PasswordHash;
                        fundooContext.Entry(user).State = EntityState.Modified;
                        fundooContext.SaveChanges();
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<UserEntity> GetByname(string name)
        {
            IEnumerable<UserEntity> result = fundooContext.User.Where(x => x.FirstName == name).ToList();

            if(result != null)
            {
                return result;
            }
            return null;
        }

        public List<UserEntity> GetAllusers()
        {
            if(fundooContext.User == null)
            {
                return null;
            }
            return fundooContext.User.ToList();
        }

        public int GetCount( int userId)
        {
            int user=fundooContext.Notes.Where(x=>x.UserId==userId).Count();
            if (user > 0)
            {
                return user;
            }
            return 0;
        }

        public int GetLableCount(int userId)
        {
            int user = fundooContext.Labels.Where(x => x.UserId == userId).Count();
            if (user > 0)
            {
                return user;
            }
            return 0;
        }
        public int GetColaboratorCount(int userId)
        {
            int user = fundooContext.Collaborators.Where(x => x.UserId == userId).Count();
            if (user > 0)
            {
                return user;
            }
            return 0;
        }

    }


 
}
