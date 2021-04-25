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
        
        public Events()
        {
            /*ObservableCollection<Event> evCollection;
            using (var dbContext = new Interactive_calendarDbContext())
            {  
                var query = dbContext.Events
                                .Where(s => s.Name == "nowy evencik");
                evCollection = new ObservableCollection<Event>(query);
                eventsDataGrid.DataContext = evCollection;
                
            }*/

            InitializeComponent();
           
        }

        private void btnAddNewEvent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Interactive_calendarDbContext())
            {
                try
                {
                    var newEvent = new Event()
                    {
                        Name = nameTextBox.Text,
                        Description = descriptionTextBox.Text,
                        DateStart = startDatePicker.SelectedDate.Value,
                        DateEnd = endDatePicker.SelectedDate.Value,
                        UserId = Int32.Parse(userIdTextBox.Text),

                    };
                    context.Events.Add(newEvent);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Check all the forms if they are filled properly ", "Something went wrong");
                }
            }

             
        }
        private void eventsDataGrid_Initialized(object sender, EventArgs e)
        {
            ObservableCollection<Event> evCollection;
            using (var dbContext = new Interactive_calendarDbContext())
            {
                //DateTime today = DateTime.Today;
                var query = dbContext.Events;
                    //.Where(s => s.Name == "nowy evencik");
                    //.Where(s => s.DateStart >= today && s.DateStart < today.AddDays(7));

                evCollection = new ObservableCollection<Event>(query);
                eventsDataGrid.DataContext = evCollection;
            }
            Messenger.Default.Register<DateTime>(this, (dateT) =>
            { 
                MessageBox.Show("loaded data: " + dateT);

            });
            
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
