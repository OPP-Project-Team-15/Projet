using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.MySqlClient;

namespace Virtual_Global_College
{
    class VGC_SqlContext : DbContext
    {
        public VGC_SqlContext() : base("name = VGC_SqlContext") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) { }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
    }
}
