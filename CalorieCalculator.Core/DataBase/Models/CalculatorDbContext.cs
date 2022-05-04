using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCalculator.Core.DataBase.Models
{
    public class CalculatorDbContext : DbContext
    {       
        public CalculatorDbContext(): base("name =CalculatorDbContext")
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<DailySum> DailySums { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<DailySum>()
                .Property(d => d.Date)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Setting>()
                .Property(s => s.Key)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Setting>()
                .Property(s => s.Value)
                .IsRequired()
                .HasMaxLength(50);

            base.OnModelCreating(modelBuilder);
        }
    }
}
