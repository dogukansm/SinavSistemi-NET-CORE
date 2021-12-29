using System;
namespace SinavSistemi.UI.Models
{
    public class ContributionsView
    {
        public int ID { get; set; }
        public int KullaniciID { get; set; }
        public string KullaniciAdSoyad { get; set; }
        public int SinavID { get; set; }
        public string KatilimTarihi { get; set; }
    }
}
