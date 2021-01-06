using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedicalMask
{
    public class MaskContext : DbContext
    {
        public DbSet<MedicalMask> MedicalMasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MedicalMask.db");
        }
    }
}