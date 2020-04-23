using System;

namespace webTry.Models
{
    public class Login
    {
        public int id { get; set; }
        public string identityNumber { get; set; }
        public string token { get; set; }
        public DateTime time { get; set; }
    }
}