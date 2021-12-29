using System;
namespace SinavSistemi.UI.Models
{
    public class ExamsView
    {
        public int ID { get; set; }

        public int RSSID { get; set; }

        public string RSSName { get; set; }

        public int KullaniciID { get; set; }

        public string KullaniciAdSoyad { get; set; }

        public string OlusturulmaTarihi { get; set; }
    }
}
