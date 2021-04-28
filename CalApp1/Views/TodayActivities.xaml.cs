using CalApp1.Entities;
using GalaSoft.MvvmLight.Messaging;
using Interactive_calendar.Entities;
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
    /// Logika interakcji dla klasy TodayActivities.xaml
    /// </summary>
    public partial class TodayActivities : UserControl
    {
        public ObservableCollection<HabitEvent> habitEvents; 
        public ObservableCollection<Event> events;

        /// <summary>
        /// Class which services the TodayActivities.xaml view
        /// </summary>
        public TodayActivities()
        {
            InitializeComponent();
            Messenger.Default.Register<CalendarDate>(this, this.ShowActivitiesForSpecifiedDay);
        }

        /// <summary>
        /// Function compares date chosen by user and starting date of event and habit and displays list of matching events and habits
        /// </summary>
        /// <param name="calDay"> - contains date chosen by user in MainWindow's calendar</param>
        private void ShowActivitiesForSpecifiedDay(CalendarDate calDay) 
        {
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var habitsQuery = dbContext.HabitEvents
                                    .Where(h => DateTime.Compare(h.DateStart, calDay.CalendarDay) == 0);
                habitEvents = new ObservableCollection<HabitEvent>(habitsQuery);

                var eventsQuery = dbContext.Events
                                    .Where(e => DateTime.Compare(e.DateStart, calDay.CalendarDay) == 0);
                events = new ObservableCollection<Event>(eventsQuery);
                habitsDataGrid.ItemsSource = habitEvents;
                eventsDataGrid.ItemsSource = events;

            }

        }

        /// <summary>
        /// Function services button Confirm Done, checks checkboxes assigned to events and creates two collections of events done and not done
        /// </summary>
        private void confirmDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<HabitEvent> doneHabits = new ObservableCollection<HabitEvent>();
            ObservableCollection<HabitEvent> notDoneHabits = new ObservableCollection<HabitEvent>();
            using (var dbContext = new Interactive_calendarDbContext())
            {
                foreach (HabitEvent habitItem in habitsDataGrid.ItemsSource)
                {
                    if (((CheckBox)DoneStatusColumn.GetCellContent(habitItem)).IsChecked == true)
                        
                    {
                        doneHabits.Add(habitItem);
                    }
                    else
                    {
                        notDoneHabits.Add(habitItem);
                    }

                }
                dbContext.UpdateRange(notDoneHabits);
                dbContext.UpdateRange(doneHabits);
                dbContext.SaveChanges();
                MessageBox.Show("Done habits confirmed");
            }
               
        }

    }

 
}
