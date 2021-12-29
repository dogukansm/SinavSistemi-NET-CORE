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
    public class ExamsController : ControllerBase
    {
        private DatabaseContext db;
        public ExamsController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/[controller]/exams")]
        public async Task<IActionResult> Exams()
        {
            List<Exams> exams = new List<Exams>();
            exams.AddRange(db.Exams.ToList());
            return Ok(exams);
        }

        [HttpGet]
        [Route("api/[controller]/exam")]
        public async Task<IActionResult> Exam([FromQuery] FindExam exam)
        {
            List<Exams> exams = new List<Exams>();
            var FIND_EXAM = db.Exams.Where(s => s.ID == exam.ID).FirstOrDefault();
            if (FIND_EXAM != null)
            {
                exams.Add(FIND_EXAM);
                return Ok(exams);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpPost]
        [Route("api/[controller]/exam")]
        public async Task<IActionResult> Exam([FromBody] AddExam exam)
        {
            Exams add = new Exams();
            add.RSSID = exam.RSSID;
            add.KullaniciID = exam.KullaniciID;
            add.OlusturulmaTarihi = DateTime.Now.ToString();

            db.Exams.Add(add);
            db.SaveChanges();

            return Ok(add); 
        }

        [HttpPut]
        [Route("api/[controller]/exam")]
        public async Task<IActionResult> Exam([FromBody] UpdateExam exam)
        {
            var FIND_EXAM = db.Exams.Where(s => s.ID == exam.ID).FirstOrDefault();
            if (FIND_EXAM != null)
            {
                FIND_EXAM.RSSID = exam.RSSID;
                db.SaveChanges();

                return Ok(FIND_EXAM);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpDelete]
        [Route("api/[controller]/exam")]
        public async Task<IActionResult> Exam([FromQuery] DeleteExam exam)
        {
            var FIND_EXAM = db.Exams.Where(s => s.ID == exam.ID).FirstOrDefault();
            if (FIND_EXAM != null)
            {
                var FIND_QUESTIONS = db.Questions.Where(s => s.SinavID == exam.ID).ToList();
                if (FIND_QUESTIONS != null)
                {
                    foreach (var item in FIND_QUESTIONS)
                    {
                        db.Questions.Remove(item);
                        db.SaveChanges();
                    }
                }
                db.Exams.Remove(FIND_EXAM);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Sınav başarıyla silindi!"
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav bulunamadı!"
                });

                return NotFound(notfound);
            }
        }
    }
}