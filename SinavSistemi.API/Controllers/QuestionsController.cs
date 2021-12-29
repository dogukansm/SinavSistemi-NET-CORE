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
    public class QuestionsController : ControllerBase
    {
        DatabaseContext db;
        public QuestionsController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/[controller]/questions")]
        public async Task<IActionResult> Questions()
        {
            List<Questions> questions = new List<Questions>();
            questions.AddRange(db.Questions.ToList());
            return Ok(questions);
        }

        [HttpGet]
        [Route("api/[controller]/question")]
        public async Task<IActionResult> Question([FromQuery] FindQuestion question)
        {
            List<Questions> questions = new List<Questions>();
            var FIND_QUESTION = db.Questions.Where(s => s.ID == question.ID).FirstOrDefault();
            if (FIND_QUESTION != null)
            {
                questions.Add(FIND_QUESTION);
                return Ok(questions);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav sorusu bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpGet]
        [Route("api/[controller]/questionbyexam")]
        public async Task<IActionResult> QuestionByExam([FromQuery] FindQuestionByExam question)
        {
            List<Questions> questions = new List<Questions>();
            var FIND_QUESTION = db.Questions.Where(s => s.SinavID == question.SinavID).ToList();
            if (FIND_QUESTION != null)
            {
                questions.AddRange(FIND_QUESTION);
                return Ok(questions);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav sorusu bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpPost]
        [Route("api/[controller]/question")]
        public async Task<IActionResult> Question([FromBody] AddQuestion question)
        {
            Questions add = new Questions();
            add.SinavID = question.SinavID;
            add.Soru = question.Soru;
            add.Cevap_A = question.Cevap_A;
            add.Cevap_B = question.Cevap_B;
            add.Cevap_C = question.Cevap_C;
            add.Cevap_D = question.Cevap_D;
            add.DogruCevap = question.DogruCevap;
            add.OlusturulmaTarihi = DateTime.Now.ToString();

            db.Questions.Add(add);
            db.SaveChanges();

            return Ok(add);
        }

        [HttpPut]
        [Route("api/[controller]/question")]
        public async Task<IActionResult> Question([FromBody] UpdateQuestion question)
        {
            var FIND_QUESTION = db.Questions.Where(s => s.ID == question.ID).FirstOrDefault();
            if (FIND_QUESTION != null)
            {
                FIND_QUESTION.SinavID = question.SinavID;
                FIND_QUESTION.Soru = question.Soru;
                FIND_QUESTION.Cevap_A = question.Cevap_A;
                FIND_QUESTION.Cevap_B = question.Cevap_B;
                FIND_QUESTION.Cevap_C = question.Cevap_C;
                FIND_QUESTION.Cevap_D = question.Cevap_D;
                FIND_QUESTION.DogruCevap = question.DogruCevap;
                db.SaveChanges();

                return Ok(FIND_QUESTION);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav sorusu bulunamadı!"
                });

                return NotFound(notfound);
            } 
        }

        [HttpDelete]
        [Route("api/[controller]/question")]
        public async Task<IActionResult> Question([FromBody] DeleteQuestion question)
        {
            var FIND_QUESTION = db.Questions.Where(s => s.ID == question.ID).FirstOrDefault();
            if (FIND_QUESTION != null)
            {
                db.Questions.Remove(FIND_QUESTION);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Sınav sorusu başarıyla silindi!"
                });

                return Ok(res);
            }   
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Sınav sorusu bulunamadı!"
                });

                return NotFound(notfound);
            } 
        }

    }
}