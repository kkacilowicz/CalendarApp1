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
            habitsComboBox.ItemsSource = returnHabits();//wiązanie wszystkich nawyków z ComboBoxem, czyli rozwijaną listą zawierającą nawyki z bazy danych
        }

        private void addHabitButton_Click(object sender, RoutedEventArgs e)//funkcja do dodawania nowego nawyku
        {
            using (var dbContext = new Interactive_calendarDbContext())//odwołanie do bazy danych
            {
                String tmpHabitName = habitNameTextBox.Text;

                if (tmpHabitName == "")//zabezpieczenie przed próbą utworzenia nawyku bez nazwy
                {
                    MessageBox.Show("Please type in habit's name!");
                }
                else
                {
                    Habit newHabit = new Habit()//tworzymy nowy nawyk
                    {
                        Name = habitNameTextBox.Text// jego nazwa jest zasysana z TextBoxa
                    };
                    dbContext.Habits.Add(newHabit);//dodaję nawyk do bazy danych 
                    dbContext.SaveChanges();//commituję zmiany
                    MessageBox.Show("New habit added!");

                }


            }
            habitsComboBox.ItemsSource = returnHabits();//dodajemy nowy nawyk do listy w ComboBoxie, aby była ona widoczna
                                                        //od razu i nie trzebabyło odświeżać aplikacji

        }


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
        //returnHabits i returnHabitEvents to funkcje odwołujące się do bazy danych
        private ObservableCollection<HabitEvent> returnHabitEvents()
        {
            ObservableCollection<HabitEvent> events;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                var query = dbContext.HabitEvents;
                events = new ObservableCollection<HabitEvent>(query);
                return events;//zwraca listę wszystkich wystąpień nawyków

            }
        }


        private void addHabitEventBtn_Click(object sender, RoutedEventArgs e)//funkcja dodająca eventy dla konkretnego nawyku
        {
            using (var dbContext = new Interactive_calendarDbContext())
            {
                ObservableCollection<DateTime> calendarSelectedDates = habitCalendar.SelectedDates;//tworzę kolekcję typu DateTime do której przypisuję wszystkie
                                                                                                   //wybrane daty z kalendarza 
                Habit tmpHabit = (Habit)habitsComboBox.SelectedItem; //pobieram nawyk który jest wybrany z ComboBoxa, konwertuję go na typ Habit  przypisuję do
                                                                     //zmiennej
                if (tmpHabit == null || weeksAheadTextBox.Text == "")//sprawdzamy czy którykolwiek nawyk został wybrany z comboBoxaoraz czy cokolwiek zostało wpisane
                                                                     //w textBoxa z ilością tygodni powtarzania nawyku
                {
                    MessageBox.Show("Check whether you chose the habit and specified number of weeks");
                }else if (calendarSelectedDates.Count == 0)//sprawdzamy czy lista dni tygodnia nie jest równa 0
                {
                    MessageBox.Show("Select Dates using calendar");
                }
                else
                {
                    foreach (DateTime singleDate in calendarSelectedDates)//pętla foreach dla każdej daty z wybranych generuje wystąpienia nawyku w te same dni
                                                                          //tygodnia co zaznaczona data 
                    {
                        for (int i = 0; i < Int32.Parse(weeksAheadTextBox.Text); i++)//pętla przechodzi tyle razy ile wpisano tygodni
                        {
                            HabitEvent newHabitEvent = new HabitEvent()//tworzymy nowe wystąpienie nawyku
                            {
                                Name = tmpHabit.Name,//jego nazwa jest taka sama jak odpowiadającego mu nawyku
                                Done = false,//oznaczamy go jako niewykonany
                                DateStart = singleDate.AddDays(i * 7),//datą startową jest kolejne wystąpienie daty z naszej tablicy dat i do każdej daty
                                                                      //dodaje licznik * 7 dni
                                HabitId = tmpHabit.Id//id nawyku którego dotyczy dany event
                            };
                            dbContext.HabitEvents.Add(newHabitEvent);//stworzony wyżej pojedynczy obiekt dodajemy do bazy dannych (listy bazodanowej)
                        }
                    }
                    dbContext.SaveChanges();//robimy commita na bazie danych
                    MessageBox.Show("Habit Events Added");//komunikat że wystąpienia nawyków zostały zapisane
                }
                
            }

        }

        private void showHabitEventsBtn_Click(object sender, RoutedEventArgs e)//funkcja służąca do wyświetlania wszystkich wystąpień eventów
        {
            habitsGridView.ItemsSource = returnHabitEvents();
        }

        private void showSpecifiedEventsBtn_Click(object sender, RoutedEventArgs e)//wyświetla wszystkie eventy wybranego nawyku
        {
            ObservableCollection<HabitEvent> events;//tworzę kolekcję do której będę zapisywać wszystkie wyniki które zostaną zwrócone z bazy danych
            Habit tmpHabit = (Habit)habitsComboBox.SelectedItem;//pobieramy wybrany nawyk z comboBoxa
            if (tmpHabit == null)//sprawdzamy czy nawyk został w ogóle wybrany
            {
                MessageBox.Show("Choose the habit");
            }
            else
            {
                using (var dbContext = new Interactive_calendarDbContext())//odpalamy bazę danych
                {
                    var query = dbContext.HabitEvents //piszemy query że chcemy wyszukać wszystkie wystąpienia nawyków, ale wtedy gdy w naszym obiekcie HabitEvents
                        .Where(e => e.HabitId == tmpHabit.Id);//pole HabitId będzie odpowiadało Id nawyku wybranego w comboBoxie
                    events = new ObservableCollection<HabitEvent>(query);//tak pobraną querkę zapisujemy do naszej kolekcji
                    habitsGridView.ItemsSource = events;
                }
            }

        }

        private void deleteHabitBtn_Click(object sender, RoutedEventArgs e)//usunięcie wybranego nawyku
        {
            Habit toRemoveHabit = (Habit)habitsComboBox.SelectedItem;//pobieramy z comboboxa wybrany nawyk i konwertujemy go na typ Habit
            if (toRemoveHabit == null)//sprawdzamy czy nawyk został wybrany w ComboBoxie
            {
                MessageBox.Show("Choose the habit");
            }
            else
            {
                ObservableCollection<HabitEvent> eventsToRemove;//tworzymy kolekcję
                using (var dbContext = new Interactive_calendarDbContext())//odpalenie bazy
                {
                    var query = dbContext.HabitEvents//piszemy query że chcemy wyszukać wszystkie wystąpienia nawyków, ale wtedy gdy w naszym obiekcie HabitEvents
                        .Where(e => e.HabitId == toRemoveHabit.Id);//pole HabitId będzie odpowiadało Id nawyku wybranego w comboBoxie
                    eventsToRemove = new ObservableCollection<HabitEvent>(query);//na podstawie powyższej querki tworzymy nową kolekcję
                    dbContext.HabitEvents.RemoveRange(eventsToRemove);//odpalamy dbContext czyli obiekt dzięki któremu mogędziałać na bazach danych, będziemy działać
                                                                      //na kolekcji HabitEvents, metoda RemoveRange pozwala usunąć wiele obiektów, jako jej parapetr
                                                                      //można przekazać całą kolekcję, co tu też robimy
                    dbContext.Remove(toRemoveHabit);//remove służy do usunięcia pojedynczego nawyku (tego pobranego z comboBoxa)
                    dbContext.SaveChanges();//robimy commita na bazie danych
                }
                MessageBox.Show("Chosen habit with corresponding events has been deleted!");//komunikat że nawyk i jego wystąpienia zostały usunięte
                habitsComboBox.ItemsSource = returnHabits();// aktualizujemy naszego comboboxa
                habitsGridView.ItemsSource = returnHabitEvents();

            }
        }
    }

}

