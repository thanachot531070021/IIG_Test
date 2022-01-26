using BackEnd.Models.DBModels;
using BackEnd.Services;
using IIG_Models;
using Microsoft.AspNetCore.Mvc;
using System;


namespace BackEnd.Controllers
{

    //[Route("api/[controller]/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        IIGContext _context;


        public MemberController(IIGContext context)
        {
            _context = context;
        }


        // ส่วนของการดึงข้อมูลไปที่ หน้า DashBoard
        [HttpGet]
       [Route("api/Member/DashBoardPage")]
        public IActionResult DashBoardPage(int memberId)
        {
            try
            {

                var result = new MemberServices(_context).DashBoardPage(memberId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // ส่วนของการดึงข้อมูลสมาชอกที่ login
        [HttpGet]
        [Route("api/Member/GetMemberById")]
        public IActionResult GetMemberById(int memberId)
        {
            try
            {

                var result = new MemberServices(_context).GetMemberById(memberId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // ส่วยของกสนเช็ค Username ช้ำ
        [HttpGet]
        [Route("api/Member/CheckUserName")]
        public IActionResult Create(string username)
        {
            try
            {
                var result = new MemberServices(_context).CheckUsername(username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //ส่วนของการสร้างสมาชิก
        [HttpPost]
        [Route("api/Member/Create")]
        public IActionResult Create(MemberModels member)
        {
            try
            {
                var result = new MemberServices(_context).Save(member);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //ส่วนของการอัพเดตข้อมูลสมาชิก
        [HttpPost]
        [Route("api/Member/Update")]
        public IActionResult Update(MemberModels member)
        {
            try
            {
                var result = new MemberServices(_context).Update(member);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //ส่วนของการเช็ค login
        [HttpPost]
        [Route("api/Member/LogIn")]
        public IActionResult LogIn(MemberModels member)
        {
            try
            {
                var result = new MemberServices(_context).LogIn(member);

                if (result.isSuccess) {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

        //ส่วนของการเช็ค passwordช้ำ
        [HttpPost]
        [Route("api/Member/GetCheckPassword")]
        public IActionResult GetCheckPassword(MemberCheckUpdateModels memberData)
        {
            try
            {
                var result = new MemberServices(_context).GetHistoryPassWord(memberData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



    }
}
