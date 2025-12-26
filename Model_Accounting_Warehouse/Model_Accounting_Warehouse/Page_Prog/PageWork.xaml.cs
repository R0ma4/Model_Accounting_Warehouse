using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

using Xceed.Document.NET;
using Xceed.Words.NET;
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

        void DOX_Import() 
        {
           int Key = dATABASEAPI.DoxOperation(1);
            switch (Key) 
            {
                case 1:
                    MessageBox.Show("Отчёт был создан","Завершенно!",MessageBoxButton.OK,MessageBoxImage.Information);
                    break;
                case -1:
                    MessageBox.Show("При создании отчёта, произошла ошибка.\nИзучите логированние", "Отказанно!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
      

        private void ClickButtonEvert(object sender, RoutedEventArgs e)
        {

            MenuItem menuItem = (MenuItem)sender;
            MessageBox.Show(menuItem.Name);
            
            switch (menuItem.Name)
            {
                case "ImportFile":
                    
                    DOX_Import();
                    break;
                case "ExitProgramm":
                    main.PageControl.Content = main.pageMainMenu;
                    break;
                case "UpdateProduct":
                    WorkPage.Content = main.tableProduct;
                    break;
                case "DropProduct":
                    main.pageDropProduct.Show();
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
