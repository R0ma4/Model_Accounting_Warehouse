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
    /// Логика взаимодействия для PageEditProduct.xaml
    /// </summary>
    public partial class PageEditProduct : Page
    {
        public bool isCreateProduct = true;
        public MainWindow main = Application.Current.MainWindow as MainWindow;

        bool EditBool = false;
        public int CodeProduct = 10001;
        public PageEditProduct()
        {
            InitializeComponent();

            if (CodeProduct > 10000 && CodeProduct < 999999)
            {

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
                if (string.IsNullOrEmpty(NameProductTextBox.Text)) { MessegeBox("Наиминование продукта, не может быть пустым", "Ошибка при регестрации продукта"); return; }

                if (!int.TryParse(BayPrise.Text, out int CorectBayPrise)) { MessegeBox("Назночение \"Цена Покупки Товара\" имел не коректный ввод!", "Ошибка при регестрации продукта"); return; }

                if (!int.TryParse(PayPrise.Text, out int CorectPayPrise)) { MessegeBox("Назночение \"Цена Продажи Товара\" имел не коректный ввод!", "Ошибка при регестрации продукта"); return; }
                if (!int.TryParse(MaiPrise.Text, out int CorectMaiPrise)) { MessegeBox("Назночение \"Стандартная цена товара\" имел не коректный ввод!", "Ошибка при регестрации продукта"); return; }
                if (!int.TryParse(ProductRemains.Text, out int CorectProductRemains)) { MessegeBox("Назночение \"Остаток на складе\" имел не коректный ввод!", "Ошибка при регестрации продукта"); return; }
                if (!int.TryParse(ArePrise.Text, out int CorectArePrise)) { MessegeBox("Назночение \"Возрост для продажи\" имел не коректный ввод!", "Ошибка при регестрации продукта"); return; }

                if (CorectArePrise < 4 || CorectArePrise > 25)
                {
                    MessegeBox("Возрост продажи не верен. И должен быть от 4 до 25", "Ошибка при регестрации продукта");
                    return;
                }


                if (CorectBayPrise < 0 || CorectPayPrise < 0 || CorectMaiPrise < 0)
                {
                    MessegeBox("НИ одна цена не может быть отцрицательной", "Ошибка при регестрации продукта");
                    return;
                }

                if (CorectPayPrise < (CorectBayPrise * 1.5))
                {
                    MessegeBox("Цена Продажи в магазине должна быть хотябы в 1.5 раза выше, чем закупочная ценна", "Ошибка при регестрации продукта");
                    return;
                }
                int Min = CorectPayPrise - (35 / CorectMaiPrise);
                int Max = CorectPayPrise + (35 / CorectMaiPrise);
                if (CorectPayPrise > Max ||  CorectPayPrise < Min)
                {
                    MessegeBox("Разница цен можду точкой и магазина - не должна быть больше/меньше 35% от изночальной стоимости", "Ошибка при регестрации продукта");
                    return;
                }


                if (CorectMaiPrise < (CorectBayPrise * 1.5))
                {
                    MessegeBox("Сумма оригинального ценика в магазине должна быть хотябы в 1.5 раза выше, чем закупочная ценна", "Ошибка при регестрации продукта");
                    return;
                }

                if (CorectProductRemains < 0)
                {
                    MessegeBox("Количесво остатка на складе не может быть отрицательным", "Ошибка при регестрации продукта");
                    return;
                }

                if (CorectProductRemains == 0)
                {
                    if (TypeStatus.SelectedIndex == 1)
                    {
                        MessegeBox("Статус \"В Наличие\" - Не может присвоеться к товару, чей остаток равен 0", "Ошибка при регестрации продукта");
                        return;
                    }
                }
                if (TypeStatus.SelectedIndex == 0)
                {
                    CorectProductRemains = 0;
                    ProductRemains.Text = "0";
                }
            }
            catch (Exception ex)
            { 
               MessegeBox(ex.Message, "Ошибка при регестрации продукта (Не обработанное исключение)");
            }


            if (isCreateProduct)
            {
                CreateProduct();
            }
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


                    var insertTableInfoQuery = @"INSERT INTO Products (
    Product_Icone, Product_Name, Product_Description, 
    Product_Prise, Product_Status, Product_Date_of_Delivery,
    Product_Remains, Product_Pay, Product_Bay, Product_Are_Pay
) 
VALUES 

(@Product_Icone, @Product_Name, @Product_Description, 
 @Product_Prise, @Product_Status, @Product_Date_of_Delivery, @Product_Remains, @Product_Pay, @Product_Bay, @Product_Are_Pay);";

                     Status = GetPositionFromComboBoxIndex(TypeStatus.SelectedIndex);

                    connection.Execute(insertTableInfoQuery, new
                    {
                        Product_Name = NameProductTextBox.Text,
                        Product_Icone = Images,
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
