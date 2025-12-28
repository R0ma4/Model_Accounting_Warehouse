using Dapper;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
// Пи пу пи пу пи ) 
using System.Net;
using System.Net.Http;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Model_Accounting_Warehouse.Modul
{
    public enum DataByse 
    {
        Product,
        LogIn,
        UserInfo,
        EmtityPrise,
        Supplier
    }
   public enum TypeData 
   {
        Nan, 
        Icon, 
        Standart,
        Button,
   }
    public class API_MENEGER_DATABASE
    {
        string NameServers = string.Empty;
        public string NameBasaData = string.Empty;
        MainWindow main = Application.Current.MainWindow as MainWindow;
        public API_MENEGER_DATABASE(string NameSerever)
        {
                NameServers = NameSerever;
            Console.WriteLine(
                "Зашифровванный Пороль: " + SHIFT("Password", "Admin123123"),
                "Разшифрованный Пороль: " + DESHIFT("Password", "Admin123123")
                );
        }
      
        #region Допка Топка 

        private string GetPositionFromComboBoxIndex(int index)
        {
            switch (index)
            {
                case 0: return "Администратор";
                case 1: return "Менеджер";
                case 2: return "Кассир";
                case 3: return "Кладовщик";
                case 4: return "Гость";
                default: return "Нет прав";
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

        #endregion

        #region Улучшенное шифрование с солью

        public string SHIFT(string key, string word, string salt = "")
        {
            word = word.Trim();
            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(key)) return word;

            // Добавляем соль к ключу
            string enhancedKey = key + salt;

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < word.Length; i++)
            {
                char currentChar = word[i];
                char keyChar = enhancedKey[i % enhancedKey.Length];

                // Сложное преобразование: XOR + сдвиг на позицию + код ключа
                int shift = (keyChar + i) % 65536;
                char encryptedChar = (char)((currentChar ^ keyChar) + shift);

                result.Append(encryptedChar);
            }

            return result.ToString();
        }

        public string DESHIFT(string key, string word, string salt = "")
        {
            word = word.Trim();
            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(key)) return word;

            string enhancedKey = key + salt;

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < word.Length; i++)
            {
                char currentChar = word[i];
                char keyChar = enhancedKey[i % enhancedKey.Length];

                int shift = (keyChar + i) % 65536;
                char decryptedChar = (char)((currentChar - shift) ^ keyChar);

                result.Append(decryptedChar);
            }

            return result.ToString();
        }

        #endregion
        // В классе API_MENEGER_DATABASE добавьте:



        /// <summary>
        /// DOX Генератор
        /// </summary>
        /// <param name="IdWareHouse"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public int DoxOperation(int IdWareHouse, string filePath = @"D:\Model_Accounting_Warehouse\Model_Accounting_Warehouse\Model_Accounting_Warehouse\Rouls\Docum.docx")
        {
            
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";

            try 
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Query<Operation>("SELECT * FROM Operation");
                    int coint_cotigori = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Operation  as opi Where IdWarehouse = @IDWH", new { IDWH = IdWareHouse});
                    int coint_operation = connection.ExecuteScalar<int>("SELECT * FROM Operation Where IdWarehouse = @IDWH",new { IDWH = IdWareHouse});
                    int coint_register_product = connection.ExecuteScalar<int>("SELECT ISNULL(COUNT(*),0) FROM Operation as opi where TypeOperation = 1 and IdWarehouse = @IDWH", new { IDWH = IdWareHouse });
                    int coint_drop_product = connection.ExecuteScalar<int>("SELECT ISNULL(COUNT(*),0) FROM Operation as opi where TypeOperation = 2 or TypeOperation = 3 or TypeOperation = 4 and IdWarehouse = @IDWH", new { IDWH = IdWareHouse });
                    int coint_finqldata_product = connection.ExecuteScalar<int>("SELECT ISNULL(COUNT(*),0) FROM Operation as opi where TypeOperation = 2 and IdWarehouse = @IDWH", new { IDWH = IdWareHouse });
                    int coint_theft_product = connection.ExecuteScalar<int>("SELECT ISNULL(COUNT(*),0) FROM Operation as opi where TypeOperation = 3 and IdWarehouse = @IDWH", new { IDWH = IdWareHouse });
                    int coint_nocotigor_product = connection.ExecuteScalar<int>("SELECT ISNULL(COUNT(*),0) FROM Operation as opi where TypeOperation = 4 and IdWarehouse = @IDWH", new { IDWH = IdWareHouse });
                    
                    var Warehouse = connection.Query<Warehouse>("SELECT * From Warehouse where id = @Id", new { Id = IdWareHouse}).ToList();
                    string NameWarehouse = string.Empty;

                    Console.WriteLine(
                        "Dox Data\n" +
                        $"coint_cotigori: {coint_cotigori}"
                        );

                    using (DocX document = DocX.Create(filePath))
                    {
                        foreach (var item in Warehouse) 
                        {
                            NameWarehouse = item.Street.Trim().ToString();
                            break;
                        }

                        foreach (var item in connectionString)
                        {
                            if (true)
                            {
                                // Заголовок 
                                Xceed.Document.NET.Paragraph title = document.InsertParagraph($"Отчет по Складу Отчет по Складу по складу «{NameWarehouse}»");
                                title.FontSize(18);
                                title.SpacingBefore(1.25);
                                title.Font("Time New Roman");
                                title.Bold();
                                title.Alignment = Alignment.center;

                                Xceed.Document.NET.Paragraph Anotach = document.InsertParagraph("\n\n" +
                                    $"Информация об хронение и продажах товаров на складе - хронявшейся в магазине по улице: \'\'. За периуд всё время от открытия склада до \'{DateTime.Now.ToString("dd.MM.yyyy")}\'\n" +
                                    $"За данное время было операций: {coint_cotigori}.\n" +
                                    $"Проданно  - Едениц товара. Самый продоваеммый [NameProduct] / Чаще всего списывали (по просрочке) [NameProduct].\n" +
                                    $"{coint_drop_product} списаний:\n\n" +
                                    $"1) Списание по кражам: {coint_theft_product} шт.\n" +
                                    $"2 )Списание из-за срока годности: {coint_finqldata_product} шт.\n" +
                                    $"3 )Списание из-за не соотвествия: {coint_nocotigor_product} шт.\n" +
                                    "\n\n");
                                Anotach.FontSize(14);
                                Anotach.Font("Time New Roman");
                                Anotach.SpacingBefore(1.25);
                                Anotach.Alignment = Alignment.right;


                                Xceed.Document.NET.Paragraph InfoGull = document.InsertParagraph("Таблица: \n\n");
                                InfoGull.FontSize(14);
                                InfoGull.Font("Time New Roman");
                                InfoGull.SpacingBefore(1.25);
                                InfoGull.Alignment = Alignment.left;
                                // Таблица 

                                var Table = document.AddTable(5, 6);
                                Table.Design = TableDesign.ColorfulList;

                                Table.Rows[0].Cells[0].Paragraphs.First().Append("ID");
                                Table.Rows[0].Cells[1].Paragraphs.First().Append("Товар");
                                Table.Rows[0].Cells[2].Paragraphs.First().Append("Продажи");
                                Table.Rows[0].Cells[3].Paragraphs.First().Append("Остаток");
                                Table.Rows[0].Cells[4].Paragraphs.First().Append("Списание");
                                Table.Rows[0].Cells[5].Paragraphs.First().Append("Причина");

                                document.InsertTable(Table);

                                // Документ создан:  
                                Xceed.Document.NET.Paragraph Responsibility = document.InsertParagraph("Подпись ответсвенного по отчёту: _______");
                                Responsibility.FontSize(11);
                                Responsibility.Bold();
                                Responsibility.Alignment = Alignment.right;
                            }
                            break;
                        }

                        document.Save();
                    }

                }
                return 1;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Dox Cenerl:\n"+ex.Message+"\n"+ex.InnerException+"\n"+ex.Source+"\n"+ex.StackTrace+"\n end.");
                return -1;
            }
        }

        public List<UserInfo> GetIdWarehouse(string LogIn) 
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString)) { return connection.Query<UserInfo>(" SELECT ui.Info_Shop From UserInfo as ui where id = (select lt.UserInfoId From LogTable as lt where Log_In = @LOG)", new { LOG = LogIn }).ToList(); }
        }

        public List<Products> GetProductsInStock(int ShopInfor_ID)
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString)) { return connection.Query<Products>("SELECT * From Products  where ShopInforID = @InforID and Product_Status = 'В наличии'", new { InforID = ShopInfor_ID }).ToList(); }
        }

        public List<Products> GetProducts() 
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString)) { return connection.Query<Products>("SELECT * FROM Products").ToList(); }
        }


        public List<Warehouse> GetOperation()
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString)) { return connection.Query<Warehouse>("SELECT * FROM Warehouse").ToList(); }
        }

        public List<Warehouse> GetWarehouse()
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";
            using (var connection = new SqlConnection(connectionString)) { return connection.Query<Warehouse>("SELECT * FROM Warehouse").ToList(); }
        }

        public List<Supplier> GetSuppliersList()
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Supplier>("SELECT * FROM Supplier").ToList();
            }
        }
        public ComboBox GetSupplier()
        {

            ComboBox comboBox = new ComboBox();


            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";

            Console.WriteLine("НАЧАЛО РАБОТЫ - GetSupplier()");
            try
            {
                Console.WriteLine("Гуди Гуди");
                using (var connection = new SqlConnection(connectionString))
                {
                    Console.WriteLine("А Я ТУТ");
                    connection.Open();

                    var suppliers = connection.Query<Supplier>("SELECT * FROM Supplier");

                    if (suppliers == null || !suppliers.Any())
                    {
                        comboBox.Items.Add("Нет поставщиков");
                        comboBox.IsEnabled = false;
                    }
                    else
                    {
                        foreach (var supplier in suppliers)
                        {
                            comboBox.Items.Add($"{supplier.Id}| {supplier.Supplier_Name}");
                            Console.WriteLine($"Загружено поставщиков добавленно : {supplier.Id}| {supplier.Supplier_Name}");
                        }
                        Console.WriteLine($"Загружено поставщиков: {suppliers.Count()}");
                    }
                    return comboBox;
                }
            }
            catch (SqlException sqlEx)
            {
                comboBox.Items.Add($"Ошибка БД: {sqlEx.Message}");
                comboBox.IsEnabled = false;
                Console.WriteLine($"SQL Ошибка: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                comboBox.Items.Add("Ошибка загрузки поставщиков");
                comboBox.IsEnabled = false;
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            comboBox.Items.Add("Да я");
            return comboBox;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Producticon"></param>
        /// <param name="ProductName"></param>
        /// <param name="ProductDescription"></param>
        /// <param name="ProductCotigory"></param>
        /// <param name="ProductStatus"></param>
        /// <param name="ProductDeliveryDate"></param>
        /// <param name="ProductPlace"></param>
        /// <param name="ProductArePay"></param>
        /// <param name="BayPrise"></param>
        /// <param name="Register">true - если это товар с регестрацияей, false - если это поставка</param>
        /// <returns></returns>
        public int AddProduct(string Producticon, string ProductName, string ProductDescription, string ProductCotigory, string ProductStatus, string ProductDeliveryDate, string ProductPlace, string ProductArePay, string BayPrise, int ShopInfor_ID, int ProductSupplier_Id, bool Register = true)
        {
            try
            {
                BayPrise = BayPrise.Replace(" ", null);

                Console.WriteLine
                    (
                    $"Producticon: {Producticon}\n" +
                    $"ProductName: {ProductName}\n" +
                    $"ProductCotigory: {ProductCotigory}\n" +
                    $"ProductStatus: {ProductStatus}\n" +
                    $"ProductDeliveryDate: {ProductDeliveryDate}\n" +
                    $"ProductPlace: {ProductPlace}\n" +
                    $"ProductArePay: {ProductArePay}\n" +
                    $"BayPrise: {BayPrise}\n" +
                    $"ProductDescription: {ProductDescription}\n"

                    );

                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                if(string.IsNullOrEmpty(Producticon) || string.IsNullOrEmpty(ProductName) || string.IsNullOrEmpty(ProductDescription) || string.IsNullOrEmpty(ProductCotigory) || string.IsNullOrEmpty(ProductStatus) || 
                  string.IsNullOrEmpty(ProductDeliveryDate) || string.IsNullOrEmpty(ProductPlace) || string.IsNullOrEmpty(ProductArePay) || string.IsNullOrEmpty(BayPrise) ) { return -7; }

               

                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();




                        var insertQuery = @"
                INSERT INTO Products 
                (Product_icon, Product_Name, Product_Description, Product_Cotigory, Product_Status, 
                 Product_DeliveryDate, Product_Place, Product_Are_Pay, BayPrise, 
                 ProductSupplierId, ShopInforID)
                VALUES 
                (@Product_icon, @Product_Name, @Product_Description, @Product_Cotigory, @Product_Status, 
                 @Product_DeliveryDate, @Product_Place, @Product_Are_Pay, @BayPrise, 
                 @ProductSupplierId, @ShopInforID)";

                        connection.Execute(insertQuery,
                            new
                            {
                                Product_icon = Producticon,
                                Product_Name = ProductName,
                                Product_Description = ProductDescription,
                                Product_Cotigory = ProductCotigory,
                                Product_Status = "Ожидаеться",
                                Product_DeliveryDate = DateTime.Now.AddDays(0), 
                                Product_Place = ProductPlace,
                                Product_Are_Pay = ProductArePay,
                                BayPrise = BayPrise,
                                ProductSupplierId = ProductSupplier_Id,
                                ShopInforID = ShopInfor_ID
                            });


                        var RegisterAndPostavka =
                       @"INSERT INTO Operation (Id,TypeOperation,Coint,DataOperation,IdWarehouse,IdProduct)
                         VALUES 
                         (@Id,@TypeOperation,@Coint,@DataOperation,@IdWarehouse,@IdProduct)";

                        int IdOperation = connection.Execute("SELECT ISNULL(MAX(Id), 0) From Operation");

                        int maxId = connection.ExecuteScalar<int>("SELECT ISNULL(MAX(Id), 0) FROM Products");
                        int newId = maxId;
                                           

                        Console.WriteLine($"IdProduct4: {newId}");

                        connection.Execute(RegisterAndPostavka,
                            new
                            {
                                Id = IdOperation + 1,
                                TypeOperation = 1,
                                Coint = 12,
                                DataOperation = DateTime.Now.AddDays(0), // как-бы добовляем сегодняшную даду)
                                IdWarehouse = ProductSupplier_Id,
                                IdProduct = newId,
                            }
                            );
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("AddProduct(); Cenerl:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.Source + "\n" + ex.StackTrace + "\n end.");
                        return 0;
                    }

                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
                return -1;
            }

        }

        public int AddNewSupplier(string Name_Supplier) 
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        var insertTableUserQuery =
                        @"INSERT INTO Vacations (Product_icon,Product_Name,Product_Description,Product_Cotigory,Product_Remains,Product_Status, Product_DeliveryDate, Product_Place, Product_Are_Pay, BayPrise)
                         VALUES 
                         (@Product_icon, @Product_Name, @Product_Description, @Product_Cotigory, @Product_Remains, @Product_Status, @Product_DeliveryDate, @Product_Place, @Product_Are_Pay, @BayPrise)";

                        connection.Execute(insertTableUserQuery,
                            new
                            {
                                    
                            }
                            );
                        return 1;
                    }
                    catch
                    {
                        return 0;
                    }

                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
                return -1;
            }
        }
        public  int PromProduct(int IdWH) 
        {
            var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    // Замена для Чековой истории (не реализорованно)
                    // var insertTableUserQuery = @"UPDATE Products SET Product_Status = 'В наличии' WHERE ShopInforID = @IDS AND Product_Status = 'Ожидается'"; (0 Записей задейсвованно)
                    var insertTableUserQuery = @"UPDATE Products SET Product_Status = 'В наличии' WHERE ShopInforID = @IDS";
                    connection.Execute(insertTableUserQuery, new { IDS = IdWH });

                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                    return -1;
                }

            }
        }
        public int DropProduct(int IdWH, string Product_Name, int Product_NameId, int TypeOperation, int Coint)
        {
            try
            {
                int TypeOperationTable = 0;
                switch (TypeOperation) 
                {
                    case 0:
                        TypeOperationTable = 2;
                        break;
                    case 1:
                        TypeOperationTable = 4;
                        break;
                    case 2:
                        TypeOperationTable = 4;
                        break;
                    case 3:
                        TypeOperationTable = 3;
                        break;
                }
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                int ID = 1;
                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {

                        connection.Open();


                        int maxId = connection.ExecuteScalar<int>("SELECT ISNULL(MAX(Id), 0) FROM Products");
                        int newId = maxId + 1;


                        var UpdaterOp = "INSERT INTO Operation(Id, TypeOperation, Coint, DataOperation, IdWarehouse, IdProduct)" +
                            " VALUES " +
                            "(@Id, @TypeOperation, @Coint, @DataOperation, @IdWarehouse, @IdProduct)";
                        connection.Execute(UpdaterOp, new { Id = newId, TypeOperation = TypeOperationTable, Coint = Coint, DataOperation = DateTime.Now.AddDays(0), IdWarehouse = IdWH, IdProduct = Product_NameId });
                        // var insertTableUserQuery = @"DELETE FROM Products where ShopInforID = @IDS and Product_Status = 'В наличии' and Product_Name = @ProdName";

                        // Замена для Чековой истории (не реализорованно)
                        var insertTableUserQuery = @"UPDATE Products 
SET Product_Status = 'Списан'
WHERE ShopInforID = @IDS 
  AND Product_Status = 'В наличии' 
  AND Product_Name = @ProdName";
                        connection.Execute(insertTableUserQuery, new { IDS = IdWH, ProdName = Product_Name });

                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + ex.StackTrace);
                        return -1;
                    }

                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
                return -2;
            }
        }

        /// <summary>
        /// Полностью сбрасывает базу данных
        /// </summary>
        public int DropDataBase() 
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        var insertTableUserQuery = @"
                    DELETE FROM LogTable;
                    DELETE FROM Products;
                    DELETE FROM Supplier;
                    DELETE FROM UserInfo;
                    DELETE FROM EmtityPrise;
                    DELETE FROM ShopInfo;";

                        connection.Execute(insertTableUserQuery);
                        return 1;
                    }
                    catch
                    {
                        return -1;
                    }

                }
            }
            catch (SqlException ex) 
            {
                MessegeBox(ex.Message, "Ошибка!");
                return -2;
            }
        }
        /// <summary>
        /// Создание Магазина
        /// </summary>
        /// <param name="_NameShop"></param>
        /// <param name="_Street"></param>
        /// <param name="_INN"></param>
        /// <param name="_KPP"></param>
        /// <param name="_BankAccount"></param>
        /// <param name="_BankName"></param>
        /// <param name="_BIK"></param>
        /// <param name="_LegalAddress"></param>
        /// <param name="_Phone"></param>
        /// <param name="_Email"></param>
        /// <returns></returns>
        public int CreateShop(string _NameShop, string _Street, string _INN, string _KPP, string _BankAccount, string _BankName, string _BIK, string _LegalAddress, string _Phone, string _Email)
        {

            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {

                        Console.WriteLine
                        (
                            $"\n\n==============================\n" +
                            $"\nShopName: {_NameShop}" +
                            $"\nStreet: {_Street}" +
                            $"\nINN: {_INN}"+
                            $"\nKPP: {_KPP}" +
                            $"\nBankAccount: {_BankAccount}" +
                            $"\nBankName: {_BankName}" +
                            $"\nBIK: {_BIK}" +
                            $"\nLegalAddress: {_LegalAddress}" +
                            $"\nPhone: {_Phone}" +
                            $"\nEmail: {_Email}\n" +
                            "\n\n=============================="
                        );
                        // Валидация ИНН
                        Regex innRegex = new Regex(@"^\d{10}$|^\d{12}$");
                        // Валидация номера счета
                        Regex accountRegex = new Regex(@"^\d{21}|^\d{20}$|^\d{19}|^\d{18}$");
                        // Валидация КПП
                        Regex kppRegex = new Regex(@"^\d{9}$");
                        // Валидация БИК
                        Regex bikRegex = new Regex(@"^\d{9}$");
                        // Валидация телефона
                        Regex phoneRegex = new Regex(@"^(\+7|8|\+375|7)[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$");
                        // Валидация email
                        Regex emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);
                        if (!innRegex.IsMatch(_INN.Trim())) { return - 6;}

                       
                        if (!kppRegex.IsMatch(_KPP.Trim())) { return - 5;}

                        if (!accountRegex.IsMatch(_BankAccount.Trim()))  {   return - 4; }


                        if (!bikRegex.IsMatch(_BIK.Trim())) { return - 3; }


                        if (!phoneRegex.IsMatch(_Phone.Trim()))  { return - 2;}


                        if (!emailRegex.IsMatch(_Email.Trim())) { return - 1; }

                        connection.Open();
                        var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM Warehouse";
                        int maxId = connection.ExecuteScalar<int>(maxIdQuery);
                        int newId = maxId + 1;

                        var insertTableUserQuery = @"
                 INSERT INTO Warehouse (Id, Street, Sales, INN, KPP, BankAccount, BankName, BIK, LegalAddress, Phone, Email)
                 VALUES 
                 (@Id, @Street, @Sales,   @INN, @KPP, @BankAccount,  @BankName, @BIK,@LegalAddress, @Phone, @Email)";

                        connection.Execute(insertTableUserQuery, new 
                        {
                            Id = newId,
                            Street = _Street,
                            Sales = 0, // По умолчанию 0 - так как при создании магазин / склад не мог ничего заработать
                            INN = _INN,
                            KPP = _KPP,
                            BankAccount = _BankAccount,
                            BankName = _BankName,
                            BIK = _BIK,
                            LegalAddress = _LegalAddress,
                            Phone = _Phone,
                            Email = _Email
                        });
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"============================================\n{ex.Message} | {ex.StackTrace} | {ex.InnerException}");

                        return 0;
                    }


                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
                return 0;
            }
        }
        // ЧТО-БЫ УДАЛИТЬ / ЗАКРЫТЬ МАГАЗИН - НУЖНО БУДЕТ СБРОСИТЬ КАК И СКАЛАД ТАК И ВСЕХ СОТРУДНИКОВ!
        public int DropWarehouse(int Id_Warehouse) // IdShop - Назначаеться из выподающего списка.
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        var insertTableUserQuery = @"
                        DELETE FROM UserInfo WHERE Info_Shop = @ID;
                        DELETE FROM Products WHERE ShopInforID = @ID;
                        DELETE FROM Warehouse WHERE Id= @ID; 
                        DELETE FROM Operation Where IdWarehouse = @ID;";

                        connection.Execute(insertTableUserQuery, new { ID = Id_Warehouse });
                        return 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\nStackTrace: {ex.StackTrace} |\nInnerException: {ex.InnerException} |\nData: {ex.Data}");
                        return 0;
                    }

                }
            }
            catch (Exception ex) 
            {
                return -1;
            }
            }

        /// <summary>
        /// Увольняет Пользователя по паспорту
        /// </summary>
        public int Fire(string Namber_Pasport)
        {
            try
            {
                Regex DateFormatRegex = new Regex(@"^\d{2} \d{2} \d{4}$", RegexOptions.Compiled);

                if (!DateFormatRegex.IsMatch(Namber_Pasport.Trim()))
                {
                    return -1;
                }

                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_Connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var LogProfil = @"SELECT logs.UserInfoId FROM LogTable as logs where logs.UserInfoId = (Select Id FROM UserInfo WHERE Pasport = @Pasport)";
                    int Llogcount = connection.ExecuteScalar<int>(LogProfil, new { Pasport = Namber_Pasport });


                    var checkQuery = @"SELECT COUNT(1) FROM UserInfo WHERE Pasport = @Pasport";
                    int count = connection.ExecuteScalar<int>(checkQuery, new { Pasport = Namber_Pasport });

                    if (count > 0)
                    {
                        var deleteQuery = @"DELETE FROM LogTable where UserInfoId = (Select Id FROM UserInfo WHERE Pasport = @Pasport)";
                        int rowsAffected = connection.Execute(deleteQuery, new { Pasport = Namber_Pasport });

                        var deleteQuery1 = @"DELETE FROM UserInfo WHERE Pasport = @Pasport";
                        int rowsAffected1 = connection.Execute(deleteQuery1, new { Pasport = Namber_Pasport });

                        return rowsAffected > 0 ? 1 : -2;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
                return -3;
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Ошибка!");
                return -4;
            }
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="Docement"></param>
        /// <param name="LogIn"></param>
        /// <param name="Password"></param>
        /// <param name="PositionComboBox"></param>
        /// <param name="UserName"></param>
        /// <param name="UsetSname"></param>
        /// <param name="SMNamse"></param>
        /// <param name="Are"></param>
        /// <returns>
        ///  1 - Всё прошло по плану 
        /// -1 было найдено больша 1 акаунта с данным логином
        ///  0 нет подключание к базе данных
        /// -5 обработана ошибка - SqlException 
        /// -6 не изветсная ошибка
        /// </returns>
        public int CreateUser(string Docement, string LogIn, string Password,int PositionComboBox, string UserName, string UsetSname, string SMNamse = "", int Are = 20, int WareHouseID = 1)
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    try 
                    {

                        connection.Open();


                        var LogInTry = connection.Query<LogTable>("SELECT Log_In FROM LogTable where Log_In = @UserName", new { UserName = LogIn });
                        if (LogInTry.Count() > 0) {MessegeBox("Данный Логин уже сущесвует!", "Ошибка при создании"); return -1; }

                        var ChechDocument = @"SELECT Pasport FROM UserInfo where Pasport = @Pasport";
                        int ChechDocumentId = connection.ExecuteScalar<int>(ChechDocument,new { Pasport = Docement });
                        if(ChechDocumentId != 0)
                        {
                            MessageBox.Show("Данный пасторт уже введён в базу данных сети", "Отказанно",
                                       MessageBoxButton.OK, MessageBoxImage.Error); return-2; }

                        var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM UserInfo";
                        int maxId = connection.ExecuteScalar<int>(maxIdQuery);
                        int newId = maxId + 1;

                        var insertTableInfoQuery = @"
                INSERT INTO UserInfo (Id, Avotar, Post, Employee_Name, Last_Name, Patronymic, Age, Pasport, Info_Shop)
                VALUES (@Id,@Avotar, @Post, @Employee_Name, @Last_Name, @Patronymic, @Age, @Pasport,  @Info_Shop)";

                        string userPost = GetPositionFromComboBoxIndex(PositionComboBox);

                        connection.Execute(insertTableInfoQuery, new
                        {
                            Id = newId,
                            Post = userPost,
                            Avotar = "",
                            Employee_Name = UserName.Trim(),
                            Last_Name = UsetSname.Trim(),
                            Patronymic = SMNamse.Trim(),
                            Age = Are,
                            Pasport = Docement,
                            Info_Shop = WareHouseID 
                        });

                        var insertTableUserQuery = @"
                INSERT INTO LogTable (Log_In, Password_User, UserInfoId)
                VALUES (@Log_In, @Password_User, @UserInfoId)";

                        string password = SHIFT("Password", Password);

                        connection.Execute(insertTableUserQuery, new
                        {
                            Log_In = LogIn.Trim(),
                            Password_User = password,
                            UserInfoId = newId,
                        });

                        MessageBox.Show("Новый сотрудник был успешно добавлен!", "Успешно!",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                        return 1;
                    }
                    catch (SqlException ex)
                    {

                        var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM UserInfo";

                        MessegeBox(ex.Message + ex.StackTrace, "Ошибка!");
                        var insertTableUserQuery = @"DELETE FROM TableUser WHERE Id = @id;";
                        connection.Execute(insertTableUserQuery, new
                        {
                            Id = connection.ExecuteScalar<int>(maxIdQuery)

                    });
                    }
                }
                return 0;
            }
            catch (SqlException ex)
            {
                return -5;
            }
            catch (Exception ex)
            {

                return -6;
            }
        }

        /// <summary>
        /// Проверяет и разрешает / заприщает достуб к профилу по Логину и Поролу. 
        /// 
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 1 - Успешный вход 
        /// -1 было найдено больша 1 акаунта с данным логином
        /// -2 не верный логин или логин не сущесвует
        /// -3 не верный пороль
        /// -5 не изветсная ошибка (повторяет событие -1)
        /// </returns>
        public int LogIn(string Login, string password)
        {

           var conections = $"Server={NameServers};DataBase={NameBasaData};Trusted_connection=True;TrustServerCertificate=True;";
           string new_password = SHIFT("Password", password);

            using (SqlConnection connection = new SqlConnection(conections))
            {
                connection.Open();
                string NameUserDiolog = string.Empty;

                
                bool loginExists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM LogTable WHERE Log_In = @UserName",
                    new { UserName = Login }) > 0;

                bool passwordCorrect = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM LogTable li WHERE li.Log_In  = @UserName AND li.Password_User = @UserPosword",
                    new { UserName = Login, UserPosword = new_password }) > 0;

                // Получаем данные пользователя
                var ReadComand = connection.Query<LogTable>(
                    "SELECT tu.Log_In, tu.Password_User, tu.Id FROM LogTable tu WHERE tu.Log_In = @UserName AND tu.Password_User = @UserPosword",
                    new { UserName = Login, UserPosword = new_password });

                Console.WriteLine("Отладочная информация:");
                Console.WriteLine($"loginExists: {loginExists}");
                Console.WriteLine($"passwordCorrect: {passwordCorrect}");
                Console.WriteLine($"Количество найденных пользователей: {ReadComand.Count()}");

                if (loginExists && passwordCorrect)
                {
                    foreach (var item in ReadComand)
                    {
                        Console.WriteLine($"Инфомация:");
                        Console.WriteLine($"Найден: {item.Log_In}, ID = {item.UserInfoId}");
                        Console.WriteLine($"Введенный пароль: {password}");
                        Console.WriteLine($"Зашифрованный: {new_password}");
                        Console.WriteLine($"Пароль из БД: {item.Password_User}");

                        var UserInfo = connection.QueryFirstOrDefault<UserInfo>(
                            "SELECT * FROM UserInfo ui WHERE ui.Id = @Id",
                            new { Id = item.UserInfoId});

                        if (UserInfo != null)
                        {
                            NameUserDiolog = $"{UserInfo.Last_Name}. {UserInfo.Employee_Name[0]}. {UserInfo.Patronymic[0]}.";
                        }
                    }

                    MessageBox.Show(NameUserDiolog + " С возвращением!", "Добро пожаловать!",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    connection.Close();
                    return 1;
                }
                else if (loginExists && !passwordCorrect)
                {
                    Console.WriteLine($"============================\nERROR: \nLOG_IN - loginExists {loginExists} | passwordCorrect {passwordCorrect}\nNameUserDiolog = {NameUserDiolog}");
                   Application.Current.Shutdown()
                        ;
                    //ПРОДАМ ГАРАЖ 7 бибок
                    return -1;
                }
                else if (!loginExists)
                {
                    Console.WriteLine($"============================\nERROR: \nLOG_IN - loginExists {loginExists} | passwordCorrect {passwordCorrect}\nNameUserDiolog = {NameUserDiolog}");
                   
                    return -2;
                }
                else
                {
                    
                    Console.WriteLine($"============================\nERROR: \nLOG_IN - loginExists {loginExists} | passwordCorrect {passwordCorrect}\nNameUserDiolog = {NameUserDiolog}");
                   
                    return -5;
                }
            }
        }

        /// <summary>
        /// Пость пользователя
        /// </summary>
        /// <param name="Login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string UserPost(string Login, string password)
        {
            try
            {
                var connectionString = $"Server={NameServers};DataBase={NameBasaData};Trusted_Connection=True;TrustServerCertificate=True;";
                string new_password = SHIFT("Password", password);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Проверяем, есть ли пользователь в LogTable
                    var logTableUser = connection.QueryFirstOrDefault<dynamic>(
                        "SELECT Id, UserInfoId FROM LogTable WHERE Log_In = @UserName AND Password_User = @Password",
                        new { UserName = Login, Password = new_password });

                    if (logTableUser == null)
                    {
                        Console.WriteLine($"❌ Пользователь {Login} не найден в LogTable или неверный пароль");
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return "Гость";
                    }

                    Console.WriteLine($"✅ Найден в LogTable: ID={logTableUser.Id}, UserInfoId={logTableUser.UserInfoId}");

                    // 2. Проверяем UserInfoId
                    if (logTableUser.UserInfoId == null)
                    {
                        Console.WriteLine($"❌ У пользователя {Login} не заполнен UserInfoId");
                        return "Гость";
                    }

                    // 3. Получаем должность из UserInfo
                    string userPost = connection.QueryFirstOrDefault<string>(
                        "SELECT Post FROM UserInfo WHERE Id = @UserInfoId",
                        new { UserInfoId = logTableUser.UserInfoId });

                    if (!string.IsNullOrEmpty(userPost))
                    {
                        Console.WriteLine($"✅ Должность из UserInfo: {userPost}");
                        return userPost;
                    }
                    else
                    {
 
                        return "Гость";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка в UserPost: {ex.Message}");
                MessageBox.Show($"Ошибка получения прав: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return "Без роли";
            }
        }
        
        /// <summary>
        /// Вернёт таблицу в полном её объёме
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// Select * From [type ty] ORDER BY [Poremetry]
        /// </returns>
        public DataGridTextColumn Select_Table_Corrector(DataByse Table, TypeData type, string columnNameSQL, string columnNameProgramm)
        {
            switch (type)
            {
                case TypeData.Standart:
                   if(Table == DataByse.Product)
                    {
                        return SelectTableProduct(columnNameSQL, columnNameProgramm);
                    }
                    break;
                case TypeData.Icon:
                    if (Table == DataByse.Product)
                    {
                        return RegisterIcone(columnNameSQL, columnNameProgramm);
                    }
                    break;
                default:
                    return new DataGridTextColumn
                    {
                        Header = "NULL",
                        Binding = new Binding("NULL"),
                        Width = new DataGridLength(130)
                    };
            }
            return new DataGridTextColumn
            {
                Header = "NULL",
                Binding = new Binding("NULL"),
                Width = new DataGridLength(130)
            };
        }
        #region Обработка данных с Продукции
        private DataGridTextColumn Entity(string type, string columnNameSQL, string columnNameProgramm) 
        {
            switch (type) 
            {
                case "":
                    return SelectTableProduct(columnNameSQL, columnNameProgramm);
                    default:
                    return new DataGridTextColumn
                    {
                        Header = "NULL",
                        Binding = new Binding("NULL"),
                        Width = new DataGridLength(130)
                    };
            }
        }
        protected DataGridTextColumn RegisterIcone(string Path, string columnNameProgramm) 
        {
            try
            {
                // Убираем @ если есть
                string cleanColumnName = Path.StartsWith("@")
                    ? Path.Substring(1)
                    : Path;
                System.Windows.Controls.Image image = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(Path)) };


                DataGridTextColumn column = new DataGridTextColumn
                {
                    Header = columnNameProgramm,
                    // Binding = new Binding(image),
                    Width = new DataGridLength(130),
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Oblique,
                    FontSize = 20
                };

                return column;
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("URI"))
                {
                    MessageBox.Show("Не вышло загрузить URI / Изоброжение\nПодробности есть в Логировании", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return new DataGridTextColumn
                {
                    Header = columnNameProgramm+"[ERROR]",
                    Binding = new Binding(""),
                    Width = new DataGridLength(230)
                };
            }
        }
        protected DataGridTextColumn SelectTableProduct(string columnName = "@Id", string displayName = null)
        {
            try
            {
                // Убираем @ если есть
                string cleanColumnName = columnName.StartsWith("@")
                    ? columnName.Substring(1)
                    : columnName;

                // Если displayName не указан, используем cleanColumnName
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = cleanColumnName;
                }
                else if (displayName.StartsWith("@"))
                {
                    displayName = displayName.Substring(1);
                }

                // Просто создаем колонку - ВСЁ!
                DataGridTextColumn column = new DataGridTextColumn
                {
                    Header = displayName,
                    Binding = new Binding(cleanColumnName),
                    Width = new DataGridLength(130),
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Oblique,
                    FontSize = 20
                };

                return column;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                // Возвращаем колонку с ошибкой
                return new DataGridTextColumn
                {
                    Header = displayName+ "[ERROR]",
                    Binding = new Binding(""),
                    Width = new DataGridLength(130)
                };
            }
        }

        #endregion

        /// <summary>
        /// Быстрый запрос к таблице с продуктами, с группировкой данных
        /// </summary>
        /// <param name="orderBy"> Имя столбца по которой идёт группировка </param>
        /// <returns>Все данные с таблицы Products / Продукты </returns>
        public List<Products> LoadProductsData(string orderBy = "Id", string LogIn = "Admin")
        {
            try
            {
                var connectionString = $"Server={NameServers};Database={NameBasaData};Trusted_Connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Сначала ОБНОВЛЯЕМ статусы (если дата прошла)
                    string updateQuery = @"
                UPDATE Products 
                SET Product_Status = 'В наличии'
                WHERE ShopInforID IN ( 
                    SELECT Info_Shop 
                    FROM UserInfo ui
                    JOIN LogTable lt ON lt.UserInfoId = ui.Id
                    WHERE lt.Log_In = @LogIn
                ) 
                AND Product_DeliveryDate < CAST(GETDATE() AS DATE)
                AND Product_Status = 'Ожидается'";

                    connection.Execute(updateQuery, new { LogIn = LogIn });

                    // 2. Потом ЗАГРУЖАЕМ данные
                    // Проверяем безопасность параметра orderBy
                    string safeOrderBy = IsValidOrderBy(orderBy) ? orderBy : "Id";

                    string selectQuery = @"
                SELECT * 
                FROM Products prd 
                WHERE prd.ShopInforID IN (
                    SELECT Info_Shop 
                    FROM UserInfo ui 
                    WHERE ui.Id IN ( 
                        SELECT UserInfoId 
                        FROM LogTable lt 
                        WHERE lt.Log_In = @LogIn
                    )
                ) 
                ORDER BY " + safeOrderBy;

                    var products = connection.Query<Products>(selectQuery, new { LogIn = LogIn });

                    return products.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Products>();
            }
        }

        // Проверка, что orderBy безопасен (предотвращает SQL-инъекции)
        private bool IsValidOrderBy(string orderBy)
        {
            var allowedColumns = new HashSet<string>
    {
        "Id", "Product_Name", "Product_Status",
        "Product_DeliveryDate", "BayPrise", "Product_Place"
    };

            return allowedColumns.Contains(orderBy);
        }
        /// <summary>
        /// Быстрый запрос к таблице с продуктами, с группировкой данных
        /// </summary>
        /// <param name="orderBy"> Имя столбца по которой идёт группировка </param>
        /// <returns>Все данные с таблицы Products / Продукты </returns>
        public List<UserInfo> LoadUserInfoData(string orderBy = "Id")
        {
            try
            {
                var connectionString = $"Server={NameServers};DataBase={NameBasaData};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM UserInfo ORDER BY {orderBy}";
                    var products = connection.Query<UserInfo>(query);

                    connection.Close();
                    return products.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<UserInfo>();
            }
        }
    }
}

