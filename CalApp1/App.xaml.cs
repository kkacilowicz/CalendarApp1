using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CalApp1.Services;
using CalendarApp1;

namespace CalApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
          
            // Create main application window,
            MainWindow mainWindow = new MainWindow();
       
            mainWindow.Show();
        }
    }
}
