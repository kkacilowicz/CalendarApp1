using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalApp1.Entities;

namespace Interactive_calendar.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual List<Event> Events { get; set; }
        public virtual List<HabitEvent> HabitEvents { get; set; }
    }
}