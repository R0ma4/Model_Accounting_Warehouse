using Model_Accounting_Warehouse.Modul;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Model_Accounting_Warehouse.Page_Prog
{
    /// <summary>
    /// Логика взаимодействия для PageLoad.xaml
    /// </summary>
    public partial class PageLoad : Page
    {
        private DispatcherTimer Updater;
        //Modul.ConfigurationReder configurationReder = new Modul.ConfigurationReder();
        public PageLoad()
        {
            InitializeComponent();

            Updater = new DispatcherTimer();
            Updater.Interval = TimeSpan.FromMilliseconds(100);
            Updater.Tick += UpdateEditor;
            Updater.Start();
            //configurationReder.ProgressBarStaber = ProgressBarStaber;
        }
        // Так-как есть PageLoad и иные Page, нужно менять их в MainWindow
        // Классы для хранения конфигурации




        private void UpdateEditor(object sender, EventArgs e)
        {
            
        }

       

        
    }
}
