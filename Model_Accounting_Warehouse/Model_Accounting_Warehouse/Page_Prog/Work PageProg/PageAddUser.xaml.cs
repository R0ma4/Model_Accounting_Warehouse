using Dapper;
using Model_Accounting_Warehouse.Modul;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
    /// Логика взаимодействия для PageAddUser.xaml
    /// </summary>
    /// 
    public partial class PageAddUser : Page
    {
        
        public PageAddUser()
        {
            InitializeComponent();


        }
        public static MainWindow main;
        Modul.API_MENEGER_DATABASE dATABASEAPI;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            main = Application.Current.MainWindow as MainWindow;
            dATABASEAPI = new Modul.API_MENEGER_DATABASE(main.Name_Server);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(LoginTextBox.Text) || LoginTextBox.Text.Length < 0) { MessegeBox("Имя не может быть пустым","Ошибка в логене пользователя"); return; }
            if(string.IsNullOrEmpty(PasswordBox.Password) ||  PasswordBox.Password.Length < 0) { MessegeBox("Пороль не может быть пустым", "Ошибка при создании пороля пользователя"); return; }
            if(PasswordBox.Password != ConfirmPasswordBox.Password) { MessegeBox("Пороли не совподают", "Пороли не совподают"); return; }
            if(string.IsNullOrEmpty(SurnameTextBox.Text) || SurnameTextBox.Text.Length < 0) { MessegeBox("Фамилия у пользователя - 100% Есть!", "Ошибка регестрации пользователя"); return; }
            if(string.IsNullOrEmpty(NameTextBox.Text) || NameTextBox.Text.Length < 0) { MessegeBox("Фамилия у пользователя - 100% Есть!", "Ошибка регестрации пользователя"); return; }



            dATABASEAPI.CreateUser(LoginTextBox.Text, PasswordBox.Password,PositionComboBox.SelectedIndex, NameTextBox.Text, SurnameTextBox.Text, MiddleNameTextBox.Text);
        }
        // MiddleNameTextBox


        /*
          public string UserPost { get; set; }
          public string EmployeeName { get; set; }
          public string EmployeeSurname { get; set; }
          public string EmployeeMiddleName { get; set; }
         */
       

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
                messeg,title,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Error
                );
        }

    }
}
