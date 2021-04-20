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

        //private readonly Interactive_calendarDbContext _context;
        Interactive_calendarDbContext context;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnToDo_Click(object sender, RoutedEventArgs e)
        {

            var newEvent = new Event()
            {
                Name = "nowy evencik",
                Description = "opis",
                DateStart = new DateTime(2021, 5, 1, 8, 30, 52),
                DateEnd = new DateTime(2021, 5, 2, 8, 30, 52),
                UserId = 1,
                
            };
            //context.Events.Add(newEvent);
            //context.SaveChanges();
           
        }

        private void events_btnClicked(object sender, RoutedEventArgs e)
        {
            DataContext = new EventsViewModel();
            

        }

        private void calendarMenu_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
             EventsViewModel calendarEvent = new EventsViewModel();
             Messenger.Default.Send<DateTime>(calendarMenu.SelectedDate.Value);
             Messenger.Default.Send<string>("test");
             DataContext = calendarEvent;
             


        }

        private void habitStatistics_Click(object sender, RoutedEventArgs e)
        {
            EventsViewModel calendarEvent = new EventsViewModel();
            Messenger.Default.Send<DateTime>(new DateTime(2021,5,2,8,30,52));
            DataContext = calendarEvent;
        }

        private void habitCreatorBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new RedScreenModel();
        }
    }
}

