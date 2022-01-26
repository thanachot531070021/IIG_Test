using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd.Models.DBModels
{
    public partial class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
