using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinavSistemi.API.Models
{
    [Table("RSS")]
    public class RSS
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
    }

    public class FindRSS
    {
        public int ID { get; set; }
    }

    public class UpdateRSS
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }
    }

    public class DeleteRSS
    {
        public int ID { get; set; }
    }
}
