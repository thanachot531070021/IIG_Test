using FrontEnd.Helpper;
using FrontEnd.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class DashBoardController : Controller
    {
        string Baseurl = "http://localhost:44119/";
        string UploadUrl = ConfigurationManager.AppSettings["UploadUrl"];

        /// <summary>
        /// Id ของสมาชิกที่ถูกเก็บใน DataBase
        /// </summary>
        int memberId = GlobalsVariable.MemberId;




        /// <summary>
        ///  หน้า DashBoard ดึงข้อมูลสมาชิกไปไปแสดงผลเป็นตาราง
        /// </summary>
        public ActionResult DashBoard()
        {
            try
            {
                var result = new CallAPI().GETDataAsync(Baseurl + "api/Member/DashBoardPage?id="+ memberId);
                var member = new DashBoardPageModel();
                if (result != null)
                {
                    member= JsonConvert.DeserializeObject<DashBoardPageModel>(result.ToString(), new ExpandoObjectConverter());
                }
                return View(member);

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, message = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }




        /// <summary>
        ///  หน้า Profile เเสดงข้อมูลสมากชิกที่ Login และ เอาไว้แก้ไขข้อมูล
        /// </summary>
        public ActionResult Profile()
        {

            var result = new CallAPI().GETDataAsync(Baseurl + "api/Member/GetMemberById?memberId=" + memberId);
            var member = new MemberModel();
            ViewBag.FileUrl = UploadUrl;
            if (result != null)
            {
                member = JsonConvert.DeserializeObject<MemberModel>(result.ToString(), new ExpandoObjectConverter());
            }
            return View(member);
        }
    }
}