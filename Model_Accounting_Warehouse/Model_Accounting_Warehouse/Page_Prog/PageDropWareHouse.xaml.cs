using Model_Accounting_Warehouse.Modul;
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
    /// Логика взаимодействия для PageDropWareHouse.xaml
    /// </summary>
    public partial class PageDropWareHouse : Page
    {
        API_MENEGER_DATABASE aPI;
        MainWindow main = Application.Current.MainWindow as MainWindow;
        int Id = 0;
        string strings = "";
        public PageDropWareHouse()
        {
            aPI = new API_MENEGER_DATABASE(main.Name_Server);
          
            InitializeComponent();
            ComboBoxData.Items.Clear();
            if (aPI != null)
            {
                foreach (var item in aPI.GetWarehouse()) { ComboBoxData.Items.Add($"{item.Id}) |{item.Street}"); strings = $"{item.Id}|{item.Street}"; }
            }
        }

        private void FrieWHButton_Click(object sender, RoutedEventArgs e)
        {
            string[] Samber = strings.Split('|');
            Id = int.Parse(Samber[0]);

            MessageBoxResult messageBoxResult = MessageBox.Show("Внимание ести сбросить (удалить) данную запись, будут автоматичиски списанны вся продукция и уволенны все сутрудники, перед работай перенесите или сохроните данные!","Будте осторожны!",MessageBoxButton.YesNo,MessageBoxImage.Asterisk);

            if (messageBoxResult == MessageBoxResult.Yes) 
            {
                if (Id > 0)
                {
                    int Key = aPI.DropWarehouse(Id);

                    ComboBoxData.Items.Clear();
                    if (aPI != null)
                    {
                        foreach (var item in aPI.GetWarehouse())
                        {
                            ComboBoxData.Items.Add(item.Street);
                        }
                    }
                    switch (Key)
                    {
                        case 0:
                            MessageBox.Show("Что-то пошло не так, смострите логированние", "Откатанно!", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 1:
                            MessageBox.Show("Склад был стёрт с Базы без возратно", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case -1:
                            MessageBox.Show("Что-то пошло не так, смострите логированние", "Откатанно! [-1]", MessageBoxButton.OK, MessageBoxImage.Stop);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Обязательно выберете склад из списка!", "Нужно быьрать склад!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
