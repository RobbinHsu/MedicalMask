using System;
using System.Collections.Generic;
using System.IO;
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
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            var directoryInfo = dir.Parent.Parent.Parent;
            optionsBuilder.UseSqlite($"Data Source={directoryInfo}/MedicalMask.db");
            //optionsBuilder.UseSqlite("Data Source=MedicalMask.db");
        }
    }
}