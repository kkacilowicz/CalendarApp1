using System;
using System.Collections.Generic;
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
        public string Amount { get; set; }
        public string SuccessRate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
