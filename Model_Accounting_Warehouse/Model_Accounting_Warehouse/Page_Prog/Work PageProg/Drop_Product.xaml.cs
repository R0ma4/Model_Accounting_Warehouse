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
       
        string Product_Name = "";
        string ID_Product = "";

        public Drop_Product()
        {
            InitializeComponent();
            ComboBoxDropProduct.Items.Clear();

            var products = API_MENEGER_DATABASE.GetProductsInStock(main.ID_WareHouse);
            Console.WriteLine($"Всего продуктов: {products.Count}");

            foreach (var item in products)
            {
                Console.WriteLine($"Добавляем продукт: ID={item.Id}, Name={item.Product_Name}");

                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Uid = item.Id.ToString(); 
                comboBoxItem.Tag = item;
                comboBoxItem.Content = $"{item.Product_Name}";

                Product_Name = comboBoxItem.Content.ToString();
                ID_Product = comboBoxItem.Uid.ToString();

                comboBoxItem.Loaded += (s, e) =>
                {
                    Console.WriteLine($"ComboBoxItem загружен: Uid={comboBoxItem.Uid}, Content={comboBoxItem.Content}");
                };

                comboBoxItem.MouseDown += ComboBoxItem_MouseDown;

                ComboBoxDropProduct.Items.Add(comboBoxItem);
            }
        }

        private void ComboBoxItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("MouseDown вызван!");

            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;

            // Выводим всю информацию
            Console.WriteLine($"Sender Type: {sender.GetType()}");
            Console.WriteLine($"Uid: '{comboBoxItem.Uid}'");
            Console.WriteLine($"Tag: '{comboBoxItem.Tag}'");
            Console.WriteLine($"Content: '{comboBoxItem.Content}'");
            Console.WriteLine($"IsLoaded: {comboBoxItem.IsLoaded}");

            // Получаем родительские элементы
            var parent = VisualTreeHelper.GetParent(comboBoxItem);
            while (parent != null)
            {
                Console.WriteLine($"Parent: {parent.GetType().Name}");
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        private void DropProduct(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Product_Name : {Product_Name}  ID_Product : {ID_Product}");
          int Key =  API_MENEGER_DATABASE.DropProduct(main.ID_WareHouse, Product_Name, int.Parse(ID_Product), CotigoruDropProduct.SelectedIndex, int.Parse(CointText.Text));

            switch (Key) 
            {
                case 1:
                    MessageBox.Show("Товар списан","Успешно!",MessageBoxButton.OK,MessageBoxImage.Information);
                    break;
                default:
                    MessageBox.Show("Товар списан "+ Key, "Отказанно!", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
