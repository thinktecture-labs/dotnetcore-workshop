using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.EfModel
{

    public class Person
    {
        public Guid Id { get; set; }

        [Required]
        public string Vorname { get; set; }
        
        [Required]
        public string Name { get; set; }

       
        public string Alter { get; set; }
    }

    public class Adresse
    {
        public Guid Id { get; set; }

        public string Street { get; set; }
    }

    public class MyContext: DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Adresse> Addresses { get; set; }

        public MyContext(DbContextOptions<MyContext> options): base(options)
        {
                
        }
        
    }
}