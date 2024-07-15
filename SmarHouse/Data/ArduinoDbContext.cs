using SmartHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace SmartHouse.Data
{
    public class ArduinoDbContext : DbContext
    {
        public ArduinoDbContext(DbContextOptions<ArduinoDbContext> options) : base(options)
        {
        }

        public DbSet<Medicao> Medicoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Medicao>(d =>
            { d.Property(e => e.MedicaoId).ValueGeneratedOnAdd(); });


        }

     
    }
}
