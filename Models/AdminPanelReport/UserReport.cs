using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace glostars.Models.AdminPanelReport
{
    public class UserReport
    {
        public int ID { get; set; }

        public string ReportCategory { get; set; }

        public string ReportedUserId { get; set; }
        public string ReportedUserName { get; set; }

        public string UploadedUserId { get; set; }
        public string UploadedUserName { get; set; }

        public int PictureId { get; set; }

        [DefaultValue(false)]
        public Boolean Action { get; set; }
    }
}