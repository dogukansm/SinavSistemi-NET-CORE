using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinavSistemi.API.Models
{
    [Table("Attendances")]
    public class Attendances
    {
        [Key]
        public int ID { get; set; }
        public int SinavID { get; set; }
        public int KullaniciID { get; set; }
        public string KatilimTarihi { get; set; }
    }

    public class FindAttendances
    {
        public int ID { get; set; }
    }

    public class AddAttendances
    {
        public int SinavID { get; set; }
        public int KullaniciID { get; set; }
        public string KatilimTarihi { get; set; }
    }

    public class UpdateAttendances
    {
        public int ID { get; set; }
        public int SinavID { get; set; }
        public int KullaniciID { get; set; }
    }

    public class DeleteAttendances
    {
        public int ID { get; set; }
    }
}
