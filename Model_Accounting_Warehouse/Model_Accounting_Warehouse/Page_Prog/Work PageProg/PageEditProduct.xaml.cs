using Dapper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PageEditProduct.xaml ThisSupplier
    /// </summary>
    public partial class PageEditProduct : Page
    {
        public bool isCreateProduct = true;
        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE aPI = new Modul.API_MENEGER_DATABASE(main.Name_Server);

        bool EditBool = false;
        public int CodeProduct = 0;
        public PageEditProduct()
        {
            InitializeComponent();


            if (aPI.GetSuppliersList().Count() != 0)
            {
                ThisSupplier.Items.Clear();
            }
            foreach (var item in aPI.GetSuppliersList())
            {
                ThisSupplier.Items.Add(item.Id +" | "+item.Supplier_Name);
            }

            if ((CodeProduct > 10000 && CodeProduct < 999999) || (CodeProduct == 0))
            {

                if(CodeProduct == 0) { return; }
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Один запрос для проверки и получения данных
                    string query = @" SELECT * FROM Products WHERE Product_Place = @Product_Place";

                    var LogInTry = connection.QueryFirstOrDefault<dynamic>(query, new { Product_Place = CodeProduct });

                    if (LogInTry != null)
                    {
                        NameProductTextBox.Text = LogInTry.Product_Name;
                        Nuber.Text = LogInTry.Product_Place.ToString();
                    }
                    else
                    {
                        MessegeBox("Не верный код хронения товара\nДанный код не сущесвует", "Ошибка при провеки продукта"); return;

                    }
                }
            }
            else 
            {
                MessegeBox("Не верный код хронения товара\nДанный код - не имеет право на сущесвование", "Ошибка при провеки продукта"); return;
            }
           
          
        }

        string Status = "", Images = "Не назначен";
        
        private void EditTable(object sender, RoutedEventArgs e)
        {
            try 
            {
                int Key = aPI.AddProduct(Images, NameProductTextBox.Text, DescriptionProductTextBox.Text, CotogoryIndex(Cotigory.SelectedIndex), "Ожидается", "Жал   ь, что оно ставиться автомотически..", Nuber.Text, ArePrise.Text, BayPrise.Text,main.ID_WareHouse,1);
                Console.WriteLine($"KeyProduct = {Key}");
                switch (Key) 
                {
                    case 0:
                        MessageBox.Show("Вызванно исключение, что не дало создать заказ. Смотрите логированние!","Ошибка заказа",MessageBoxButton.OK,MessageBoxImage.Error);
                        break;
                    case -7:
                        MessageBox.Show("Ошибка оформления заказа, читайте логированнние", "Ошибка заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case -1:
                        MessageBox.Show("Ошибка оформления заказа, читайте логированнние", "Ошибка заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case 1:
                        MessageBox.Show("Заказ успено офромлен", "Заказ оформлен", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    default : MessageBox.Show("Не обработанно", "Ошибка заказа", MessageBoxButton.OK, MessageBoxImage.Error); break;
                }
            }
            catch (Exception ex)
            { 
               MessegeBox(ex.Message, "Ошибка при регестрации продукта (Не обработанное исключение)");
            }


           
        }

        string ThisSupplierConvert(int s) 
        {
            return null;
        }

        public void CreateProduct()
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    var LogInTry = connection.Query<Products>("SELECT * FROM Products Where Products.Product_Name = @Product_Name", new { Product_Name = NameProductTextBox.Text });
                    if (LogInTry.Count() > 0) { MessegeBox("Данный Продукт уже сущесвует!", "Ошибка при создании"); return; }


                    var insertTableInfoQuery = @"INSERT INTO Products 
( Product_icon, Product_Name, Product_Description, Product_Cotigory, Product_Status, Product_DeliveryDate,
    Product_Remains, Product_Place, Product_Are_Pay, Product_Supplier, Product_Pay,ShopInfor ) 
VALUES
( @Product_icon, @Product_Name, @Product_Description, @Product_Cotigory, @Product_Status, @Product_DeliveryDate,
    @Product_Remains, @Product_Place, @Product_Are_Pay, @Product_Supplier, @Product_Pay,@ShopInfor ) ";

                     Status = GetPositionFromComboBoxIndex(TypeStatus.SelectedIndex);

                    connection.Execute(insertTableInfoQuery, new
                    {
                        Product_Name = NameProductTextBox.Text,
                        Product_icon = Images,
                        Product_Description = DescriptionProductTextBox.Text,
                        Product_Prise = int.Parse(MaiPrise.Text),
                        Product_Remains = int.Parse(ProductRemains.Text),
                        Product_Pay = int.Parse(PayPrise.Text),
                        Product_Bay = int.Parse(PayPrise.Text),
                        Product_Are_Pay = int.Parse(ArePrise.Text),
                        Product_Date_of_Delivery = Calend.DisplayDate, // Дата из Элемента Calendar
                        Product_Status = Status,

                    });

                
                    MessageBox.Show("Новый продукт успешно добавлен!", "Успешно!",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
            }
            catch (Exception ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
            }
        }

        private string CotogoryIndex(int index)
        {
            // Если индекс 0 (заголовок "Котигория товара"), возвращаем пустую строку
            if (index == 0)
            {
                return "NULL";
            }

            // Для остальных индексов получаем реальную категорию
            switch (index)
            {
                case 1: return "Алкаголь";
                case 2: return "Быстрое питание";
                case 3: return "Бытовая техника";
                case 4: return "Бытовая химия";
                case 5: return "Детсоке питание";
                case 6: return "Фрукты - Овощи";
                case 7: return "Молочные продукты";
                case 8: return "Огневые нагриватели";
                case 9: return "Табак";
                case 10: return "Энергетические напитки";
                case 11: return "Гаизованные напитки";
                case 12: return "Снеки";
                case 13: return "Электронника";
                case 14: return "Камукативыне приборы";
                case 15: return "Куханные приборы";
                default: return "Ожидается";
            }
        }

        private string GetPositionFromComboBoxIndex(int index)
        {
            switch (index)
            {
                case 0: return "Нет в наличии";
                case 1: return "В наличии";
                case 2: return "Ожидается";
                default: return "Ожидается";
            }
        }

        public void MessegeBox(string messeg, string title)
        {
            MessageBox.Show(
                messeg, title,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Error
                );
        }

        private void ProductRemains_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            ThisSupplier = aPI.GetSupplier();
        }

        private void KeyDowns(object sender, KeyEventArgs e)
        {

        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Выберите изображение для товара"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Images = openFileDialog.FileName;
                ImageProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
