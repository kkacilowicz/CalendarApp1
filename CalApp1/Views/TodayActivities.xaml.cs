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
        public ObservableCollection<HabitEvent> habitEvents; //2 kolekcje, odpowiedzialne za nawyki i eventy są wyniesione jako pola klasowe
        public ObservableCollection<Event> events;//bo chcę żeby były widoczne we wszystkich poniższych metodach klasowych

        public TodayActivities()//konstruktor
        {
            Messenger.Default.Register<CalendarDate>(this, this.ShowActivitiesForSpecifiedDay);//odbieram wiadomość, czyli rejestruję że przyjdzie do mnie obiekt typu 
            //CalendarDate, this cyli kontekst jako pierwszy parametr
            InitializeComponent();
        }

       


        private void ShowActivitiesForSpecifiedDay(CalendarDate calDay)//metoda obsługująca przyjście tej daty, jako parametr przyjmuje obiekt typu CalendarDate 
        {
            using (var dbContext = new Interactive_calendarDbContext())//odpalamy bazę danych
            {
                var habitsQuery = dbContext.HabitEvents//tworzymy querkę w której będziemy szukać wszystkich wystąpień nawyków których data startowa jest równa
                    //dacie obiektu który do nas przyszedł
                                    .Where(h => DateTime.Compare(h.DateStart, calDay.CalendarDay) == 0);
                habitEvents = new ObservableCollection<HabitEvent>(habitsQuery);//tworzymy nowy obiekt kolekcji HabitEvents i przypisujemy go do zmiennej, będzie on przechowywał wystąpienia nawyków

                var eventsQuery = dbContext.Events//analogicznie tylko z eventami
                                    .Where(e => DateTime.Compare(e.DateStart, calDay.CalendarDay) == 0);
                events = new ObservableCollection<Event>(eventsQuery);
                habitsDataGrid.ItemsSource = habitEvents;
                eventsDataGrid.ItemsSource = events;

            }

        }

        private void confirmDoneBtn_Click(object sender, RoutedEventArgs e)//funkcja obsługująca przycisk ConfirmDone
        {
            ObservableCollection<HabitEvent> doneHabits = new ObservableCollection<HabitEvent>();//tworzymy pustą kolekcję która będzie przechowywać wystąpienia nawyków
            //które mają zaznaczonego checkboxa
            ObservableCollection<HabitEvent> notDoneHabits = new ObservableCollection<HabitEvent>();
            using (var dbContext = new Interactive_calendarDbContext())//otwarcie bazy danych
            {
                foreach (HabitEvent habitItem in habitsDataGrid.ItemsSource)//przechodzimy po każdym wystąpieniu nawyku z naszego datagrida ItemsSource czyli wywołaniu
                    //kolekcji wystąpień naywku, która jest aktualnie załadowana do dataGrida
                {
                    if (((CheckBox)DoneStatusColumn.GetCellContent(habitItem)).IsChecked == true)//bierzemy kolumnę z checkboxami, z niej jedną komórkę i sprawdzamy czy 
                        //jest checked
                    {
                        doneHabits.Add(habitItem);//to to wystąpienie nawyku wrzucamy do kolekcji doneHabits
                    }
                    else
                    {
                        notDoneHabits.Add(habitItem);// jeŻeli nie to do kolekcji notDoneHabits
                    }

                }
                dbContext.UpdateRange(notDoneHabits);//odpalamy kontekst bazodanowy i updateujemy kolekcję obiektów 
                dbContext.UpdateRange(doneHabits);
                dbContext.SaveChanges();
                MessageBox.Show("Done habits confirmed");
            }
               
        }

    }

 
}
