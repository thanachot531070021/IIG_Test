using BackEnd.Models.DBModels;
using System.Collections.Generic;

namespace IIG_Models
{
    public class ViewModel
    {
        public class DashBoardPageModel
        {
            public Member MemberLogin { get; set; }
            public List<Member> MemberList { get; set; }
        }
    }
}
