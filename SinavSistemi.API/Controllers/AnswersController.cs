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
    public class AnswersController : ControllerBase
    {
        private DatabaseContext db;
        public AnswersController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/[controller]/answers")]
        public async Task<IActionResult> Answers()
        {
            List<Answers> answers = new List<Answers>();
            answers.AddRange(db.Answers.ToList());
            return Ok(answers);
        }

        [HttpGet]
        [Route("api/[controller]/answer")]
        public async Task<IActionResult> Answer([FromQuery] FindAnswer answer)
        {
            List<Answers> answers = new List<Answers>();
            var FIND_ANSWERS = db.Answers.Where(s => s.ID == answer.ID).FirstOrDefault();
            if (FIND_ANSWERS != null)
            {
                answers.Add(FIND_ANSWERS);
                return Ok(answers);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Cevap bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpGet]
        [Route("api/[controller]/answerbyattendance")]
        public async Task<IActionResult> AnswerByAttendance([FromQuery] FindAnswerByAttendance answer)
        {
            List<Answers> answers = new List<Answers>();
            var FIND_ANSWERS = db.Answers.Where(s => s.KullaniciID == answer.KullaniciID && s.KatilimID == answer.KatilimID).ToList();
            if (FIND_ANSWERS != null)
            {
                answers.AddRange(FIND_ANSWERS);
                return Ok(answers);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Cevap bulunamadı!"
                });

                return NotFound(notfound);
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/[controller]/answer")]
        public async Task<IActionResult> Answer([FromBody] AddAnswer answer)
        {
            Answers add = new Answers();
            add.KullaniciID = answer.KullaniciID;
            add.KatilimID = answer.KatilimID;
            add.SoruID = answer.SoruID;
            add.Cevap = answer.Cevap;
            add.CevapTarihi = DateTime.Now.ToString();
            db.Answers.Add(add);
            db.SaveChanges();

            return Ok(add);
        }

        [HttpPut]
        [Route("api/[controller]/answer")]
        public async Task<IActionResult> Answer([FromBody] UpdateAnswer answer)
        {
            var FIND_ANSWER = db.Answers.Where(s => s.ID == answer.ID).FirstOrDefault();
            if (FIND_ANSWER != null)
            {
                FIND_ANSWER.Cevap = answer.Cevap;
                db.SaveChanges();

                return Ok(FIND_ANSWER);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Cevap bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpDelete]
        [Route("api/[controller]/answer")]
        public async Task<IActionResult> Answer([FromQuery] DeleteAnswer answer)
        {
            var FIND_ANSWER = db.Answers.Where(s => s.ID == answer.ID).FirstOrDefault();
            if (FIND_ANSWER != null)
            {
                db.Answers.Remove(FIND_ANSWER);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Cevap başarıyla silindi!"
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Cevap bulunamadı!"
                });

                return NotFound(notfound);
            }
        }
    }
}