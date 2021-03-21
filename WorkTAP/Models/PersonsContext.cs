using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WorkTAP.Models
{
    public class PersonsContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public PersonsContext(DbContextOptions<PersonsContext> options)
               : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasKey("PersonId", "Name");


            modelBuilder.Entity<Person>()
                .HasData(
                new Person { Id = 1, Name = "Nikita", DisplayName = "Nik" },
                new Person { Id = 2, Name = "Alexandr", DisplayName = "Habib" }
                );
            modelBuilder.Entity<Skill>()
                .HasData(
                new { Name = "C#", PersonId = 1, Level = (byte)6 },
                new { Name = "PHP", PersonId = 2, Level = (byte)3 }
                 );
        }
    }
}
