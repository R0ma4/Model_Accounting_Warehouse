using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;


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


        public void CreateUser(string LogIn, string Password,int PositionComboBox, string UserName, string UsetSname, string SMNamse = "")
        {
            try
            {
                var connectionString = $"Server={main.Name_Server};Database={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    var LogInTry = connection.Query<TableUser>("SELECT UserName FROM TableUser where UserName = @UserName", new { UserName = LogIn });
                    if (LogInTry.Count() > 0) { MessegeBox("Данный Логин уже сущесвует!", "Ошибка при создании"); return; }

                    var maxIdQuery = @"SELECT ISNULL(MAX(Id), 0) FROM TableInfo";
                    int maxId = connection.ExecuteScalar<int>(maxIdQuery);
                    int newId = maxId + 1;

                    var insertTableInfoQuery = @"
                INSERT INTO TableInfo (Id, UserPost, EmployeeName, EmployeeSurname, EmployeeMiddleName)
                VALUES (@Id, @UserPost, @EmployeeName, @EmployeeSurname, @EmployeeMiddleName)";

                    string userPost = GetPositionFromComboBoxIndex(PositionComboBox);

                    connection.Execute(insertTableInfoQuery, new
                    {
                        Id = newId,
                        UserPost = userPost,
                        EmployeeName = UserName.Trim(),
                        EmployeeSurname = UsetSname.Trim(),
                        EmployeeMiddleName = SMNamse.Trim()
                    });

                    var insertTableUserQuery = @"
                INSERT INTO TableUser (UserName, UserPosword, ThisUser)
                VALUES (@UserName, @UserPosword, @ThisUser)";

                    string password = SHIFT("Password", Password);

                    connection.Execute(insertTableUserQuery, new
                    {
                        UserName = LogIn.Trim(),
                        UserPosword = password,
                        ThisUser = newId
                    });

                    MessageBox.Show("Новый сотрудник был успешно добавлен!", "Успешно!",
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
                    "SELECT COUNT(*) FROM TableUser WHERE UserName = @UserName",
                    new { UserName = Login }) > 0;

                bool passwordCorrect = connection.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM TableUser WHERE UserName = @UserName AND UserPosword = @UserPosword",
                    new { UserName = Login, UserPosword = new_password }) > 0;

                // Получаем данные пользователя
                var ReadComand = connection.Query<TableUser>(
                    "SELECT tu.UserName, tu.UserPosword, tu.Id FROM TableUser as tu WHERE UserName = @UserName AND UserPosword = @UserPosword",
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
                        Console.WriteLine($"Найден: {item.UserName}, ID = {item.ThisUser}");
                        Console.WriteLine($"Введенный пароль: {password}");
                        Console.WriteLine($"Зашифрованный: {new_password}");
                        Console.WriteLine($"Пароль из БД: {item.UserPosword}");

                        var UserInfo = connection.QueryFirstOrDefault<TableInfo>(
                            "SELECT * FROM TableInfo Where Id = @Id",
                            new { Id = item.ThisUser});

                        if (UserInfo != null)
                        {
                            NameUserDiolog = $"{UserInfo.EmployeeSurname}. {UserInfo.EmployeeName[0]}. {UserInfo.EmployeeMiddleName[0]}.";
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


        public string UserPost(string Login, string password)
        {

            var conections = $"Server={NameServers};DataBase={NameBasaData};Trusted_connection=True;TrustServerCertificate=True;";
            string new_password = SHIFT("Password", password);

            using (SqlConnection connection = new SqlConnection(conections))
            {
                string Post = null;
                connection.Open();
                string NameUserDiolog = string.Empty;


                bool loginExists = connection.ExecuteScalar<int>( "SELECT COUNT(*) FROM TableUser WHERE UserName = @UserName", new { UserName = Login }) > 0;

                bool passwordCorrect = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM TableUser WHERE UserName = @UserName AND UserPosword = @UserPosword", new { UserName = Login, UserPosword = new_password }) > 0;

                // Получаем данные пользователя
                var ReadComand = connection.Query<TableUser>("SELECT tu.UserName, tu.UserPosword, tu.Id FROM TableUser as tu WHERE UserName = @UserName AND UserPosword = @UserPosword", new { UserName = Login, UserPosword = new_password });

                Console.WriteLine("Отладочная информация:");
                Console.WriteLine($"loginExists: {loginExists}");
                Console.WriteLine($"passwordCorrect: {passwordCorrect}");
                Console.WriteLine($"Количество найденных пользователей: {ReadComand.Count()}");

                string IDUSerInfo = null;
                if (loginExists && passwordCorrect)
                {
                    foreach (var item in ReadComand)
                    {

                        IDUSerInfo = connection.QueryFirstOrDefault<string>("SELECT ti.UserPost FROM TableInfo ti  WHERE ti.Id = (SELECT tu.ThisUser FROM TableUser tu WHERE tu.UserName = @UserName )", new { UserName = item.UserName });
                    }
                    if (IDUSerInfo.ToString() != null) {
                        MessageBox.Show($"Вы вошли как - {IDUSerInfo}",null,MessageBoxButton.OK);
                        return IDUSerInfo;  }
                    else { MessageBox.Show("Гость - как по умолчанию"); }
                        connection.Close();
                    return null;
                }
                else if (loginExists && !passwordCorrect)
                {
                    return null;
                }
                else if (!loginExists)
                {
                    return null;
                }
                else
                {
                    return null;
                }
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

        #region Обработка данных с Продукции
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
    }
}

