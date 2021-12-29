using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinavSistemi.API.Models
{
    [Table("Exams")]
    public class Exams
    {
        [Key]
        public int ID { get; set; }

        public int RSSID { get; set; }

        public int KullaniciID { get; set; }

        public string OlusturulmaTarihi { get; set; }
    }

    public class FindExam
    {
        public int ID { get; set; }
    }

    public class AddExam
    {
        public int RSSID { get; set; }

        public int KullaniciID { get; set; }

        public string OlusturulmaTarihi { get; set; }
    }

    public class UpdateExam
    {
        public int ID { get; set; }

        public int RSSID { get; set; }

        public int KullaniciID { get; set; } 
    }

    public class DeleteExam
    {
        public int ID { get; set; }
    }
}
