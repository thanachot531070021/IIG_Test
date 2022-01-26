using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIG_Models
{
    public class AttachmentModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string Code { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public DateTime CreateDate { get; set; }
        public string Path { get; set; }
        public string Base64 { get; set; }
        public bool IsActive { get; set; }

    }
}
