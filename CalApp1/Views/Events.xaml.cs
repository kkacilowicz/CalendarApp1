using CalApp1.Entities;
using CalApp1.Services;
using CalApp1.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Interactive_calendar.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalApp1.Views
{
    /// <summary>
    /// Logika interakcji dla klasy Events.xaml
    /// </summary>
    public partial class Events : UserControl
    {
        private GoogleCalendarService _googleCalendarService;

        /// <summary>
        /// Class which services the Events.xaml view
        /// </summary>
        public Events()
        {
            _googleCalendarService = new GoogleCalendarService();
           
            InitializeComponent();
        }

        /// <summary>
        /// Function compares date chosen by user and starting date of event and displays list of matching events
        /// </summary>
        /// <param name="calDay"> - contains date chosen by user in MainWindow's calendar</param>
        public void ShowCalendarDayEvents(CalendarDate calDay)
        {
            ObservableCollection<Event> evCollection; 
            using (var dbContext = new Interactive_calendarDbContext()) 
            {
               
                var query = dbContext.Events 
                                .Where(s => DateTime.Compare(s.DateStart, calDay.CalendarDay) == 0);

                evCollection = new ObservableCollection<Event>(query);
                eventsDataGrid.ItemsSource = evCollection; 

            }
        }

        /// <summary>
        /// Function services button Add Event, creates new Event object and adds it to data base
        /// </summary>
        private void btnAddNewEvent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Interactive_calendarDbContext())
            {
                try 
                {
                    var newEvent = new Event()
                    {
                        Name = nameTextBox.Text,
                        Description = descriptionTextBox.Text,
                        DateStart = startDatePicker.SelectedDate.Value,
                        DateEnd = endDatePicker.SelectedDate.Value,
                        UserId = 1,

                    };
                    context.Events.Add(newEvent);
                    context.SaveChanges();
                    MessageBox.Show("New Event Added to Db!");
                    
                    var AddEventtoGoogleCalendar = _googleCalendarService.InsertEvent(newEvent.Name, 
                        newEvent.Description, newEvent.DateStart, newEvent.DateEnd);

                    if (AddEventtoGoogleCalendar)
                    {
                        MessageBox.Show("New Event Added to Google Calendar API!");
                    }
                    else
                    {
                        MessageBox.Show("Couldn't load event to Google Calendar API ", "Something went wrong");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Check all the forms if they are filled properly ", "Something went wrong");
                }
            }
        }

        private void eventsDataGrid_Initialized(object sender, EventArgs e)
        {
            
            
        }

        /// <summary>
        /// Function services button Delete Event, which deletes event from data base
        /// </summary>
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Event takenEvent = (Event)eventsDataGrid.SelectedItem;
                using (var dbContext = new Interactive_calendarDbContext())
                {
                    dbContext.Events.Remove(takenEvent);
                    dbContext.SaveChanges();
                }
                eventsDataGrid.ItemsSource = GetSpcifiedEvents(takenEvent.DateStart);

                MessageBox.Show("Event deleted from Db!");

                var DeleteEventtoGoogleCalendar = _googleCalendarService.DeleteEvent(takenEvent.Name,
                    takenEvent.DateStart);

                if (DeleteEventtoGoogleCalendar)
                {
                    MessageBox.Show("Event sucessfully deleted from Google Calendar API!");
                }
                else
                {
                    MessageBox.Show("Couldn't delete event from Google Calendar API ", "Something went wrong");
                }


            }
            catch(Exception exc)
            {
                MessageBox.Show("No event selected!");
            }
           

        }

        /// <summary>
        /// Function services button Show specified Events, which displays events of specified starting date
        /// </summary>
        private void specifiedEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            eventsDataGrid.ItemsSource = GetSpcifiedEvents(eventsCalendar.SelectedDate.Value);
        }

        /// <summary>
        /// Function compares starting date chosen by user and starting dates of events in data base and returns list of matching events
        /// </summary>
        /// <param name="startDate"> - contains events starting date choosen by user in Events view's Calendar</param>
        private ObservableCollection<Event> GetSpcifiedEvents(DateTime startDate)
        {
            ObservableCollection<Event> events;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.Events
                    .Where(e => e.DateStart == startDate);
                events = new ObservableCollection<Event>(query);
            }
            return events;
        }

        /// <summary>
        /// Function services button Show all Events, which displays all events from database
        /// </summary>
        private void allEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Event> events;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.Events;
                events = new ObservableCollection<Event>(query);
            }
            eventsDataGrid.ItemsSource = events;

        }

     
    }
}
