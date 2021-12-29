using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SinavSistemi.API.Helpers;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private DatabaseContext db;
        public AttendancesController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("api/[controller]/attendances")]
        [HttpGet]
        public async Task<IActionResult> Attendances()
        {
            List<Attendances> attendances = new List<Attendances>();
            attendances.AddRange(db.Attendances.ToList());

            return Ok(attendances);
        }

        [Route("api/[controller]/attendance")]
        [HttpGet]
        public async Task<IActionResult> Attendance([FromQuery] FindAttendances attendance)
        {
            List<Attendances> attendances = new List<Attendances>();
            var FIND_ATTENDANCES = db.Attendances.Where(s => s.ID == attendance.ID).FirstOrDefault();
            if (FIND_ATTENDANCES != null)
            {
                attendances.Add(FIND_ATTENDANCES);
                return Ok(attendances);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Katılım bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [Route("api/[controller]/attendance")]
        [HttpPost]
        public async Task<IActionResult> Attendance([FromBody] AddAttendances attendance)
        {
            Attendances add = new Attendances();
            add.SinavID = attendance.SinavID;
            add.KullaniciID = attendance.KullaniciID;
            add.KatilimTarihi = DateTime.Now.ToString();
            db.Attendances.Add(add);
            db.SaveChanges();
            return Ok(add);
        }

        [Route("api/[controller]/attendance")]
        [HttpPut]
        public async Task<IActionResult> Attendance([FromBody] UpdateAttendances attendance)
        {
            var FIND_ATTENDANCE = db.Attendances.Where(s => s.ID == attendance.ID).FirstOrDefault();
            if (FIND_ATTENDANCE != null)
            {
                FIND_ATTENDANCE.SinavID = attendance.SinavID;
                db.SaveChanges();

                return Ok(FIND_ATTENDANCE);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Katılım bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [Route("api/[controller]/attendance")]
        [HttpDelete]
        public async Task<IActionResult> Attendance([FromQuery] DeleteAttendances attendance)
        {
            var FIND_ATTENDANCE = db.Attendances.Where(s => s.ID == attendance.ID).FirstOrDefault();
            if (FIND_ATTENDANCE != null)
            {
                var FIND_ANSWERS = db.Answers.Where(s => s.KatilimID == attendance.ID);
                foreach (var item in FIND_ANSWERS)
                {
                    db.Answers.Remove(item);
                    db.SaveChanges();
                }

                db.Attendances.Remove(FIND_ATTENDANCE);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Katılım başarıyla silindi"
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Katılım bulunamadı!"
                });

                return NotFound(notfound);
            }
        }
    }
}