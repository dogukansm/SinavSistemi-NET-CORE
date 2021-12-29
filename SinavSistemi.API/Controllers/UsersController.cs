using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Helpers;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private DatabaseContext db;
        public UsersController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("api/[controller]/users")]
        public async Task<IActionResult> Users()
        {
            List<Users> users = new List<Users>();
            users.AddRange(db.Users.ToList());
            return Ok(users);
        }

        [HttpGet]
        [Route("api/[controller]/user")]
        public async Task<IActionResult> User([FromQuery] FindUser user)
        {
            List<Users> users = new List<Users>();
            var FIND_USERS = db.Users.Where(s => s.ID == user.ID).FirstOrDefault();
            users.Add(FIND_USERS);
            if (FIND_USERS != null)
            {
                return Ok(users);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Kullanici bulunamadı!"
                });
                return NotFound(notfound);
            }
        }

        [HttpGet]
        [Route("api/[controller]/login")]
        public async Task<IActionResult> Login([FromQuery] LoginUser user)
        {
            var FIND_USER = db.Users.Where(s => s.KullaniciAdi == user.KullaniciAdi && s.Sifre == user.Sifre).FirstOrDefault();
            if (FIND_USER != null)
            {
                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Giriş başarılı!",
                    ID = FIND_USER.ID
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Kullanıcı bulunamadı!"
                });

                return NotFound(notfound);
            }
        }

        [HttpPost]
        [Route("api/[controller]/user")]
        public async Task<IActionResult> User([FromBody] AddUser user)
        {
            var CHECK_USER = db.Users.Where(s => s.KullaniciAdi == user.KullaniciAdi).Count();
            if (CHECK_USER != 0)
            {
                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Kullanıcı mevcut!"
                });

                return Ok(res);
            }
            else
            {
                Users add = new Users();
                add.Ad = user.Ad;
                add.Soyad = user.Soyad;
                add.KullaniciAdi = user.KullaniciAdi;
                add.Sifre = user.Sifre;
                add.KayitTarihi = DateTime.Now.ToString();

                db.Users.Add(add);
                db.SaveChanges();

                return Ok(add);
            }
        }

        [HttpPut]
        [Route("api/[controller]/user")]
        public async Task<IActionResult> User([FromBody] UpdateUser user)
        {
            List<Users> users = new List<Users>();
            var FIND_USER = db.Users.Where(s => s.ID == user.ID).FirstOrDefault();
            if (FIND_USER != null)
            {
                FIND_USER.Ad = user.Ad;
                FIND_USER.Soyad = user.Soyad;
                FIND_USER.Sifre = user.Sifre;
                db.SaveChanges();

                users.Add(FIND_USER);
                return Ok(users);
            }
            else
            {
                List<dynamic> notfound = new List<dynamic>();
                notfound.Add(new
                {
                    title = "Kullanıcı bulunamadı!"
                });
                return NotFound(notfound);
            }
            
        }

        [HttpDelete]
        [Route("api/[controller]/user")]
        public async Task<IActionResult> User([FromQuery] DeleteUser user)
        {
            var FIND_USER = db.Users.Where(s => s.ID == user.ID).FirstOrDefault();
            if (FIND_USER != null)
            {
                db.Users.Remove(FIND_USER);
                db.SaveChanges();

                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Kullanıcı başarıyla silindi!"
                });

                return Ok(res);
            }
            else
            {
                List<dynamic> res = new List<dynamic>();
                res.Add(new
                {
                    title = "Kullanıcı bulunamadı!"
                });

                return NotFound(res);
            }
        }
    }
}