using CalApp1.Entities;
using CalApp1.ViewModels;
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

namespace CalendarApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
              

        public MainWindow()
        {
            InitializeComponent();
            TodayActivitiesViewModel todayActivitiesModel = new TodayActivitiesViewModel();
            DataContext = todayActivitiesModel;

        }

        /// <summary>
        /// Function services button Events and displays Events view
        /// </summary>
        private void events_btnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new EventsViewModel(); 


        }

        /// <summary>
        /// Function sends date from calendar to new canvas stored window
        /// </summary>
        private void calendarMenu_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            EventsViewModel calendarEvent = new EventsViewModel();
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = calendarMenu.SelectedDate.Value});
                                                                                                                    
            DataContext = calendarEvent;

        }

        /// <summary>
        /// Function services button Habits Statistics and displays HabitsStatistics view
        /// </summary>
        private void habitStatistics_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new HabitsStatisticsViewModel();
        }

        /// <summary>
        /// Function services button Habit creator and displays Habits view
        /// </summary>
        private void habitCreatorBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new HabitsViewModel();
        }

        /// <summary>
        /// Function displays events and habits for chosen date 
        /// </summary>
        private void calendarMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TodayActivitiesViewModel todayActivitiesModel = new TodayActivitiesViewModel();
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = calendarMenu.SelectedDate.Value });
            DataContext = todayActivitiesModel;
        }

        /// <summary>
        /// Function runs after opening MainWindow and sends todays date
        /// </summary>
        private void mainWindowGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = DateTime.Today });
        }
    }
}

