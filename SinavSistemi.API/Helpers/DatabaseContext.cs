using System;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Helpers
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        { 
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<RSS> RSS { get; set; }
        public virtual DbSet<Exams> Exams { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Answers> Answers { get; set; }
        public virtual DbSet<Attendances> Attendances { get; set; }
    }
}
