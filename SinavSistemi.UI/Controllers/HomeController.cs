using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SinavSistemi.API.Controllers;
using SinavSistemi.API.Models;
using SinavSistemi.UI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SinavSistemi.API.Helpers;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SinavSistemi.UI.Controllers
{
    public class HomeController : Controller
    {
        private string api = "https://localhost:5001/api/"; 

        [ActionName("index")]
        public async Task<IActionResult> Index()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "users/users";

            List<Users> users = new List<Users>();

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    users = JsonConvert.DeserializeObject<List<Users>>(apiResponse);
                }
            }

            dynamic model = new ExpandoObject();
            model.users = users;

            return View(model);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(string KullaniciAdi, string Sifre)
        {
            string apiUrl = api + "users/login?KullaniciAdi=" + KullaniciAdi + "&Sifre=" + Sifre;

            string resultOfLogin = "";
            int resultOfID = 0;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foreach (var item in JsonConvert.DeserializeObject<List<dynamic>>(apiResponse))
                    {
                        resultOfLogin = item["title"];
                        resultOfID = item["id"];
                    }
                }
            } 
            if (resultOfLogin == "Giriş başarılı!")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, resultOfID.ToString())
                };
                var useridentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index");
            }

            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string Ad, string Soyad, string KullaniciAdi, string Sifre)
        {
            if ((Ad != null && Ad != "") && (Soyad != null && Soyad != "") && (KullaniciAdi != null && KullaniciAdi != "") && (Sifre != null && Sifre != ""))
            {
                try
                {
                    string apiUrl = api + "users/user";
                    dynamic register = new ExpandoObject();
                    var input = new
                    {
                        Ad = Ad,
                        Soyad = Soyad,
                        KullaniciAdi = KullaniciAdi,
                        Sifre = Sifre
                    };

                    var json = JsonConvert.SerializeObject(input);

                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json")))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (apiResponse.Contains("title"))
                            {
                                register = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                                TempData["HATA"] = "Kullanıcı daha önce kayıt olmuş!";
                                return RedirectToAction("Register");
                            }
                            else
                            {
                                register = JsonConvert.DeserializeObject<Users>(apiResponse);
                                TempData["BASARILI"] = "Kayıt başarılı!";
                                return RedirectToAction("Login");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["HATA"] = "Kayıt yapılırken bir sorun oluştu!";
                    return RedirectToAction("Register");
                }
            }
            else
            {
                TempData["HATA"] = "Kutucuklar boş olamaz!";
                return RedirectToAction("Register");
            }
        }

        [HttpGet]
        [Route("users/add")]
        public async Task<IActionResult> UsersAdd()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        [Route("users/add")]
        public async Task<IActionResult> UsersAdd(string Ad, string Soyad, string KullaniciAdi, string Sifre)
        {
            if ((Ad != null && Ad != "") && (Soyad != null && Soyad != "") && (KullaniciAdi != null && KullaniciAdi != "") && (Sifre != null && Sifre != ""))
            {
                try
                {
                    string apiUrl = api + "users/user";
                    dynamic register = new ExpandoObject();
                    var input = new
                    {
                        Ad = Ad,
                        Soyad = Soyad,
                        KullaniciAdi = KullaniciAdi,
                        Sifre = Sifre
                    };

                    var json = JsonConvert.SerializeObject(input);

                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json")))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (apiResponse.Contains("title"))
                            {
                                register = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                                TempData["HATA"] = "Kullanıcı daha önce kayıt olmuş!";
                                return RedirectToAction("UsersAdd");
                            }
                            else
                            {
                                register = JsonConvert.DeserializeObject<Users>(apiResponse);
                                TempData["BASARILI"] = "Kayıt başarılı!";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["HATA"] = "Kayıt yapılırken bir sorun oluştu!";
                    return RedirectToAction("UsersAdd");
                }
            }
            else
            {
                TempData["HATA"] = "Kutucuklar boş olamaz!";
                return RedirectToAction("UsersAdd");
            }
        }

        [HttpGet]
        [Route("users/edit")]
        public async Task<IActionResult> UsersEdit(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "users/user?ID=" + id;

            List<Users> user = new List<Users>();

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<List<Users>>(apiResponse);
                }
            }

            ViewBag.user = user[0];

            return View();
        }

        [HttpPost]
        [Route("users/edit")]
        public async Task<IActionResult> UsersEdit(int id, string Ad, string Soyad, string KullaniciAdi, string Sifre)
        {
            if ((Ad != null && Ad != "") && (Soyad != null && Soyad != ""))
            {
                string apiURL = api + "users/user";
                dynamic input;
                if (Sifre != null)
                {
                    input = new
                    {
                        ID = id,
                        Ad = Ad,
                        Soyad = Soyad,
                        Sifre = Sifre
                    };
                }
                else
                {
                    input = new
                    {
                        ID = id,
                        Ad = Ad,
                        Soyad = Soyad
                    };
                }


                var json = JsonConvert.SerializeObject(input);

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsync(apiURL, new StringContent(json, Encoding.UTF8, "application/json")))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode.ToString() == "OK")
                        {
                            TempData["BASARILI"] = "Kullanıcı bilgileri güncellendi!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["HATA"] = "Kullanıcı bilgileri güncellenirken hata!";
                            return RedirectToAction("UsersEdit", new { id = id });
                        }
                    }
                }
            }
            else
            {
                TempData["HATA"] = "Kutucuklar boş olamaz!";
                return RedirectToAction("UsersEdit", new { id = id });
            }
        }

        [Route("users/remove")]
        public async Task<IActionResult> UsersRemove(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "users/user?ID="+id;

            string resultOfLogin = "";

            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.DeleteAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foreach (var item in JsonConvert.DeserializeObject<List<dynamic>>(apiResponse))
                    {
                        resultOfLogin = item["title"]; 
                    }
                }
            }

            if (resultOfLogin == "Kullanıcı başarıyla silindi!")
            {
                TempData["BASARILI"] = "Başarıyla silindi!";
                return RedirectToAction("Index");
            }
            else if (resultOfLogin == "Kullanıcı bulunamadı!")
            {
                TempData["HATA"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Index");
            }

            return View();
        }

        [Route("rss")]
        public async Task<IActionResult> RSS()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "rss/allrss";

            List<RSS> rss = new List<RSS>();

            using(var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<List<RSS>>(apiResponse);
                }
            }

            dynamic model = new ExpandoObject();
            model.rss = rss;

            return View(model);
        }

        [Route("rss/update")]
        public async Task<IActionResult> RSSUpdate()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "rss/rss";

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.PostAsync(apiURL, new StringContent(string.Empty,Encoding.UTF8, "application/json")))
                {
                    if (response.StatusCode.ToString() == "OK")
                    {
                        TempData["BASARILI"] = "RSS verileri güncellendi!";
                        return RedirectToAction("RSS");
                    }
                    else
                    {
                        TempData["HATA"] = "RSS verileri güncellenemedi!";
                        return RedirectToAction("RSS");
                    }
                }
            }

            return View();
        }

        [Route("rss/delete")]
        public async Task<IActionResult> RSSRemove(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "rss/rss?ID=" + id;

            string resultOfLogin = "";

            using (var httpClient = new HttpClient())
            {
                using(var response = await httpClient.DeleteAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foreach (var item in JsonConvert.DeserializeObject<List<dynamic>>(apiResponse))
                    {
                        resultOfLogin = item["title"];
                    }
                }
            }

            if (resultOfLogin == "RSS yayını başarıyla silindi!")
            {
                TempData["BASARILI"] = "Başarıyla silindi!";
                return RedirectToAction("RSS");
            }
            else if (resultOfLogin == "RSS yayını bulunamadı!")
            {
                TempData["HATA"] = "RSS bulunamadı!";
                return RedirectToAction("RSS");
            }

            return View();
        }

        [Route("exams")]
        public async Task<IActionResult> Exams()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURLExams = api + "exams/exams";
            List<ExamsView> exams = new List<ExamsView>();

            using(var httpClient = new HttpClient())
            {
                dynamic exam;
                dynamic rss;
                dynamic user;

                using (var response = await httpClient.GetAsync(apiURLExams))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    exam = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                for (int i = 0; i < exam.Count; i++)
                {
                    string apiURLRSS = api + "rss/rss?ID=" + exam[i]["rssid"];
                    using (var response = await httpClient.GetAsync(apiURLRSS))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        rss = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                    }

                    string apiURLUser = api + "users/user?ID=" + exam[i]["kullaniciID"];
                    using (var response = await httpClient.GetAsync(apiURLUser))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                    }

                    int id = exam[i]["id"];
                    int rssid = exam[i]["rssid"];
                    string rsstitle = rss[0]["title"];
                    int kullaniciid = exam[i]["kullaniciID"];
                    string kullaniciadsoyad = user[0]["ad"] + " " + user[0]["soyad"];
                    string olusturulmatarihi = exam[i]["olusturulmaTarihi"];

                    exams.Add(new ExamsView
                    {
                        ID = id,
                        RSSID = rssid,
                        RSSName = rsstitle,
                        KullaniciID = kullaniciid,
                        KullaniciAdSoyad = kullaniciadsoyad,
                        OlusturulmaTarihi = olusturulmatarihi
                    });
                }

                
            }

            dynamic model = new ExpandoObject();
            model.exams = exams;
            return View(model);
        }

        [HttpGet]
        [Route("exams/add")]
        public async Task<IActionResult> ExamsAdd()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "rss/allrss";

            List<RSS> rss = new List<RSS>();

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(apiURL))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<List<RSS>>(apiResponse);
                }
            }

            List<SelectListItem> RSSFeed = (from s in rss
                                            orderby s.ID descending
                                            select new SelectListItem
                                            {
                                                Text = s.Title,
                                                Value = s.ID.ToString()
                                            }).Take(5).ToList();
            ViewBag.RSSFeed = RSSFeed;

            List<SelectListItem> Cevaplar = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "A",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "B",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "C",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "D",
                    Value = "3"
                }
            };
            ViewBag.Cevaplar = Cevaplar;

            return View();
        }

        [HttpPost]
        [Route("exams/add")]
        public async Task<IActionResult> ExamsAdd(string RSSSelect, string Soru1, string Soru2, string Soru3, string Soru4, string Soru5,
            string Cevap1A, string Cevap1B, string Cevap1C, string Cevap1D, string Cevap2A, string Cevap2B, string Cevap2C, string Cevap2D,
            string Cevap3A, string Cevap3B, string Cevap3C, string Cevap3D, string Cevap4A, string Cevap4B, string Cevap4C, string Cevap4D,
            string Cevap5A, string Cevap5B, string Cevap5C, string Cevap5D, string DogruCevap1, string DogruCevap2, string DogruCevap3,
            string DogruCevap4, string DogruCevap5)
        {
            string apiURL = api + "exams/exam";
            int primaryID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.PrimarySid));
            int rssID = Convert.ToInt32(RSSSelect);

            dynamic input = new
            {
                RSSID = rssID,
                KullaniciID = primaryID
            };

            var json = JsonConvert.SerializeObject(input);

            using(var httpClient = new HttpClient())
            {
                dynamic rss;
                using(var response = await httpClient.PostAsync(apiURL, new StringContent(json, Encoding.UTF8, "application/json")))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                string apiURLExam = api + "questions/question";
                int sinavID = rss["id"];

                dynamic soru1 = new
                {
                    SinavID = sinavID,
                    Soru = Soru1,
                    Cevap_A = Cevap1A,
                    Cevap_B = Cevap1B,
                    Cevap_C = Cevap1C,
                    Cevap_D = Cevap1D,
                    DogruCevap = DogruCevap1
                };

                string soru1_json = JsonConvert.SerializeObject(soru1);
                using (var response = await httpClient.PostAsync(apiURLExam, new StringContent(soru1_json, Encoding.UTF8, "application/json")))
                { }

                dynamic soru2 = new
                {
                    SinavID = sinavID,
                    Soru = Soru2,
                    Cevap_A = Cevap2A,
                    Cevap_B = Cevap2B,
                    Cevap_C = Cevap2C,
                    Cevap_D = Cevap2D,
                    DogruCevap = DogruCevap2
                };

                string soru2_json = JsonConvert.SerializeObject(soru2);
                using (var response = await httpClient.PostAsync(apiURLExam, new StringContent(soru2_json, Encoding.UTF8, "application/json")))
                { }

                dynamic soru3 = new
                {
                    SinavID = sinavID,
                    Soru = Soru3,
                    Cevap_A = Cevap3A,
                    Cevap_B = Cevap3B,
                    Cevap_C = Cevap3C,
                    Cevap_D = Cevap3D,
                    DogruCevap = DogruCevap3
                };

                string soru3_json = JsonConvert.SerializeObject(soru3);
                using (var response = await httpClient.PostAsync(apiURLExam, new StringContent(soru3_json, Encoding.UTF8, "application/json")))
                { }

                dynamic soru4 = new
                {
                    SinavID = sinavID,
                    Soru = Soru4,
                    Cevap_A = Cevap4A,
                    Cevap_B = Cevap4B,
                    Cevap_C = Cevap4C,
                    Cevap_D = Cevap4D,
                    DogruCevap = DogruCevap4
                };

                string soru4_json = JsonConvert.SerializeObject(soru4);
                using (var response = await httpClient.PostAsync(apiURLExam, new StringContent(soru4_json, Encoding.UTF8, "application/json")))
                { }

                dynamic soru5 = new
                {
                    SinavID = sinavID,
                    Soru = Soru5,
                    Cevap_A = Cevap5A,
                    Cevap_B = Cevap5B,
                    Cevap_C = Cevap5C,
                    Cevap_D = Cevap5D,
                    DogruCevap = DogruCevap5
                };

                string soru5_json = JsonConvert.SerializeObject(soru5);
                using (var response = await httpClient.PostAsync(apiURLExam, new StringContent(soru5_json, Encoding.UTF8, "application/json")))
                { }

                TempData["BASARILI"] = "Sınav oluşturuldu!";
                return RedirectToAction("Exams");
            }

            return View();
        }

        [HttpGet]
        [Route("exams/edit")]
        public async Task<IActionResult> ExamsEdit(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "exams/exam?ID=" + id;
            List<SelectListItem> Cevaplar = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "A",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "B",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "C",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "D",
                    Value = "3"
                }
            };
            List<Exams> exams = new List<Exams>();
            List<RSS> rsslist = new List<RSS>();
            List<Questions> questions = new List<Questions>();
            List<dynamic> answers = new List<dynamic>();

            using (var httpClient = new HttpClient())
            {
                dynamic exam;
                using (var response = await httpClient.GetAsync(apiURL))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    exam = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic rss;
                int rssid = exam[0]["rssid"];
                string apiURLRSS = api + "rss/rss?ID=" + rssid;
                using (var response = await httpClient.GetAsync(apiURLRSS))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic question; 
                int examid = exam[0]["id"];
                string apiURLQuestion = api + "questions/questionbyexam?SinavID=" + examid;
                using(var response = await httpClient.GetAsync(apiURLQuestion))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    question = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                exams.Add(new Exams
                {
                    ID = exam[0]["id"],
                    RSSID = exam[0]["rssid"],
                    KullaniciID = exam[0]["kullaniciID"],
                    OlusturulmaTarihi = exam[0]["olusturulmaTarihi"]
                });
                rsslist.Add(new RSS
                {
                    ID = rss[0]["id"],
                    Title = rss[0]["title"],
                    Description = rss[0]["description"],
                    Category = rss[0]["category"]
                });
                for (int i = 0; i < question.Count; i++)
                {
                    questions.Add(new Questions
                    {
                        ID = question[i]["id"],
                        SinavID = question[i]["sinavID"],
                        Soru = question[i]["soru"],
                        Cevap_A = question[i]["cevap_A"],
                        Cevap_B = question[i]["cevap_B"],
                        Cevap_C = question[i]["cevap_C"],
                        Cevap_D = question[i]["cevap_D"],
                        DogruCevap = question[i]["dogruCevap"]
                    });
                    string dogrucevap = question[i]["dogruCevap"].ToString();
                    var cevap = (from s in Cevaplar
                                 select new SelectListItem
                                 {
                                     Text = s.Text,
                                     Value = s.Value,
                                     Selected = dogrucevap == s.Value
                                 }).ToList();
                    answers.Add(cevap);
                }

                

            }

            ViewBag.Exam = exams[0];
            ViewBag.RSSFeed = rsslist[0];
            ViewBag.Questions = questions.ToList();
            ViewBag.Cevaplar = answers;

            return View();
        }

        [HttpPost]
        [Route("exams/edit")]
        public async Task<IActionResult> ExamsEdit(int id, int Soru1ID, int Soru2ID, int Soru3ID, int Soru4ID, int Soru5ID,
            string Soru1, string Soru2, string Soru3, string Soru4, string Soru5, string Cevap1A, string Cevap1B, string Cevap1C,
            string Cevap1D, string Cevap2A, string Cevap2B, string Cevap2C, string Cevap2D, string Cevap3A, string Cevap3B, string Cevap3C,
            string Cevap3D, string Cevap4A, string Cevap4B, string Cevap4C, string Cevap4D,string Cevap5A, string Cevap5B, string Cevap5C,
            string Cevap5D, string DogruCevap1, string DogruCevap2, string DogruCevap3, string DogruCevap4, string DogruCevap5)
        {
            string apiURL = api + "questions/question"; 
            int _DogruCevap1 = Convert.ToInt32(DogruCevap1);
            int _DogruCevap2 = Convert.ToInt32(DogruCevap2);
            int _DogruCevap3 = Convert.ToInt32(DogruCevap3);
            int _DogruCevap4 = Convert.ToInt32(DogruCevap4);
            int _DogruCevap5 = Convert.ToInt32(DogruCevap5);

            UpdateQuestion input1 = new UpdateQuestion
            {
                ID = Soru1ID,
                SinavID = id,
                Soru = Soru1,
                Cevap_A = Cevap1A,
                Cevap_B = Cevap1B,
                Cevap_C = Cevap1C,
                Cevap_D = Cevap1D,
                DogruCevap = _DogruCevap1
            };

            UpdateQuestion input2 = new UpdateQuestion
            {
                ID = Soru2ID,
                SinavID = id,
                Soru = Soru2,
                Cevap_A = Cevap2A,
                Cevap_B = Cevap2B,
                Cevap_C = Cevap2C,
                Cevap_D = Cevap2D,
                DogruCevap = _DogruCevap2
            };

            UpdateQuestion input3 = new UpdateQuestion
            {
                ID = Soru3ID,
                SinavID = id,
                Soru = Soru3,
                Cevap_A = Cevap3A,
                Cevap_B = Cevap3B,
                Cevap_C = Cevap3C,
                Cevap_D = Cevap3D,
                DogruCevap = _DogruCevap3
            };

            UpdateQuestion input4 = new UpdateQuestion
            {
                ID = Soru4ID,
                SinavID = id,
                Soru = Soru4,
                Cevap_A = Cevap4A,
                Cevap_B = Cevap4B,
                Cevap_C = Cevap4C,
                Cevap_D = Cevap4D,
                DogruCevap = _DogruCevap4
            };

            UpdateQuestion input5 = new UpdateQuestion
            {
                ID = Soru5ID,
                SinavID = id,
                Soru = Soru5,
                Cevap_A = Cevap5A,
                Cevap_B = Cevap5B,
                Cevap_C = Cevap5C,
                Cevap_D = Cevap5D,
                DogruCevap = _DogruCevap5
            };

            var json1 = JsonConvert.SerializeObject(input1);
            var json2 = JsonConvert.SerializeObject(input2);
            var json3 = JsonConvert.SerializeObject(input3);
            var json4 = JsonConvert.SerializeObject(input4);
            var json5 = JsonConvert.SerializeObject(input5);

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.PutAsync(apiURL, new StringContent(json1, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PutAsync(apiURL, new StringContent(json2, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PutAsync(apiURL, new StringContent(json3, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PutAsync(apiURL, new StringContent(json4, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PutAsync(apiURL, new StringContent(json5, Encoding.UTF8, "application/json")))
                {

                }
            }

            return RedirectToAction("ExamsEdit", new { id = id });
        }

        [Route("exams/delete")]
        public async Task<IActionResult> ExamsRemove(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "exams/exam?ID=" + id;

            string resultOfLogin = "";

            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.DeleteAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foreach (var item in JsonConvert.DeserializeObject<List<dynamic>>(apiResponse))
                    {
                        resultOfLogin = item["title"];
                    }
                }
            }

            if (resultOfLogin == "Sınav başarıyla silindi!")
            {
                TempData["BASARILI"] = "Başarıyla silindi!";
                return RedirectToAction("Exams");
            }
            else if (resultOfLogin == "Sınav bulunamadı!")
            {
                TempData["HATA"] = "Sınav bulunamadı!";
                return RedirectToAction("Exams");
            }


            return View();
        }

        [HttpGet]
        [Route("exams/join")]
        public async Task<IActionResult> ExamsJoin(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "exams/exam?ID=" + id;
            List<SelectListItem> Cevaplar = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "A",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "B",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "C",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "D",
                    Value = "3"
                }
            };
            List<Exams> exams = new List<Exams>();
            List<RSS> rsslist = new List<RSS>();
            List<Questions> questions = new List<Questions>(); 

            using (var httpClient = new HttpClient())
            {
                dynamic exam;
                using (var response = await httpClient.GetAsync(apiURL))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    exam = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic rss;
                int rssid = exam[0]["rssid"];
                string apiURLRSS = api + "rss/rss?ID=" + rssid;
                using (var response = await httpClient.GetAsync(apiURLRSS))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic question;
                int examid = exam[0]["id"];
                string apiURLQuestion = api + "questions/questionbyexam?SinavID=" + examid;
                using (var response = await httpClient.GetAsync(apiURLQuestion))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    question = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                exams.Add(new Exams
                {
                    ID = exam[0]["id"],
                    RSSID = exam[0]["rssid"],
                    KullaniciID = exam[0]["kullaniciID"],
                    OlusturulmaTarihi = exam[0]["olusturulmaTarihi"]
                });
                rsslist.Add(new RSS
                {
                    ID = rss[0]["id"],
                    Title = rss[0]["title"],
                    Description = rss[0]["description"],
                    Category = rss[0]["category"]
                });
                for (int i = 0; i < question.Count; i++)
                {
                    questions.Add(new Questions
                    {
                        ID = question[i]["id"],
                        SinavID = question[i]["sinavID"],
                        Soru = question[i]["soru"],
                        Cevap_A = question[i]["cevap_A"],
                        Cevap_B = question[i]["cevap_B"],
                        Cevap_C = question[i]["cevap_C"],
                        Cevap_D = question[i]["cevap_D"],
                        DogruCevap = question[i]["dogruCevap"]
                    });
                }



            }

            ViewBag.Exam = exams[0];
            ViewBag.RSSFeed = rsslist[0];
            ViewBag.Questions = questions.ToList();
            ViewBag.Cevaplar = Cevaplar;

            return View();
        }

        [HttpPost]
        [Route("exams/join")]
        public async Task<IActionResult> ExamsJoin(int id, int Soru1ID, int Soru2ID, int Soru3ID, int Soru4ID, int Soru5ID,
            string Cevap1, string Cevap2, string Cevap3, string Cevap4, string Cevap5)
        {
            string apiURLAttendance = api + "attendances/attendance";
            string apiURL = api + "answers/answer";
            int primaryID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.PrimarySid));
            dynamic attendanceinput = new
            {
                KullaniciID = primaryID,
                SinavID = id,
                KatilimTarihi = DateTime.Now.ToString()
            };

            var jsonattendance = JsonConvert.SerializeObject(attendanceinput);

            using (var httpClient = new HttpClient())
            {
                dynamic attendance;
                using(var response = await httpClient.PostAsync(apiURLAttendance, new StringContent(jsonattendance, Encoding.UTF8, "application/json")))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    attendance = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                int attendanceid = attendance["id"];

                dynamic input1 = new
                {
                    KullaniciID = primaryID,
                    KatilimID = attendanceid,
                    SoruID = Soru1ID,
                    Cevap = Cevap1
                };

                dynamic input2 = new
                {
                    KullaniciID = primaryID,
                    KatilimID = attendanceid,
                    SoruID = Soru2ID,
                    Cevap = Cevap2
                };

                dynamic input3 = new
                {
                    KullaniciID = primaryID,
                    KatilimID = attendanceid,
                    SoruID = Soru3ID,
                    Cevap = Cevap3
                };

                dynamic input4 = new
                {
                    KullaniciID = primaryID,
                    KatilimID = attendanceid,
                    SoruID = Soru4ID,
                    Cevap = Cevap4
                };

                dynamic input5 = new
                {
                    KullaniciID = primaryID,
                    KatilimID = attendanceid,
                    SoruID = Soru5ID,
                    Cevap = Cevap5
                };

                var json1 = JsonConvert.SerializeObject(input1);
                var json2 = JsonConvert.SerializeObject(input2);
                var json3 = JsonConvert.SerializeObject(input3);
                var json4 = JsonConvert.SerializeObject(input4);
                var json5 = JsonConvert.SerializeObject(input5);

                using (var response = await httpClient.PostAsync(apiURL, new StringContent(json1, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PostAsync(apiURL, new StringContent(json2, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PostAsync(apiURL, new StringContent(json3, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PostAsync(apiURL, new StringContent(json4, Encoding.UTF8, "application/json")))
                {

                }

                using (var response = await httpClient.PostAsync(apiURL, new StringContent(json5, Encoding.UTF8, "application/json")))
                {

                }
            }

            return RedirectToAction("Exams");
        }


        [Route("attendance")]
        public async Task<IActionResult> Attendance()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "attendances/attendances";

            List<ContributionsView> answers = new List<ContributionsView>();

            using(var httpClient = new HttpClient())
            {
                dynamic answer; 
                dynamic user;
                using (var response = await httpClient.GetAsync(apiURL))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    answer = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                for (int i = 0; i < answer.Count; i++)
                {
                    string apiURLUser = api + "users/user?ID=" + answer[i]["kullaniciID"];
                    using (var response = await httpClient.GetAsync(apiURLUser))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                    }

                    int id = answer[i]["id"];
                    int kullaniciid = answer[i]["kullaniciID"];
                    string kullaniciadsoyad = user[0]["ad"] + " " + user[0]["soyad"];
                    int sinavID = answer[i]["sinavID"]; 
                    string katilimTarihi = answer[i]["katilimTarihi"];

                    answers.Add(new ContributionsView
                    {
                        ID = id,
                        KullaniciID = kullaniciid,
                        KullaniciAdSoyad = kullaniciadsoyad,
                        SinavID = sinavID,
                        KatilimTarihi = katilimTarihi
                    });
                }
            }

            dynamic model = new ExpandoObject();
            model.contributions = answers;

            return View(model);
        }

        [HttpGet]
        [Route("attendance/view")]
        public async Task<IActionResult> AttendanceView(int id)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated == false)
            {
                return RedirectToAction("Login");
            }

            string apiURL = api + "attendances/attendance?ID=" + id;
            List<SelectListItem> Cevaplar = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "A",
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = "B",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "C",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "D",
                    Value = "3"
                }
            }; 
            List<Exams> exams = new List<Exams>();
            List<RSS> rsslist = new List<RSS>();
            List<Questions> questions = new List<Questions>();
            List<dynamic> answers = new List<dynamic>();
            List<dynamic> myanswers = new List<dynamic>();

            using (var httpClient = new HttpClient())
            {
                dynamic attendance;
                using(var response = await httpClient.GetAsync(apiURL))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    attendance = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                string apiURLExam = api + "exams/exam?ID=" + attendance[0]["sinavID"];
                dynamic exam;
                using (var response = await httpClient.GetAsync(apiURLExam))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    exam = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic rss;
                int rssid = exam[0]["rssid"];
                string apiURLRSS = api + "rss/rss?ID=" + rssid;
                using (var response = await httpClient.GetAsync(apiURLRSS))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                dynamic question;
                int examid = exam[0]["id"];
                string apiURLQuestion = api + "questions/questionbyexam?SinavID=" + examid;
                using (var response = await httpClient.GetAsync(apiURLQuestion))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    question = JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }

                exams.Add(new Exams
                {
                    ID = exam[0]["id"],
                    RSSID = exam[0]["rssid"],
                    KullaniciID = exam[0]["kullaniciID"],
                    OlusturulmaTarihi = exam[0]["olusturulmaTarihi"]
                });
                rsslist.Add(new RSS
                {
                    ID = rss[0]["id"],
                    Title = rss[0]["title"],
                    Description = rss[0]["description"],
                    Category = rss[0]["category"]
                });
                for (int i = 0; i < question.Count; i++)
                {
                    questions.Add(new Questions
                    {
                        ID = question[i]["id"],
                        SinavID = question[i]["sinavID"],
                        Soru = question[i]["soru"],
                        Cevap_A = question[i]["cevap_A"],
                        Cevap_B = question[i]["cevap_B"],
                        Cevap_C = question[i]["cevap_C"],
                        Cevap_D = question[i]["cevap_D"],
                        DogruCevap = question[i]["dogruCevap"]
                    });
                    string dogrucevap = question[i]["dogruCevap"].ToString();
                    var cevap = (from s in Cevaplar
                                 select new SelectListItem
                                 {
                                     Text = s.Text,
                                     Value = s.Value,
                                     Selected = dogrucevap == s.Value
                                 }).ToList();
                    answers.Add(cevap);
                }

                
                string apiURLMyAnswers = api + "answers/answerbyattendance?KullaniciID=" + attendance[0]["kullaniciID"] + "&KatilimID=" + attendance[0]["id"];
                using(var response = await httpClient.GetAsync(apiURLMyAnswers))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    myanswers = JsonConvert.DeserializeObject<List<dynamic>>(apiResponse);
                }
            }

            ViewBag.Exam = exams[0];
            ViewBag.RSSFeed = rsslist[0];
            ViewBag.Questions = questions.ToList();
            ViewBag.Cevaplar = answers;
            ViewBag.MyAnswers = myanswers;

            return View();
        }



        public async Task<JsonResult> GetRSSText(int p)
        {
            string apiURL = api + "rss/rss?ID=" + p;

            List<RSS> rss = new List<RSS>();
            using(var httpClient = new HttpClient())
            {
                using(var response = await httpClient.GetAsync(apiURL))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rss = JsonConvert.DeserializeObject<List<RSS>>(apiResponse);
                }
            }

            return Json(rss[0]);
        }
    }
}
