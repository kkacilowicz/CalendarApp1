using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Interactive_calendar.Entities;

namespace CalApp1.Views
{
    /// <summary>
    /// Logika interakcji dla klasy HabitsStatistics.xaml
    /// </summary>
    public partial class HabitsStatistics : UserControl
    {
        /// <summary>
        /// NonParametric constructor to initialize  
        /// </summary>
        public HabitsStatistics()
        {
            InitializeComponent();
            habitsComboBox.ItemsSource = returnHabits();
            
        }
        /// <summary>
        /// Returns a collection of all Habit types from database  
        /// </summary>
        private ObservableCollection<Habit> returnHabits()
        {
            ObservableCollection<Habit> habitsCol;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.Habits;
                habitsCol = new ObservableCollection<Habit>(query);
                return habitsCol;//zwraca listę wszystkich nawyków które są w bazie danych

            }
        }
        /// <summary>
        /// Returns all HabitEvent with Done status occurences in last month  
        /// </summary>
        /// <param name="HabitId"> - contains info about the HabitType </param>
        private int GetDoneHabitEventsFromLastMonth(int HabitId)
        {
            
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var Today = DateTime.Now;
                var TodayMinusMonth = Today.AddMonths(-1);

                var ListOfDoneEventsPastMonth =  dbContext
                    .HabitEvents
                    .Where(c => c.DateStart <= Today)
                    .Where(c=>c.DateStart>TodayMinusMonth)
                    .Where(d=>d.HabitId == HabitId)
                    .Where(g=>g.Done==true)
                    .ToList();

                if (ListOfDoneEventsPastMonth == null)
                    return 0;
                else
                    return ListOfDoneEventsPastMonth.Count();

            }
        }

        /// <summary>
        /// Returns all HabitEvent with UnDone status occurences in last month  
        /// </summary>
        /// <param name="HabitId"> - contains info about the HabitType </param>
        private int GetUndoneHabitEventsFromLastMonth(int HabitId)
        {

            using (var dbContext = new Interactive_calendarDbContext())
            {
                var Today = DateTime.Now;
                var TodayMinusMonth = Today.AddMonths(-1);

                var ListOfDoneEventsPastMonth = dbContext
                    .HabitEvents
                    .Where(c => c.DateStart <= Today)
                    .Where(c => c.DateStart > TodayMinusMonth)
                    .Where(d => d.HabitId == HabitId)
                    .Where(g => g.Done == false)
                    .ToList();

                if (ListOfDoneEventsPastMonth == null)
                    return 0;
                else
                    return ListOfDoneEventsPastMonth.Count();

            }
        }




        /*
        public ObservableCollection<KeyValuePair<string, int>> DataCollection
        {
            var Data = new ObservableCollection<KeyValuePair<string, int>>
            {
                new KeyValuePair<string,int> ("Test1", 10),
                new KeyValuePair<string,int> ("Test2", 5),
            };

            return Data;
        }
        */

    }
}
