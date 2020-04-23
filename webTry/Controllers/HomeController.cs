using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using webTry.Models;
namespace webTry.Controllers
{
    public class HomeController : ApiController
    {

        DapperClass dapperClass = new DapperClass();
        // GET api/values
        public IEnumerable<Models.User> Get()
        {
            return dapperClass.SearchUser();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        [AcceptVerbs("GET", "POST")]
        public string Login(string identity ,string pass)
        {
            Boolean girdi = dapperClass.Login(identity, pass) , kayit;
            string token = "";
            if(girdi)
            {
                token = dapperClass.getToken(identity);
                if(token == "")
                {
                    do
                    {
                        token = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                        kayit = dapperClass.insertLogin(identity, token);
                    } while (kayit);
                }  
            }else
            {
                token = "başarısız";
            }
            return token;
        }
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [AcceptVerbs("GET", "POST")]
        public void addUser(string userIdentityNumber , string userPersonalName , string userPersonalSurname , string userPhoneNumber ,string userEmail,DateTime userBirthDate , string userPassword , string userImageSource , int userRol = 1)
        {
            Boolean dolu = dapperClass.controlUser(userIdentityNumber, userEmail);
            if(!dolu)
            {
                User yeni = new User();
                yeni.userIdentityNumber = userIdentityNumber;
                yeni.userPersonalName = userPersonalName;
                yeni.userPersonalSurname = userPersonalSurname;
                yeni.userPhoneNumber = userPhoneNumber;
                yeni.userEmail = userEmail;
                yeni.userBirthDate = userBirthDate;
                yeni.userPassword = userPassword;
                yeni.userImageSource = userImageSource;
                yeni.userRol = userRol;
                dapperClass.AddUser(yeni);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public void addUser(User user)
        {
            Boolean dolu = dapperClass.controlUser(user.userIdentityNumber, user.userEmail);
            if (!dolu)
            {
                dapperClass.AddUser(user);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public void Logout(string token)
        {
            dapperClass.Logout(token);
        }

        [AcceptVerbs("GET", "POST")]
        public void freezeAccount(string token)
        {
            dapperClass.freezeUser(token);
        }

        [AcceptVerbs("GET", "POST")]
        public void activeAccount(string identity , string pass)
        {
            dapperClass.active(identity, pass);
        }

        [AcceptVerbs("GET", "POST")]
        public Boolean controlLogin(string token)
        {
            return dapperClass.controlToken(token);
        }
        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
