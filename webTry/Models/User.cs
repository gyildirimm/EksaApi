using System;

namespace webTry.Models
{
    public class User
    {
        public int id { get; set; }
        public string userIdentityNumber { get; set; }
        public string userPersonalName { get; set; }
        public string userPersonalSurname { get; set; }
        public string userPassword { get; set; }
        public string userPhoneNumber { get; set; }
        public string userEmail { get; set; }
        public string userImageSource { get; set; }
        public DateTime userBirthDate { get; set; }
        public int userRol { get; set; }
        public Boolean isActivity { get; set; }
    }
}