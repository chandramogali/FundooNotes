using BusinessLayer.Interface;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using RepositoryLayer.entity;
using RepositoryLayer.servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness business;
        private readonly IBus bus;
        private readonly ILogger<UserController> logger;

        public UserController(IUserBusiness business, IBus bus, ILogger<UserController> logger)
        {
            this.business = business;
            this.bus = bus;
            this.logger = logger;
           
        }

        [HttpPost("Reg")]
        public ActionResult Register(RegisterModel model)
        {
            var result= business.UserRegistration(model);
            
            if(result != null) {
                
                return Ok(new ResponseModel<UserEntity> { success = true, message = "reg success", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { success = false, message = "reg not success", Data = result });
            }
        }

        [HttpPost("Login")]

        public ActionResult Login(LoginModel model)
        {
            

            bool email = business.CheckEmail(model.Email);

            bool pass = business.CheckPassword(model.Password);


            if (email != true && pass != true)
            {
                
                return BadRequest(new ResponseModel<bool> { success = false, message = "UserEmail and Paaword does not  match", });
            }

            if (email != true)
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "UserEmail does not  Exist", });
            }


            if (pass != true)
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "password does not  Exist", });
            }

            string result =business.UserLogin(model);

            if (result != null)
            {
                logger.LogInformation("Login success for user {Username}", model.Email);
                return Ok(new ResponseModel<string> { success = true, message = "Login Sucsess", Data=result });
            }
            return BadRequest(new ResponseModel<string> { success = false, message = "Login fails", });
        }

        [HttpPost("sessionLogin")]
        public ActionResult GetLoginGetails(string email, string password)
        {
            var result = business.UserLoginForSession(email,password);
           // string token=business.UserLogin(model);

            if (result != null)
            {
                HttpContext.Session.SetInt32("UserId" ,(int)result.UserId);

                //var response = new
                //{
                //    User = result,
                //    Token = token
                //};

                //return Ok(new ResponseModel<Object> { success = true, message = "Login Sucsess", Data = response });


                return Ok(new ResponseModel<UserEntity> { success = true, message = "Login Sucsess", Data = result});
            }

            return BadRequest(new ResponseModel<UserEntity> { success = false, message = "Login fails" });

        }

        [HttpPost("ForgetPassword")]
        public async  Task<IActionResult> UserForgetPassword(string email)
        {
            if (business.CheckEmail(email))
            {
                Send send = new Send();
                ForgetPasswordModel model = business.ForgetPassword(email);
                send.SendingMail(model.Email, model.Token);

                Uri uri =new Uri("rabbitmq://localhost/NotesEmail_Queue");
                var endPoint = await bus.GetSendEndpoint(uri);

                await endPoint.Send(model);

                return Ok(new ResponseModel<ForgetPasswordModel> { success=true,message="email send successfully", Data=model});
            }
            else
            {
                return BadRequest(new ResponseModel<ForgetPasswordModel> { success = false, message = "email not  send successfully" });

            }

        }
        [Authorize]
        [HttpPost("ResetPassword")]

        public ActionResult PasswordReSet( string password, string confirmPassword)
        {
            string email = User.Claims.FirstOrDefault(x=> x.Type == "Email").Value;
            var result = business.Resetpassword(email, password, confirmPassword);
            if (result)
            {
                return Ok(new ResponseModel<bool> { success = true, message = "Password Reset Successfully", Data = result });

            }
            else
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "Reset password faild" });
            }

        }


        [HttpGet("Checkemail")]
        public ActionResult CheckEmail(string email) { 
          var result=business.CheckEmail(email);
            if (result)
            {
                return Ok(new ResponseModel<bool> { success = true, message = "Email exist", Data = result });

            }
            else
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "Email not found" });
            }
        }

        [HttpGet("CheckPassword")]
        public ActionResult checkePassword(string password)
        {

            var result = business.CheckPassword(password);

            if (result)
            {

                return Ok(new ResponseModel<bool> { success = true, message = "password  exist", Data = result });

            }
            else
            {
                return BadRequest(new ResponseModel<bool> { success = false, message = "Password not found" });
            }
        }

        [HttpGet("getAllUsersByName")]
        public ActionResult getByname(string name)
        {
            IEnumerable<UserEntity> result=business.GetByname(name);

            if(result != null )
            {
                return Ok(new ResponseModel<IEnumerable<UserEntity>> { success = true, message = "data fetch sucessfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<UserEntity>> { success = false, message = "not found" });
            }
        }

        
        [HttpGet("getCount")]
        public ActionResult coutnt()
        {
            int user=int.Parse(User.Claims.Where(x=>x.Type=="UserId").FirstOrDefault().Value);
            int result=business.GetCount(user);
            if (result == 0)
            {
                return null;
            }
            return Ok(new ResponseModel<int> { success = true, message = "number of count", Data = result });
        }
        
    }
}
