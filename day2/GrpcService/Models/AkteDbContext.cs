using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GrpcService.Models
{
    public class AkteDbContext: DbContext
    {
        public AkteDbContext(DbContextOptions<AkteDbContext> options): base(options)
        {
                
        }
        
        public DbSet<Akte> Akten { get; set; }
        
        public DbSet<Vorgang> Vorgaenge { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Akte>().HasKey(a => a.AktenNummer);
            modelBuilder.Entity<Akte>().HasIndex(a => a.Name);
            
            modelBuilder.Entity<Vorgang>().HasKey(a => a.VorangsNummer);
               
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
           
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is Akte a)
                {
                    a.CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }

    public class Akte
    {
        public Guid AktenNummer { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Vorgang> Vorgaenge { get; set; } = new List<Vorgang>();
    }

    public class Vorgang
    {
        public Guid VorangsNummer { get; set; }
        
        public Guid AktenNummer { get; set; }

        public Akte Akte { get; set; }
        
        public string Name { get; set; }

    }
}