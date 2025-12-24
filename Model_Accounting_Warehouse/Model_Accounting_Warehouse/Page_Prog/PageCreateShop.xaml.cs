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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Model_Accounting_Warehouse.Page_Prog
{
    /// <summary>
    /// Логика взаимодействия для PageCreateShop.xaml
    /// </summary>
    public partial class PageCreateShop : Page
    {
        public static MainWindow main;
        Modul.API_MENEGER_DATABASE dATABASEAPI;
        public PageCreateShop()
        {
            InitializeComponent();
            main = Application.Current.MainWindow as MainWindow;
            dATABASEAPI = new Modul.API_MENEGER_DATABASE(main.Name_Server);

            bool Remerd = Standaer();
            if (!Remerd) 
            {
                MessageBox.Show("Не вышло потянуть стандартные / последние данные склада","Прерванно!",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
            }
        }

        bool Standaer() 
        {
            try 
            {
                foreach (var item in dATABASEAPI.GetWarehouse())
                {
                    NameShopToSklad.Text = item.Street;
                    INNShop.Text = item.INN;
                    KPPShop.Text = item.KPP;
                    NamberBankAccount.Text = item.BankAccount;
                    NamberBIKShop.Text = item.BIK;
                    LegalAddressShop.Text = item.LegalAddress;
                    Phone.Text = item.Phone;
                    Emale.Text = item.Email;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        string CorectNameBank(int index_banck) 
        {
            switch (index_banck) 
            {
                case 1: return "ПОА СберБанк";
                case 2: return "ВТБ";
                case 3: return "Газпромбанк";
                case 4: return "Альфа-Банк";
                case 5: return "Райффайзенбанк";
                case 6: return "Промсвязьбанк";
                case 7: return "Московский кредитный банк";
                case 8: return "Россельхозбанк";
                case 9: return "Тинькофф Банк";
                case 10: return "ВТБ";
                case 11: return "Открытие";
                case 12: return "Совкомбанк";
                case 13: return "Банк Уралсиб";
                case 14: return "Промсвязьбанк";
                case 15: return "ЮниКредит Банк\"";
                case 16: return "Росбанк";
                default: return "Не обработанный Банк";
            }

        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e) 
        {
            if(string.IsNullOrEmpty(NameShopToSklad.Text) || string.IsNullOrEmpty(INNShop.Text) || string.IsNullOrEmpty(NamberBIKShop.Text) ||
               string.IsNullOrEmpty(KPPShop.Text) || string.IsNullOrEmpty(NamberBankAccount.Text) || 
               string.IsNullOrEmpty(NameShopToSklad.Text) || string.IsNullOrEmpty(LegalAddressShop.Text) ||
               string.IsNullOrEmpty(Phone.Text) || string.IsNullOrEmpty(BankComboBox.Text) || string.IsNullOrEmpty(Emale.Text) || BankComboBox.SelectedIndex == 0) 
            {
                MessageBox.Show(
                    "При регестрации все элементы должны быть заполнены! и Выбранны!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop
                    );
                return;
            }

         int KEY_SHOP_CREATE = dATABASEAPI.CreateShop(
             "\'ООО\' Мой Магазин и Склад", // Имя Компании / Магазина / Склада (Пока по стандарту  " 'ООО' Мой Магазин и Склад ")
             NameShopToSklad.Text,
             INNShop.Text,
             KPPShop.Text,
             NamberBankAccount.Text,
             CorectNameBank(BankComboBox.SelectedIndex),
             NamberBIKShop.Text,
             LegalAddressShop.Text,
             Phone.Text, Emale.Text
             );

            switch (KEY_SHOP_CREATE) 
            {
                case 0:
                    MessageBox.Show("Создание Магазина вызвало исключение или создание невозможно!","NULL",MessageBoxButton.OK,MessageBoxImage.Hand);
                    break;
                case 1:
                    MessageBox.Show($"Запись успешно добавленна с таблицу Базы Данных!", "Завершинно!", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                // Обработка ключей с Ошибкой: 
                case -1:
                    MessageBox.Show($"Электронная почта, имела не веррный формат", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -2:
                    MessageBox.Show($"Номер телефона имела не веррный формат", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -3:
                    MessageBox.Show($"БИК имел не веррный формат", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -4:
                    MessageBox.Show($"Не корректно введён номаер счёта", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -5:
                    MessageBox.Show($"КПП имел не веррный формат", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case -6:
                    MessageBox.Show($"ИНН Имел не верный формат", "Прерванно", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
