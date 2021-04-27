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
        public Events()//klasa obsługująca widok Events
        {
            /*
            using (var dbContext = new Interactive_calendarDbContext())
            {
                User tempUser = new User()
                {
                    Email = "Ania"
                };
                dbContext.Users.Add(tempUser);
                dbContext.SaveChanges();
            }
            */
            Messenger.Default.Register<CalendarDate>(this, this.ShowCalendarDayEvents); //odebranie danych (klikniętej daty) z MainWindow
                                                                                        //this jest chyba od kontekstu, nie jestem pewna, ale musi tutaj być
                                                                                        //evDate to parametr który przyszedł (typu DateTime)
                                                                                        //=> oznacza że to coś podobnego jak wyrażenia lambda w Javie, czyli ten poniższy blok jest wywołany w argumencie tej funkcji
                                                                                        //dostaję daną (tutaj evDate) i mogę ją przerobić i dopiero funkcja się wykonuje
            InitializeComponent();//wyświetla widok, ale w tym konstruktorze Events można też coś doimplementować żeby wyświetlało się przy starcie widoku, np żeby już ładowały się tam dane
        }

        public void ShowCalendarDayEvents(CalendarDate calDay)
        {
            ObservableCollection<Event> evCollection; //lista typu ObservableCollection, bo ten typ jest zalecany do DataGridów, przechowująca typ Event
            using (var dbContext = new Interactive_calendarDbContext()) //using otwierający bazę danych, na której będziemy działać 
            {
               MessageBox.Show("loaded data: " + calDay.CalendarDay);//sprawdzanie czy data przychodzi z MainWindow
                var query = dbContext.Events //tworzę quekę sqlową typu var, bierzemy instancję bazy danych, chcemy działać na naszej kolekcji Eventów
                                .Where(s => DateTime.Compare(s.DateStart, calDay.CalendarDay) == 0);//to nasz condition statement, co chcemy wyłuskać na podstawie czego
                                                                                                    //argument s to pojedynczy event z całej kolekcji eventów(z racji że .Where zostało wywołane na całej kolekcji to wnioskuje ono że s jest jednym eventem), po strzałce znajduje się porównanie dat przy pomocy metody Compare, porównuję datę startową eventu wpisanego w Events UI z tą datą która przyszła z kalendarza w MainWindow, ta funkcja zwraca 0 wtedy, kiedy te daty są sobie równe
                                                                                                    //w rezultacie ta querka powinna mi zwrócić te wszystkie eventy, w których data startowa jest taka sama jak kliknięta data w kalendarzu
                evCollection = new ObservableCollection<Event>(query);//do wyżej zdefiniowanej kolekcji w której będziemy przetrzymywać wyniki, przypisujemynowo tworzoną za pomoca konstruktora kolekcję ObservableCollection typu Event i jako argument przyjmuje ona querkę
                                                                      //zwracana w ten sposów lista obiektów jest przypisywana do evCollection
                eventsDataGrid.ItemsSource = evCollection; //do DataContext eventsDataGrid przypisujemy listę, aby wyświetlić tą listę

            }
        }

        private void btnAddNewEvent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Interactive_calendarDbContext())//dostajemy się do bazy danych tworząc zmienną context 
            {//wszystkie operacje na bazach danych warto robić w using, w nawiasie jest to co powinno się otworzyć, ponieważ to co tu otworzymy zamknie się na końcu tego usingu i nie trzeba pamiętać o jej zamykaniu
                try //try catcha używamy żeby zabezpieczyć się przed tym że coś nie zostało wpisane w te labelki poniżej
                {
                    var newEvent = new Event()//dodawanie nowych elementów do bazy danych
                    //tworzę nowy obiekt modelu z folderu Entifies, tutaj Event. W C# można używać zamiast konstruktora takiego bloku

                    {
                        Name = nameTextBox.Text,//nadaję mu wszystkie pola
                        Description = descriptionTextBox.Text,
                        DateStart = startDatePicker.SelectedDate.Value,//zwraca datę typu DateTime
                        DateEnd = endDatePicker.SelectedDate.Value,
                        UserId = Int32.Parse(userIdTextBox.Text),//string z TextBoxa parsuję na int

                    };
                    context.Events.Add(newEvent);//wywołujemy context czyli zmienną za której pomocą będziemy dostawać się do bazy danych, następnie kolekcję Events, pod tą nazwą są wszystkie Eventy, na tej kolekcji wywołuję funkcję Add() która doda nowy element który przyjmuje w argumencie
                    context.SaveChanges();//to taki commit jakby, context - czyli nasza baza danych, wywoływana na niej metoda SaveChanges
                }
                catch (Exception ex)//łapanie wyjątków że jak któreś pole dodawania Eventów nie zostanie uzupełnione to wyświetli się komunikat żeby sprawdzić
                {
                    MessageBox.Show("Check all the forms if they are filled properly ", "Something went wrong");
                }
            }
            /* tutaj się wywala, trochę nie umiem dobrać się do tych danych w fromularzu i wyrzuca NullReference Exception
            var AddToGoogleCalendar = _googleCalendarService.InsertEvent(nameTextBox.Text, descriptionTextBox.Text,
                startDatePicker.SelectedDate.Value, endDatePicker.SelectedDate.Value);

            if (!AddToGoogleCalendar)
            {
                MessageBox.Show("Unfortunately adding event to Google Calendar went wrong");
            }
            else
            {
                MessageBox.Show("Successfully added to Google Calendar");
            }
            */


        }//DataGrid to nasza tabela do wyświetlania danych (tutaj Eventów)
        private void eventsDataGrid_Initialized(object sender, EventArgs e)
        {
            /*ObservableCollection<Event> evCollection; 
            using (var dbContext = new Interactive_calendarDbContext())
            {
                Messenger.Default.Register<DateTime>(this, (dateT) =>
                {
                    //DateTime today = DateTime.Today;
                    var query = dbContext.Events
                        .Where(s => s.DateStart == dateT);

                    evCollection = new ObservableCollection<Event>(query);
                    eventsDataGrid.DataContext = evCollection;


                });
            }*/
            
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Event takenEvent = (Event)eventsDataGrid.SelectedItem;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                dbContext.Events.Remove(takenEvent);
                dbContext.SaveChanges();
            }

        }

        /* private void eventsDataGrid_Loaded(object sender, RoutedEventArgs e)
         {
             ObservableCollection<Event> evCollection;
             using (var dbContext = new Interactive_calendarDbContext())
             { 
                 Messenger.Default.Register<DateTime>(this, (evDate) =>
                 {
                     MessageBox.Show("loaded data: " + evDate);
                     var query = dbContext.Events
                                 .Where(s => DateTime.Compare(s.DateStart,evDate) == 0);

                     evCollection = new ObservableCollection<Event>(query);
                     eventsDataGrid.DataContext = evCollection;
                 });

             }



         }*/
    }
}
