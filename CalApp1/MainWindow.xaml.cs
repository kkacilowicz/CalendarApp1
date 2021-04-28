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
            DataContext = new TodayActivitiesViewModel();

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
            DataContext = new HabitsStatisticsViewModel();
        }

        private void habitCreatorBtn_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new HabitsViewModel();
        }

        private void calendarMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)//taka sama funkcja jak wcześniejsze selected dates changed, tylko działa po podwójnum kliknięcu na jakąś datę i wyświela aktywności na dzień dzisiejszy
        {
            TodayActivitiesViewModel todayActivitiesModel = new TodayActivitiesViewModel();//stworzony jest ViewModel do dzisiejszych nawyków i wydarzeń
            Messenger.Default.Send<CalendarDate>(new CalendarDate { CalendarDay = calendarMenu.SelectedDate.Value });//tutaj jest wywoływany jest ten Messanger i w nim przesyłam ten obiekt typu CalendarDate w nawiasie jest to co chcę przesłać, czyli tworzę nowy obiekt i w to pole CalendarDate wrzucam tą naszą datę wybraną z kalendarza
            DataContext = todayActivitiesModel;// ładuję DataContext aby pojawił się widok TodayActivities
        }
    }
}

