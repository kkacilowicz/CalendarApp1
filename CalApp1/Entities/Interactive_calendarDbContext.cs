//Definicja bazy danych i jej ustawień
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interactive_calendar.Entities
{
    public class Interactive_calendarDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=Interactive-calendarDb;Trusted_Connection=True;";
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitEvent> HabitEvents { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
            modelBuilder.Entity<Habit>()
                .Property(H => H.Name)
                .IsRequired(); 

            modelBuilder.Entity<Event>()
                .Property(E => E.Name)
                .IsRequired();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
