using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace glostars.Models.AdminPanelReport
{
    public class AdminLoginData
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Date { get; set; }

        public AdminLoginData()
        {
            Date = DateTime.Now;
        }
    }
}