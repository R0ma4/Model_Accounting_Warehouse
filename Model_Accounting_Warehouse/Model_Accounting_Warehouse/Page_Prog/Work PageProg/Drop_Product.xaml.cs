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
using System.Windows.Shapes;

namespace Model_Accounting_Warehouse.Page_Prog.Work_PageProg
{
    /// <summary>
    /// Логика взаимодействия для Drop_Product.xaml
    /// </summary>
    public partial class Drop_Product : Window
    {
        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE API_MENEGER_DATABASE = new Modul.API_MENEGER_DATABASE(main.Name_Server);
        public Drop_Product()
        {
            InitializeComponent();
            API_MENEGER_DATABASE.GetProductsInStock(main.ID_WareHouse);
        }
    }
}
