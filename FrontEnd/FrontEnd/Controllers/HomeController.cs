using FrontEnd.Helpper;
using FrontEnd.Models;
using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{


    public class GlobalsVariable
    {
        private static int _memberId;
        public static int MemberId
        {
            get
            {
                return _memberId;
            }
            set
            {
                _memberId = value;
            }
        }
    }

    public class HomeController : Controller
    {
        string Baseurl = "http://localhost:44119/";
        const string securityKey = "securityKeyIIG";

        //Folder เก็บไฟล์รูปของสมาชิก
        private static string FILE_PATH_Member = ConfigurationManager.AppSettings["GetLocalPathFileMember"];


        //Folder เข้าถึงหน้า Login
        public ActionResult Index()
        {
            return View("LogIn");
        }

        //Folder เข้าถึงหน้า เพิ่มสมาชิก
        public ActionResult Register()
        {
            return View();
        }


        //เพิ่มสมาชิก 
        public ActionResult SaveMember(MemberModel member)
        {
            try
            {
                var checkUserName = new CallAPI().GETDataAsync(Baseurl + "api/Member/CheckUserName?username=" + member.Username);
                if (Convert.ToBoolean(checkUserName) == false)
                {
                    return Json(new { code = 200, checkUserName = false }, JsonRequestBehavior.AllowGet);
                }

                AttachmentModel File = null;
                if (member.MemberImage != null)
                {
                    member.MemberImage = FileController.PathFile_PATH(member.MemberImage, FILE_PATH_Member);
                    File = member.MemberImage;
                }

                member.Password = EncryptPlainTextToCipherText(member.Password, securityKey);
                var result = new CallAPI().POSTDataAsync(member, Baseurl + "api/Member/Create");
                if (Convert.ToBoolean(result) == true && member.MemberImage != null)
                {
                    FileController.SaveFile_PATH(File, FILE_PATH_Member);
                }

                return Json(new { code = 200, checkUserName = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult UpdateMember(MemberModel member)
        {

            try
            {
                member.Id = GlobalsVariable.MemberId;
                member.Password = EncryptPlainTextToCipherText(member.Password, securityKey);
                var dataMemberCheck = new MemberCheckUpdateModels
                {
                    Id = member.Id,
                    Password = member.Password
                };

                var checkHistoryPassWord = new CallAPI().POSTDataAsync(member, Baseurl + "api/Member/GetCheckPassword");
                if (Convert.ToBoolean(checkHistoryPassWord) == false)
                {
                    return Json(new { code = 200 , passwordStatus=false }, JsonRequestBehavior.AllowGet);
                }



                AttachmentModel File = null;
                if (member.MemberImage != null)
                {
                    member.MemberImage = FileController.PathFile_PATH(member.MemberImage, FILE_PATH_Member);
                    File = member.MemberImage;
                }


                var result = new CallAPI().POSTDataAsync(member, Baseurl + "api/Member/Update");
                if (Convert.ToBoolean(result) == true && member.MemberImage != null)
                {
                    FileController.SaveFile_PATH(File, FILE_PATH_Member);
                }



                return Json(new { code = 200, passwordStatus = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }






        [HttpPost]
        public ActionResult LogInMember(string Username, string Password)
        {
            try
            {
                var data = new MemberModel
                {
                    Username = Username,
                    Password = EncryptPlainTextToCipherText(Password, securityKey)
                };

                ServiceResult result = new CallAPI().POSTData(data, Baseurl + "api/Member/LogIn");
                GlobalsVariable.MemberId = Convert.ToInt32(result.data);
                return Json(new { code = result.code, data = GlobalsVariable.MemberId }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpGet]
        public ActionResult GetMemberById(int memberId)
        {
            try
            {
                var result = new CallAPI().GETDataAsync(Baseurl + "api/Member/LogIn?Id=" + memberId);
                return Json(new { code = result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpPost]
        public ActionResult SetUpload(FileReviewDocumentModel request)
        {
            var result = new AttachmentModel();
            if (request != null)
            {
                var file = new AttachmentModel();
                file.Code = string.Format(@"{0}", Guid.NewGuid());
                file.FileName = request.OriginalName;
                file.FileType = request.Type;
                file.FileSize = request.FileSize;
                file.Base64 = request.base64;

                result = file;
            }

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }




        public static string EncryptPlainTextToCipherText(string PlainText, string securityKey)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }



    }
}