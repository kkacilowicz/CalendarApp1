using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Interactive_calendar.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalApp1.ViewModels
{
    public class EventsViewModel
    {
        public DateTime calendarDate;
        
        public EventsViewModel(DateTime calendarDate)
        {
            this.calendarDate = calendarDate;
            
        }

        public EventsViewModel( )
        {
            
        }

       
    }
}
