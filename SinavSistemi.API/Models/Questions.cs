using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinavSistemi.API.Models
{
    [Table("Questions")]
    public class Questions
    {
        [Key]
        public int ID { get; set; }

        public int SinavID { get; set; }

        public string Soru { get; set; }

        public string Cevap_A { get; set; }

        public string Cevap_B { get; set; } 

        public string Cevap_C { get; set; } 

        public string Cevap_D { get; set; }

        public int DogruCevap { get; set; }

        public string OlusturulmaTarihi { get; set; }
    }

    public class FindQuestion
    {
        public int ID { get; set; }
    }

    public class FindQuestionByExam
    {
        public int SinavID { get; set; }
    }

    public class AddQuestion
    {
        public int SinavID { get; set; }

        public string Soru { get; set; }

        public string Cevap_A { get; set; }

        public string Cevap_B { get; set; }

        public string Cevap_C { get; set; }

        public string Cevap_D { get; set; }

        public int DogruCevap { get; set; }
    }

    public class UpdateQuestion
    {
        public int ID { get; set; }

        public int SinavID { get; set; }

        public string Soru { get; set; }

        public string Cevap_A { get; set; }

        public string Cevap_B { get; set; }

        public string Cevap_C { get; set; }

        public string Cevap_D { get; set; }

        public int DogruCevap { get; set; }
    }

    public class DeleteQuestion
    {
        public int ID { get; set; }
    }
}
