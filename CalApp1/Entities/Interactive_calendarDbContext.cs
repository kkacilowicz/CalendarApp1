//Definicja bazy danych i jej ustawień
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Interactive_calendar.Entities
{
    public class Interactive_calendarDbContext : DbContext
    {
        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=Interactive-calendarDb;Trusted_Connection=True;";
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Habit> Habits { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(U => U.Name) //kolumna Name 
                .IsRequired() //jest wymagana
                .HasMaxLength(25); //długość kolumny name


            modelBuilder.Entity<Habit>()
                .Property(H => H.Name)
                .IsRequired(); //kolumna Name jest wymagana

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
