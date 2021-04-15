using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interactive_calendar.Entities;

namespace CalApp1.Entities
{
    public class HabitEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int HabitId { get; set; }
        public virtual Habit Habit { get; set; }
    }
}
