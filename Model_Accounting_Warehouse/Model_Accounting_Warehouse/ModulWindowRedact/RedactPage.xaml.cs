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

namespace Model_Accounting_Warehouse.ModulWindowRedact
{
    /// <summary>
    /// Логика взаимодействия для RedactPage.xaml
    /// </summary>
    public partial class RedactPage : Window
    {
        public MainWindow main = Application.Current.MainWindow as MainWindow;
        public RedactPage()
        {
            InitializeComponent();
        }

        private void RedactProduct(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(CodeProduct.Text, out int CProd))
            {
                main.pageEdit.
                 = main.pageEdit;
            }
        }
    }
}
