using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{

    public class DashBoardPageModel
    {
        public List<MemberModel> MemberList { get; set; }
    }


    public class MemberModel
    {
        public int Id { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public AttachmentModel MemberImage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }

    }

    public class MemberCheckUpdateModels
    {
        public int Id { get; set; }
        public string Password { get; set; }
    }


}