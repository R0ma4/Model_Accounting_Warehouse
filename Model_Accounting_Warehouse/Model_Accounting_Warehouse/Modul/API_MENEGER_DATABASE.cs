using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
// Пи пу пи пу пи ) 
using System.Net;
using System.Net.Http;

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
        public async Task<string> GetRegionByIPAsync()
        {
            string Region = "ru";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string response = await client.GetStringAsync("http://ip-api.com/json/");
                    // var json = JObject.Parse(response);

                    // return json["country"]?.ToString() + ", " + json["regionName"]?.ToString();
                    // // Пример: "Russia, Moscow"
                    return Region;
                }
            }
            catch
            {
                return "Не удалось определить регион";
            }
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
                default: return "Гость";
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

        public int AddProduct()
        {
            return 0;
        }

        public int AddNewSupplier(string Name_Supplier) 
        {
            return 0; 
        }

        /// <summary>
        /// Полностью сбрасывает базу данных
        /// </summary>
        public void DropDataBase() 
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

                    }
                    catch
                    {

                    }


                }
            }
            catch (SqlException ex) 
            {
                MessegeBox(ex.Message, "Ошибка!");
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
                        // Валидация ИНН
                        Regex innRegex = new Regex(@"^\d{10}$|^\d{12}$");
                        // Валидация номера счета
                        Regex accountRegex = new Regex(@"^\d{20}$");
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

                        //  if (!accountRegex.IsMatch(_BankName.Trim()))  {   return - 4; }


                        if (!bikRegex.IsMatch(_BIK.Trim())) { return - 3; }


                        if (!phoneRegex.IsMatch(_Phone.Trim()))  { return - 2;}


                        if (!emailRegex.IsMatch(_Email.Trim())) { return - 1; }

                        connection.Open();
                        var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM ShopInfo";
                        int maxId = connection.ExecuteScalar<int>(maxIdQuery);
                        int newId = maxId + 1;

                        var insertTableUserQuery = @"
                 INSERT INTO ShopInfo (Id, Street, Sales, INN, KPP, BankAccount, BankName, BIK, LegalAddress, Phone, Email)
                 VALUES 
                 (@Id, @Street, @Sales,   @INN, @KPP, @BankAccount,  @BankName, @BIK,@LegalAddress, @Phone, @Email)";

                        connection.Execute(insertTableUserQuery, new 
                        {
                            Id = newId,
                            Street = _Street,
                            Sales = 0, // По умолчанию 0 - так как при создании магазин не мог ничего заработать
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
        public void DropShop(int IdShop) // IdShop - Назначаеться из выподающего списка.
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
                    DELETE FROM LogTable WHERE Info_Shop = @ID;

                    DELETE FROM Products WHERE ShopInfor = @ID;

                    DELETE FROM ShopInfo WHERE Id = @ID;";

                        connection.Execute(insertTableUserQuery, new { ID = IdShop });

                    }
                    catch
                    {

                    }


                }
            }
            catch (Exception ex) 
            {

            }
            }

        /// <summary>
        /// Увольняет Пользователя по паспорту
        /// </summary>
        public int Fire(string Namber_Pasport)
        {
            try
            {
                Regex DateFormatRegex =  new Regex(@"^\d{2} \d{2} \d{4}$", RegexOptions.Compiled); 

                if(!DateFormatRegex.IsMatch(Namber_Pasport.Trim())) { return -1;  }

                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var Shin = @"SELECT Pasport FROM UserInfo where Pasport = @Pasport";

                   bool Corrects = connection.ExecuteScalar<int>(Shin, new { Pasport = Namber_Pasport }) == 1;

                    if(Corrects) 
                    {
                        var insertTableUserQuery = @"DELETE FROM UserInfo where Pasport = @Pasport";

                        var Correct = connection.Execute(insertTableUserQuery, new { Pasport = Namber_Pasport });
                        return 1;
                    }
                    else {  return -1; }
                }
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message, "Ошибка!");
            }
            catch (Exception ex)
            {
                MessegeBox(ex.Message+ ex.StackTrace, "Ошибка!");
            }
            return 0;
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
        public void CreateUser(string Docement, string LogIn, string Password,int PositionComboBox, string UserName, string UsetSname, string SMNamse = "", int Are = 20)
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
                        if (LogInTry.Count() > 0) { MessegeBox("Данный Логин уже сущесвует!", "Ошибка при создании"); return; }

                        var ChechDocument = @"SELECT Pasport FROM UserInfo where Pasport = @Pasport";
                        int ChechDocumentId = connection.ExecuteScalar<int>(ChechDocument,new { Pasport = Docement });
                        if(ChechDocumentId != 0)
                        {
                            MessageBox.Show("Данный пасторт уже введён в базу данных сети", "Отказанно",
                                       MessageBoxButton.OK, MessageBoxImage.Error); return; }

                        var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM UserInfo";
                        int maxId = connection.ExecuteScalar<int>(maxIdQuery);
                        int newId = maxId + 1;

                        var insertTableInfoQuery = @"
                INSERT INTO UserInfo (Id, Avotar, Post, Employee_Name, Last_Name, Patronymic, Age, Pasport)
                VALUES (@Id,@Avotar, @Post, @Employee_Name, @Last_Name, @Patronymic, @Age, @Pasport)";

                        string userPost = GetPositionFromComboBoxIndex(PositionComboBox);

                        connection.Execute(insertTableInfoQuery, new
                        {
                            Id = newId,
                            Post = userPost,
                            Avotar = "C:\\Users\\Asus\\Downloads\\avatardefault_92824.png",
                            Employee_Name = UserName.Trim(),
                            Last_Name = UsetSname.Trim(),
                            Patronymic = SMNamse.Trim(),
                            Age = Are,
                            Pasport = Docement
                        });

                        var insertTableUserQuery = @"
                INSERT INTO LogTable (Log_In, Password_User, UserInfoId,Info_Shop)
                VALUES (@Log_In, @Password_User, @UserInfoId, @Info_Shop)";

                        string password = SHIFT("Password", Password);

                        connection.Execute(insertTableUserQuery, new
                        {
                            Log_In = LogIn.Trim(),
                            Password_User = password,
                            UserInfoId = newId,
                            Info_Shop = 1 // Как стандартый индекс
                        });

                        MessageBox.Show("Новый сотрудник был успешно добавлен!", "Успешно!",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
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
            }
            catch (SqlException ex)
            {
                MessegeBox(ex.Message + ex.StackTrace, "Ошибка!");
            }
            catch (Exception ex)
            {
                MessegeBox(ex.Message+ ex.StackTrace, "Ошибка!");
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
                            "SELECT ui.Last_Name FROM UserInfo ui WHERE ui.Id = @Id",
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
                    MessageBox.Show("Неверный пароль!", "Отказано",MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
                else if (!loginExists)
                {
                    MessageBox.Show("Неверный логин!", "Отказано", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -2;
                }
                else
                {
                    MessageBox.Show("Неизвестный случай, смотрите логирование", "Отказано", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    // ВАРИАНТ 1: Через JOIN (рекомендуется)
                    string post = connection.QueryFirstOrDefault<string>(@"
                SELECT ui.Post 
                FROM LogTable lt
                INNER JOIN UserInfo ui ON lt.UserInfoId = ui.Id
                WHERE lt.Log_In = @UserName AND lt.Password_User = @Password",
                        new
                        {
                            UserName = Login,
                            Password = new_password
                        });

                    if (!string.IsNullOrEmpty(post))
                    {
                        Console.WriteLine($"Должность пользователя {Login}: {post}");
                        return post;
                    }

                    // ВАРИАНТ 2: Если JOIN не сработал, проверяем шаг за шагом
                    Console.WriteLine("JOIN не дал результата, проверяем вручную...");

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
                        Console.WriteLine($"❌ Не найдена должность для UserInfoId={logTableUser.UserInfoId}");
                        return "Гость";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка в UserPost: {ex.Message}");
                MessageBox.Show($"Ошибка получения прав: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return "Гость";
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
                    Image image = new Image() { Source = new BitmapImage(new Uri(Path)) };


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
        public List<Products> LoadProductsData(string orderBy = "Id")
        {
            try
            {
                var connectionString = $"Server={NameServers};DataBase={NameBasaData};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM Products ORDER BY {orderBy}";
                    var products = connection.Query<Products>(query);

                    connection.Close();
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

