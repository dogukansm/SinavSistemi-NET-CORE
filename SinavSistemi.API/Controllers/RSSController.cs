using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SinavSistemi.API.Helpers;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [ApiController]
    public class RSSController : ControllerBase
    {
        private DatabaseContext db;
        public RSSController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/[controller]/allrss")]
        public async Task<IActionResult> AllRSS()
        {
            List<RSS> rss = new List<RSS>();
            rss.AddRange(db.RSS.ToList());
            return Ok(rss);   
        }

        [HttpGet]
        [Route("api/[controller]/rss")]
        public async Task<IActionResult> RSS([FromQuery] FindRSS findrss)
        {
            List<RSS> rss = new List<RSS>();
            var CHECK_RSS = db.RSS.Where(s => s.ID == findrss.ID).FirstOrDefault();
            if (CHECK_RSS != null)
            {
                rss.Add(CHECK_RSS);
                return Ok(rss);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "RSS yayını bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpPost]
        [Route("api/[controller]/rss")]
        public async Task<IActionResult> RSS()
        {
            List<RSS> rss = new List<RSS>();

            string RSSURL = "https://www.wired.com/feed";
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(RSSURL);
            var items = (from s in xDoc.Descendants("item")
                         select new
                         {
                             title = s.Element("title").Value,
                             description = s.Element("description").Value,
                             category = s.Element("category").Value
                         });
            if (items != null)
            {
                foreach (var item in items)
                {
                    var CHECK_RSS = db.RSS.Where(s => s.Title == item.title).FirstOrDefault();
                    if (CHECK_RSS == null)
                    {
                        RSS add = new RSS();
                        add.Title = item.title;
                        add.Description = item.description;
                        add.Category = item.category;

                        db.RSS.Add(add);
                        db.SaveChanges();

                        rss.Add(add);
                    }
                }
            }

            return Ok(rss);
        }

        [HttpPut]
        [Route("api/[controller]/rss")]
        public async Task<IActionResult> RSS([FromBody] UpdateRSS updaterss)
        {
            List<RSS> rss = new List<RSS>();
            var FIND_RSS = db.RSS.Where(s => s.ID == updaterss.ID).FirstOrDefault();
            if (FIND_RSS != null)
            {
                FIND_RSS.Title = updaterss.Title;
                FIND_RSS.Description = updaterss.Description;
                FIND_RSS.Category = updaterss.Category;
                db.SaveChanges();

                rss.Add(FIND_RSS);
                return Ok(FIND_RSS);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "RSS bulunamadı!"
                });

                return NotFound(notfound);
            } 
        }

        [HttpDelete]
        [Route("api/[controller]/rss")]
        public async Task<IActionResult> RSS([FromQuery] DeleteRSS deleterss)
        {
            var FIND_RSS = db.RSS.Where(s => s.ID == deleterss.ID).FirstOrDefault();
            if (FIND_RSS != null)
            {
                db.RSS.Remove(FIND_RSS);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "RSS yayını başarıyla silindi!"
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "RSS yayını bulunamadı!"
                });

                return NotFound(notfound);
            }
        }
    }
}