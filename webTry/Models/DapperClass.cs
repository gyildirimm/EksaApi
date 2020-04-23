using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
namespace webTry.Models
{
    public class DapperClass
    {
        public IEnumerable<Models.User> SearchUser()
        {
            IEnumerable<Models.User> model;
            try
            {
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
                {
                    model = sqlConnection.Query<Models.User>("SELECT * FROM USERS");
                    sqlConnection.Close();
                }
            }
            catch (Exception){throw;}      
            return model;
        }

        public Boolean Login(string identity , string pass)
        {
            Boolean login = false;  
            try
            {
                IEnumerable<Models.User> model;
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
                {
                    string q = "SELECT * FROM USERS Where userIdentityNumber=" + identity + " AND userPassword=" + pass + " AND isActivity=1";
                    model = sqlConnection.Query<Models.User>(q);
                    if (model.Count() > 0) { login = true; } else { login = false; }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex) { }
            return login;
        }

        public Boolean insertLogin(string identity , string token)
        {

            try
            {
                IEnumerable<Models.Login> model;
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
                {
                    var q = "SELECT * FROM Login Where token=@token";
                    var v = new DynamicParameters();
                    v.Add("@token", token);
                    model = sqlConnection.Query<Models.Login>(q, v);
                    if (model.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        var query = "INSERT INTO Login(identityNumber, token , time) VALUES(@identityNumber, @token , @time)";
                        var variables = new DynamicParameters();
                        variables.Add("@identityNumber", identity);
                        variables.Add("@token", token);
                        variables.Add("@time", DateTime.Now);
                        sqlConnection.Execute(query, variables);
                        /*string q2 = "SELECT * FROM Login Where identityNumber=" + identity;
                        model = sqlConnection.Query<Models.Login>(q2);
                        if(model.Count() > 0)
                        {
                            var u = "UPDATE Login SET time=@time , token=@token WHERE identityNumber=@identityNumber";
                            var v2 = new DynamicParameters();
                            v2.Add("@identityNumber", identity);
                            v2.Add("@token", token);
                            v2.Add("@time", DateTime.Now);
                            sqlConnection.Execute(u, v2);
                        }else
                        {
                            var query = "INSERT INTO Login(identityNumber, token , time) VALUES(@identityNumber, @token , @time)";
                            var variables = new DynamicParameters();
                            variables.Add("@identityNumber", identity);
                            variables.Add("@token", token);
                            variables.Add("@time", DateTime.Now);
                            sqlConnection.Execute(query, variables);
                        }    */
                        sqlConnection.Close();
                        return false;
                    }
                }
            }
            catch (Exception ex) { return false; }
        }

        public string getToken(String identity)
        {
            string token = "";  
            try
            {
                IEnumerable<Models.Login> model;
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
                {
                    var query = "SELECT token FROM Login Where identityNumber=@identityNumber";
                    var variables = new DynamicParameters();
                    variables.Add("@identityNumber", identity);
                    model = sqlConnection.Query<Models.Login>(query, variables);
                    if (model.Count() > 0)
                    {
                        token = model.First().token;
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex) { }
            return token;

        }

        public void Logout(string token)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var query = "DELETE FROM Login WHERE token=@token";
                    var variables = new DynamicParameters();
                    variables.Add("@token", token);
                    sqlConnection.Execute(query, variables);
                    sqlConnection.Close();
                }
                catch (Exception ex) { }
            }
        }

        public Boolean controlUser(string identity , string mail)
        {
            Boolean dolu = false;
            try
            {
                IEnumerable<Models.User> model;
                using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
                {
                    var query = "SELECT * FROM USERS Where identityNumber=@identityNumber OR userEmail=@userEmail";
                    var variables = new DynamicParameters();
                    variables.Add("@identityNumber", identity);
                    variables.Add("@userEmail", mail);
                    model = sqlConnection.Query<Models.User>(query, variables);
                    if (model.Count() > 0)
                    {
                        dolu = true;
                    }
                    else
                    {
                        dolu = false;
                    }
                    sqlConnection.Close();
                }
                
            }
            catch (Exception ex) { }
            return dolu;
        }

        public void AddUser(User User)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var query = "INSERT INTO USERS(userIdentityNumber,userPersonalName,userPersonalSurname,userPhoneNumber,userEmail,userBirthDate,userPassword,userImageSource,userRol) VALUES(@userIdentityNumber, @userPersonalName , @userPersonalSurname , @userPhoneNumber,@userEmail,@userBirthDate,@userPassword,@userImageSource,@userRol)";
                    var variables = new DynamicParameters();
                    variables.Add("@userIdentityNumber", User.userIdentityNumber);
                    variables.Add("@userPersonalName", User.userPersonalName);
                    variables.Add("@userPersonalSurname", User.userPersonalSurname);
                    variables.Add("@userPhoneNumber", User.userPhoneNumber);
                    variables.Add("@userEmail", User.userEmail);
                    variables.Add("@userBirthDate", User.userBirthDate);
                    variables.Add("@userPassword", User.userPassword);
                    variables.Add("@userImageSource", User.userImageSource);
                    variables.Add("@userRol", User.userRol);
                    sqlConnection.Execute(query, variables);
                    sqlConnection.Close();
                }
                catch (Exception ex){}
            }
        }

        public void freezeUser(string token)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var query = "UPDATE USERS SET isActivity=0 WHERE  userIdentityNumber = (SELECT identityNumber FROM Login WHERE token=@token)";
                    var variables = new DynamicParameters();
                    variables.Add("@token", token);
                    sqlConnection.Execute(query, variables);
                    var q2 = "DELETE FROM Login WHERE token=@token";
                    sqlConnection.Execute(q2, variables);
                    sqlConnection.Close();
                }
                catch (Exception ex) { }
            }
        }

        public void active(string identity , string pass)
        {
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var query = "UPDATE USERS SET isActivity=1 WHERE userIdentityNumber = @userIdentityNumber AND userPassword = @userPassword";
                    var variables = new DynamicParameters();
                    variables.Add("@userIdentityNumber", identity);
                    variables.Add("@userPassword", pass);
                    sqlConnection.Execute(query, variables);
                    sqlConnection.Close();
                }
                catch (Exception ex) { }
            }
        }

        public Boolean controlToken(string token)
        {
            Boolean girdi = false;
            IEnumerable<Models.Login> model;
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EksaContext"].ToString()))
            {
                try
                {
                    var query = "SELECT * FROM Login WHERE token=@token";
                    var variables = new DynamicParameters();
                    variables.Add("@token", token);
                    model = sqlConnection.Query<Models.Login>(query , variables);
                    if(model.Count() > 0) { girdi = true; } else { girdi = false; }
                    sqlConnection.Close();
                }
                catch (Exception ex) { }
            }
            return girdi;
        }

    }
}