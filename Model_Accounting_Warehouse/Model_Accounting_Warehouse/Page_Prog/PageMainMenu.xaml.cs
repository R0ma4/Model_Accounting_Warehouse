using Dapper;
using Model_Accounting_Warehouse.Modul;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
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
using System.Windows.Threading;
using static Model_Accounting_Warehouse.Page_Prog.PageLoad;


namespace Model_Accounting_Warehouse.Page_Prog
{


    /// <summary>
    /// Логика взаимодействия для PageMainMenu.xaml
    /// </summary>
    public partial class PageMainMenu : Page
    {
        public static MainWindow main = Application.Current.MainWindow as MainWindow;
        Modul.API_MENEGER_DATABASE dATABASEAPI;

        public PageMainMenu()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (main != null)
            {
                dATABASEAPI = new Modul.API_MENEGER_DATABASE(main.Name_Server);
            }
        }
        string NameUserDiolog = string.Empty;
        private void KeyControlPage(object sender, KeyEventArgs e)
        {
          
            try
            {
                if (e.Key == Key.Enter)
                {
                    var conections = $"Server={main.Name_Server};DataBase={main.Name_Data_Base};Trusted_connection=True;TrustServerCertificate=True;";
                    dATABASEAPI.NameBasaData = main.Name_Data_Base;
                    if (NameUser.Text.Length <= 0) { MessageBox.Show("Имя не может быть пустым", "Ошибка входа в профиль администрации", MessageBoxButton.OK, MessageBoxImage.Error); }
                    if (PasswordUser.Password.Length <= 0)
                    {
                        if (PasswordUserCheck.Text.Length <= 0)
                        { MessageBox.Show("Пороль не может быть пустым", "Ошибка входа в профиль администрации", MessageBoxButton.OK, MessageBoxImage.Error); }
                        else { PasswordUser.Password = PasswordUserCheck.Text; }
                        return;
                    }

                    Console.WriteLine($"{dATABASEAPI.UserPost(NameUser.Text, PasswordUser.Password)} - Пост пользователя");
                    int KeyEnteProfil = dATABASEAPI.LogIn(NameUser.Text, PasswordUser.Password);

                    Console.WriteLine("Ключ входа в профиль: " + KeyEnteProfil);
                    switch (KeyEnteProfil)
                    {
                        case 1: main.PageControl.Content = new Page_Prog.PageWork(); break;
                    }
                }

            }
            catch 
            {

            }
                if (e.Key == Key.F5) { NameUser.Text = "Admin"; PasswordUser.Password = null; }
        }
    
        
        bool ChekPassword = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Скрыть показать пороль
            string content = string.Empty;
            if (ChekPassword)
            {
                content =  PasswordUserCheck.Text; 
                
                PasswordUserCheck.Visibility = Visibility.Collapsed;
                PasswordUser.Visibility = Visibility.Visible;
                PassworDutton.Content = "Показать пороль";
            }
            else 
            {
                content = PasswordUser.Password;

                PasswordUserCheck.Visibility = Visibility.Visible;
                PasswordUser.Visibility = Visibility.Collapsed;
                PassworDutton.Content = "Скрыть Пороль";
            }
            PasswordUserCheck.Text = PasswordUser.Password = content;
            ChekPassword = !ChekPassword;
        }
    }
}
