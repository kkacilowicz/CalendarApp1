using CalApp1.Entities;
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
    /// Logika interakcji dla klasy Habits.xaml
    /// </summary>
    public partial class Habits : UserControl
    {
        public Habits()
        {
            InitializeComponent();
            habitsComboBox.ItemsSource = returnHabits();
        }

        private void addHabitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Interactive_calendarDbContext())
            {
                Habit newHabit = new Habit()
                {
                    Name = habitNameTextBox.Text
                };
                dbContext.Habits.Add(newHabit);
                dbContext.SaveChanges();
                

            }
            habitsComboBox.ItemsSource = returnHabits();

        }


        private ObservableCollection<Habit> returnHabits()
        {
            ObservableCollection<Habit> habitsCol;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.Habits;
                habitsCol = new ObservableCollection<Habit>(query);
                return habitsCol;

            }
        }

        private ObservableCollection<HabitEvent> returnHabitEvents()
        {
            ObservableCollection<HabitEvent> events;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.HabitEvents;
                events = new ObservableCollection<HabitEvent>(query);
                return events;

            }
        }


        private void addHabitEventBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new Interactive_calendarDbContext())
            {
                ObservableCollection<DateTime> calendarSelectedDates = habitCalendar.SelectedDates;
                Habit tmpHabit = (Habit)habitsComboBox.SelectedItem;
                if(tmpHabit == null || weeksAheadTextBox.Text == "")
                {
                    MessageBox.Show("Check whether you chose the habit and specified number of weeks");
                }else if (calendarSelectedDates.Count == 0)
                {
                    MessageBox.Show("Select Dates using calendar");
                }
                else
                {
                    foreach (DateTime singleDate in calendarSelectedDates)
                    {
                        for (int i = 0; i < Int32.Parse(weeksAheadTextBox.Text); i++)
                        {
                            HabitEvent newHabitEvent = new HabitEvent()
                            {
                                Name = tmpHabit.Name,
                                Done = false,
                                DateStart = singleDate.AddDays(i * 7),
                                HabitId = tmpHabit.Id
                            };
                            dbContext.HabitEvents.Add(newHabitEvent);
                        }
                    }
                    dbContext.SaveChanges();
                    MessageBox.Show("Habit Events Added");
                }
                
            }

        }

        private void showHabitEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            habitsGridView.ItemsSource = returnHabitEvents();
        }

        private void showSpecifiedEventsBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<HabitEvent> events;
            Habit tmpHabit = (Habit)habitsComboBox.SelectedItem;
            if (tmpHabit == null)
            {
                MessageBox.Show("Choose the habit");
            }
            else
            {
                using (var dbContext = new Interactive_calendarDbContext())
                {
                    var query = dbContext.HabitEvents
                        .Where(e => e.HabitId == tmpHabit.Id);
                    events = new ObservableCollection<HabitEvent>(query);
                    habitsGridView.ItemsSource = events;
                }
            }

        }

        private void deleteHabitBtn_Click(object sender, RoutedEventArgs e)
        {
            Habit toRemoveHabit = (Habit)habitsComboBox.SelectedItem;
            if (toRemoveHabit == null)
            {
                MessageBox.Show("Choose the habit");
            }
            else
            {
                ObservableCollection<HabitEvent> eventsToRemove;
                using (var dbContext = new Interactive_calendarDbContext())
                {
                    var query = dbContext.HabitEvents
                        .Where(e => e.HabitId == toRemoveHabit.Id);
                    eventsToRemove = new ObservableCollection<HabitEvent>(query);
                    dbContext.HabitEvents.RemoveRange(eventsToRemove);
                    dbContext.Remove(toRemoveHabit);
                    dbContext.SaveChanges();
                }
                MessageBox.Show("Chosen habit with corresponding events has been deleted!");
                habitsComboBox.ItemsSource = returnHabits();
                habitsGridView.ItemsSource = returnHabitEvents();

            }
        }
    }

}

