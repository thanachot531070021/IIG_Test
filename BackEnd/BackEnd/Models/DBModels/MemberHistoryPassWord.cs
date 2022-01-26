using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd.Models.DBModels
{
    public partial class MemberHistoryPassWord
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
