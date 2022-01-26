using System;

namespace IIG_Models
{
    public class MemberModels
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







    public class MemberHistoryPassWordModels
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string PasswordHash { get; set; }
        public string SaltValues { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class PassWordModels
    {
        public string Hashed { get; set; }
        public string Salt { get; set; }
    }


}
