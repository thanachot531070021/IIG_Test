using BackEnd.Helper;
using BackEnd.Models.DBModels;
using IIG.Common.Models;
using IIG_Models;
using System;
using System.Linq;
using static IIG_Models.ViewModel;

namespace BackEnd.Services
{

    public class MemberServices
    {

        IIGContext _context;

        public MemberServices(IIGContext context)
        {
            _context = context;
        }




        //ดึงข้อมูล สมาชิกโดยใช้ memberId
        public MemberModels GetMemberById(int memberId)
        {
            using (var db = _context)
            {

                var data = db.Members.Where(m => m.Id == memberId).FirstOrDefault();
                var result = new MemberModels
                {
                    Id = data.Id,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Username = data.Username,
                    CreateDate = data.CreateDate,
                    ModifyDate = data.ModifyDate,
                    MemberImage = GetMemberImage(memberId)
                };

                return result;
            }
        }

        // ดึงข้อมูล สามาชิกทั้งหมด ไปแสดง
        public DashBoardPageModel DashBoardPage(int memberId)
        {
            DashBoardPageModel DashBoardPage = new DashBoardPageModel();

            using (var db = _context)
            {
                DashBoardPage.MemberLogin = db.Members.Where(w => w.Id == memberId).FirstOrDefault();
                DashBoardPage.MemberList = db.Members.ToList();
            }
            return DashBoardPage;
        }

        // เช็คส่วนของ Username ช้ำหรือไม่
        public bool CheckUsername(string username)
        {
            using (var db = _context)
            {
                var data = db.Members.Where(w => w.Username == username).FirstOrDefault();
                if (data != null)
                {
                    return false;
                }
            }
            return true;
        }

        // บันทึกข้อมูลสมาชิก
        public bool Save(MemberModels member)
        {
            var result = false;
            using (var db = _context)
            {
                if (member != null)
                {
                    Member memberSave = new Member
                    {
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Username = member.Username,
                        CreateDate = DateTime.Now,
                    };

                    db.Members.Add(memberSave);

                    db.SaveChanges();
                    SavePassword(db, memberSave.Id, member.Password);
                    SaveMemberImage(db, member.MemberImage, memberSave.Id);

                    result = true;
                }
            }
            result = true;
            return result;
        }

        // อัพเดตข้อมูลสมาชิก
        public bool Update(MemberModels member)
        {
            var result = false;
            using (var db = _context)
            {
                if (member != null)
                {
                    var update = db.Members.Where(o => o.Id == member.Id).FirstOrDefault();

                    if (update != null)
                    {
                        update.FirstName = member.FirstName;
                        update.LastName = member.LastName;
                        update.ModifyDate = DateTime.Now;
                    }

                    db.SaveChanges();
                    SavePassword(_context, member.Id, member.Password);
                    UpdateMemberImage(_context, member.MemberImage, member.Id);

                    result = true;
                }



            }
            return result;
        }

        //บันทึกรหัสผ่านของสมาชิกลงตาราง MemberHistoryPassWords
        public bool SavePassword(IIGContext db, int memberId, string password)
        {
            var result = false;

            MemberHistoryPassWord data = new MemberHistoryPassWord
            {
                MemberId = memberId,
                PasswordHash = password,
                CreateDate = DateTime.Now,
            };

            db.MemberHistoryPassWords.Add(data);
            db.SaveChanges();
            result = true;
            return result;
        }

        // ส่วของการเช็ค login
        public ServiceResult LogIn(MemberModels member)
        {
            var result = new ServiceResult();
            using (var db = _context)
            {
                if (member != null)
                {
                    var memberData = db.Members.Where(w => w.Username == member.Username).FirstOrDefault();
                    var passordData = db.MemberHistoryPassWords.Where(o => o.MemberId == memberData.Id).OrderByDescending(o => o.CreateDate).FirstOrDefault();

                    var PassWordHasg = new PassWordHasg();

                    if (PassWordHasg.DecryptCipherTextToPlainText(member.Password) == PassWordHasg.DecryptCipherTextToPlainText(passordData.PasswordHash))
                    {
                        result.isSuccess = true;
                        result.data = memberData.Id;
                        result.code = 200;

                    }
                    else
                    {
                        result.code = 400;
                    }
                }



            }
            return result;
        }


        // เช็คส่วนของ แก้ไขรหัส ตรงกับที่ แก้ไข 5ครั่งบ่าสุดหรือไม่
        public bool GetHistoryPassWord(MemberCheckUpdateModels dataMemberCheck)
        {
            var result = false;
            using (var db = _context)
            {
                var data = db.MemberHistoryPassWords.Where(w => w.MemberId == dataMemberCheck.Id).OrderByDescending(u => u.CreateDate).Take(5);
                var PassWordHasg = new PassWordHasg();
                string passwordCheck = PassWordHasg.DecryptCipherTextToPlainText(dataMemberCheck.Password);

                foreach (var item in data)
                {
                    if (passwordCheck == PassWordHasg.DecryptCipherTextToPlainText(item.PasswordHash))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // บันทึกข้อมูล รูปภาพของสมากชิก ลงในตาราง MemberImages
        public bool SaveMemberImage(IIGContext db, AttachmentModel attachment, int memberId)
        {

            var result = false;
            if (attachment != null)
            {
                MemberImage memberImageSave = new MemberImage
                {
                    MemberId = memberId,
                    Code = attachment.Code,
                    FileName = attachment.FileName,
                    FileSize = attachment.FileSize,
                    FileType = attachment.FileType,
                    Path = attachment.Path,
                    IsActive = attachment.IsActive,
                    CreateDate = DateTime.Now,

                };
                db.MemberImages.Add(memberImageSave);
                db.SaveChanges();
            }
            result = true;

            return result;
        }

        // อัพเดตข้อมูล รูปภาพของสมากชิก ในตาราง MemberImages โดยอ้างอิง memberId
        public bool UpdateMemberImage(IIGContext db, AttachmentModel attachment, int memberId)
        {

            var result = false;
            if (attachment != null)
            {
                var update = db.MemberImages.Where(o => o.Id == memberId).FirstOrDefault();

                if (update != null)
                {
                    update.MemberId = memberId;
                    update.Code = attachment.Code;
                    update.FileName = attachment.FileName;
                    update.FileSize = attachment.FileSize;
                    update.FileType = attachment.FileType;
                    update.Path = attachment.Path;
                    update.IsActive = attachment.IsActive;
                    update.CreateDate = DateTime.Now;
                    db.SaveChanges();

                }
                else
                {
                    SaveMemberImage(db, attachment, memberId);
                }

                result = true;
            }
            return result;
        }

        // ดึงข้อมูล รูปภาพของสมากชิกในตาราง MemberImages โดยอ้างอิง memberId
        public AttachmentModel GetMemberImage(int memberId)
        {
            using (var db = _context)
            {
                AttachmentModel result = null;
                var data = db.MemberImages.Where(m => m.MemberId == memberId).OrderByDescending(w => w.CreateDate).FirstOrDefault();

                if (data != null)
                {
                    result = new AttachmentModel
                    {
                        Id = data.Id,
                        MemberId = data.MemberId,
                        Code = data.Code,
                        FileName = data.FileName,
                        FileSize = data.FileSize,
                        FileType = data.FileType,
                        Path = data.Path,
                    };
                }
                return result;
            }
        }
    }
}
