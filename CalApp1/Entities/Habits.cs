using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Interactive_calendar.Entities
{
    public class Habit
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        public int AmountPlannedInLastMonth { get; set; }
        public int AmountDoneInLastMonth { get; set; }
        public int SuccessRate { get; set; }

    }
}
