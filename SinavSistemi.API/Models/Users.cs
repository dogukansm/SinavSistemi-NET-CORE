using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace SinavSistemi.API.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int ID { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string KullaniciAdi { get; set; }

        public string Sifre { get; set; }

        public string KayitTarihi { get; set; }
    }

    public class FindUser
    {
        public int ID { get; set; }
    }

    public class LoginUser
    {
        public string KullaniciAdi { get; set; }

        public string Sifre { get; set; }
    }

    public class AddUser
    {
        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string KullaniciAdi { get; set; }

        public string Sifre { get; set; }

        public string KayitTarihi { get; set; }
    }

    public class UpdateUser
    {
        public int ID { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string Sifre { get; set; }
    }

    public class DeleteUser
    {
        public int ID { get; set; }
    }
}
