using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interactive_calendar.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GoogleEmail { get; set; }
        public int CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual List<Habit> Habits { get; set; }
    }
}
