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
            DataContext = new EventsViewModel(); //do kontextu aplikacji okna MainWindow przypisuję model EventsViewModel, dzięki czemu znaczniku Canvas w xamlu wyświetla się View wybranego modelu


        }

        //funkcja wywoływana kiedy zmieni się data na kalendarzu, czyli jak klikniemy na jakiś dzień
        private void calendarMenu_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            EventsViewModel calendarEvent = new EventsViewModel();//po naciśnięciu wybranego dnia w kalendarzu tworzę obiekt typu EventsViewModel, chcę odpalić zakładkę events z UI
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = calendarMenu.SelectedDate.Value});//wysyłam do okna które się zaraz odpali parametry które chcę wysłać, tutaj akurat wysyłam datę która została kliknięta w kalendarzu <CalendarDate>, w nawiasie jest jej wartość
                                                                                                                    
            DataContext = calendarEvent;//aby pokazać to okno przypisuję wyżej stworzony model do tego "okienka" DataContext
                                        //wtedy w MainWindow.xaml jest sprawdzane jaka data przyszła i tam jest tworzony widok danego dnia za pomocą

        }

        private void habitStatistics_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void habitCreatorBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new HabitsViewModel();
        }

        private void calendarMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EventsViewModel calendarEvent = new EventsViewModel();
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = calendarMenu.SelectedDate.Value });
            DataContext = calendarEvent;
        }
    }
}

