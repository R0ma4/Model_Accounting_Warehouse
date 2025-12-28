using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для PageTableProduct.xaml
    /// </summary>
    public partial class PageTableProduct : Page
    {
        // Коллекция для хранения продуктов (ObservableCollection для автоматического обновления UI)
        private ObservableCollection<Products> _products;
        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE _database = new Modul.API_MENEGER_DATABASE(main.Name_Server);
        public string NameLofin;

        Products Product;
        public PageTableProduct()
        {
            InitializeComponent();
            _products = new ObservableCollection<Products>();
            ProductsGrid.DataContext = _products;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            main.Reg.FixElement(GridWindow);
            LoadProducts(' ');
        }

        private void LoadProducts(char s) 
        {
            if (ProductsGrid.Columns.Count > 0)
            {
                ProductsGrid.Columns.Clear();
            }
            _database.NameBasaData = main.Name_Data_Base;

            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart ,"Id", "ID продукта"));
            // ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Icon, "Product_icon", "Изоброжение товара"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_Name", "Название"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_Description", "Описание"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_Status", "Статус"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_DeliveryDate", "Дата последнего привоза"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_Cotigory", "Отдел товара"));
            ProductsGrid.Columns.Add(_database.Select_Table_Corrector(Modul.DataByse.Product, Modul.TypeData.Standart, "Product_Place", "Место / Полка товара"));

            ProductsGrid.ItemsSource = _database.LoadProductsData("Id", NameLofin);

        }
        // Старый варик :) 
        private void LoadProducts()
        {
            try
            {
                var conections = $"Server={main.Name_Server};DataBase={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(conections))
                {
                    connection.Open();

                    
                    var products = connection.Query<Products>("SELECT * FROM Products ORDER BY Product_Id");

                   
                    _products.Clear();
                    foreach (var product in products)
                    {
                        _products.Add(product);
                    }
                    if(_products.Count > 0) 
                    { 
                        ProductsGrid.Visibility = Visibility.Visible;
                        InfoTextBlock.Visibility = Visibility.Collapsed;
                    }
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
             
            }
        }


        private void EditRowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MessageBoxResult messageBox =  MessageBox.Show("Вы уверены, что хотите удалить? " + button.Uid, "Вы уверенны?",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (messageBox == MessageBoxResult.Yes) 
            {
                var conections = $"Server={main.Name_Server};DataBase={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(conections))
                {
                    try
                    {
                        connection.Open();
                        var exQ = connection.Query("delete from Products where Product_Id = @Product_Id", new { Product_Id = button.Uid });
                        connection.Close();

                        MessageBox.Show("Товар был успешно удалён!\nНо инромация о нём, осталась в отчёте!", "Продукт был удалён!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    }
                    catch { }
                }
            }
        }

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void KeyControl(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5) 
            {
                LoadProducts(' ');

            }
        }
    }
}
