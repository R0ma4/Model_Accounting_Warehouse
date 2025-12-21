using System;
using System.Collections.Generic;
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

namespace Model_Accounting_Warehouse.Page_Prog
{



    /// <summary>
    /// Логика взаимодействия для PageWork.xaml
    /// </summary>
    public partial class PageWork : Page
    {


        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE dATABASEAPI = new Modul.API_MENEGER_DATABASE(main.Name_Server);
        public PageWork()
        {
            InitializeComponent();
       
        }


      

        private void ClickButtonEvert(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            MessageBox.Show(menuItem.Name);

            switch (menuItem.Name) 
            {
                case "ExitProgramm":
                    // main.PageControl.Content = main.mainpageMM;
                    break;
                case "UpdateProduct":
                    WorkPage.Content = main.tableProduct;
                    break;
                case "RedactProduct":
                    ModulWindowRedact.RedactPage redactPage = new ModulWindowRedact.RedactPage();
                    redactPage.ShowDialog();
                    break;
                case "AddEmployeeInDataBase":
                     WorkPage.Content = main.pageAddUser;
                    break;
                case "CreateNewProduct":
                   WorkPage.Content = main.pageEdit;
                    break;
                case "FaierEmployeeInDataBase":
                    WorkPage.Content = main.pageFierEmployee;
                    break;
                default:
                   WorkPage.Content = main.page404Error;
                    break;
            }
        }
    }
}
