using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinavSistemi.API.Models
{
    [Table("Answers")]
    public class Answers
    {
        [Key]
        public int ID { get; set; }

        public int KullaniciID { get; set; }

        public int KatilimID { get; set; }

        public int SoruID { get; set; }

        public int Cevap { get; set; }

        public string CevapTarihi { get; set; }
    }

    public class FindAnswer
    {
        public int ID { get; set; }
    }

    public class FindAnswerByAttendance
    {
        public int KullaniciID { get; set; }

        public int KatilimID { get; set; }
    }

    public class AddAnswer
    {
        public int KullaniciID { get; set; }

        public int KatilimID { get; set; }

        public int SoruID { get; set; }

        public int Cevap { get; set; }

        public string CevapTarihi { get; set; }
    }

    public class UpdateAnswer
    {
        public int ID { get; set; }

        public int KullaniciID { get; set; }

        public int KatilimID { get; set; }

        public int SoruID { get; set; }

        public int Cevap { get; set; }
    }

    public class DeleteAnswer
    {
        public int ID { get; set; }
    }
}
