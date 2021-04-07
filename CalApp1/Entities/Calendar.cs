using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interactive_calendar.Entities
{
    public class Calendar
    {
        public int Id { get; set; }
        public virtual List<Event> Events { get; set; }

        public string Name { get; set; }
    }
}