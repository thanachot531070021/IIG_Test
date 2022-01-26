using System;
using System.Collections.Generic;

#nullable disable

namespace BackEnd.Models.DBModels
{
    public partial class MemberImage
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
