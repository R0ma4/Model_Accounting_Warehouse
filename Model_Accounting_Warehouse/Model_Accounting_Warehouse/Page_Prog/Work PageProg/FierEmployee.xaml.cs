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

namespace Model_Accounting_Warehouse.Page_Prog.Work_PageProg
{
    /// <summary>
    /// Логика взаимодействия для FierEmployee.xaml
    /// </summary>
    public partial class FierEmployee : Page
    {
        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE API_MENEGER_DATABASE = new Modul.API_MENEGER_DATABASE(main.Name_Server);
        public FierEmployee()
        {
            InitializeComponent();

        }

        private void FrieEmployee_Click(object sender, RoutedEventArgs e)
        {

            if (API_MENEGER_DATABASE != null)
            {

                if (TypeFrie.SelectedIndex != 0)
                {

                    int key = API_MENEGER_DATABASE.Fire(DocementNomer.Text);

                    switch (key)
                    {
                        case 1:
                            MessageBox.Show("Успешно уволен!","Успешно!",MessageBoxButton.OK,MessageBoxImage.Information);
                            break;
                        case 0:
                            MessageBox.Show("Повторите попытку позже", "Отказанно", MessageBoxButton.OK, MessageBoxImage.Warning);
                            break;
                        case -1:
                            MessageBox.Show("Не верно введён паспорт!", "Отказанно", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        default:
                            MessageBox.Show(key.ToString(), "Отказанно", MessageBoxButton.OK, MessageBoxImage.Error);
                            break ;
                    }
                }
            }
        }
    }
}
