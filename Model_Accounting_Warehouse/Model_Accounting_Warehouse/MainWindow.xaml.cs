using Model_Accounting_Warehouse.Modul;
using Model_Accounting_Warehouse.Page_Prog;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Model_Accounting_Warehouse
{


    // Сущность для Базы данных 
    public class TableUser
    {
        public string UserName { get; set; }
        public string UserPosword { get; set; }
        public int ThisUser { get; set; }
        public ICollection<TableInfo> UserInfoLost { get; set; }
        public TableUser() { }
    }

    public class TableInfo
    {
        public int Id { get; set; }
        public string UserPost { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeMiddleName { get; set; }
    }

    public class Products
    {
        public int Id { get; set; }
        public string Product_icon { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public string Product_Cotigory { get; set; }
        public int Product_Remains { get; set; }
        public DateTime Product_DeliveryDate { get; set; }
        public string Product_Department { get; set; }
        public string Product_Place { get; set; }
        public int Product_Supplier { get; set; }
        public int Product_Pay { get; set; }
    }

    public class Supplier   
    {
        public int Id { get; set; }
        public string Supplier_Name { get; set; }
    }

    public class EmtityPrise
    {
        public int Id { get; set; }
        public decimal GlobalPrise { get; set; }
        public decimal BayPrise { get; set; }
        public decimal ShopPayPrise { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Окно и Обновления
        Modul.ConfigurationReder Reg = new Modul.ConfigurationReder();
        private DispatcherTimer Updater;
        public int Drop_Tick = 150;
        public int Tick = 0;
        public int Time_Tick = 3000;
        public bool FoneRegister = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Reg.Puth_Ini_Roul = Puth_Ini_Roul;

            Updater = new DispatcherTimer();
            Updater.Interval = TimeSpan.FromMilliseconds(Time_Tick);
            Updater.Tick += UpdateEditor;
            Updater.Start();

            switch (PageMainStart)
            {
                case "Log In":
                    PageControl.Content = new Page_Prog.PageMainMenu();
                    break;
                case "Register":
                    // Страница регестрации
                    PageControl.Content = new Page_Prog.Work_PageProg.PageAddUser();
                    break;
                case "Settengs":
                    // Страница настроек (без достуба к БД)
                    break;
                case "Load":
                    // Страница Загрузки
                    PageControl.Content = new Page_Prog.PageLoad();
                    break;
                default:

                    break;
            }
        }

        public Page_Prog.Work_PageProg.PageTableProduct tableProduct;
        public Page_Prog.Work_PageProg.PageAddUser pageAddUser; 
        public Page_Prog.Work_PageProg.PageEditProduct pageEdit;
        public Page_Prog.Work_PageProg.Page404Error page404Error;
        public Page_Prog.PageMainMenu pageMM;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var config = new ConfigurationReder.AppConfig();
            
            pageEdit = new Page_Prog.Work_PageProg.PageEditProduct();
            tableProduct = new Page_Prog.Work_PageProg.PageTableProduct();
            pageMM = new Page_Prog.PageMainMenu();
            pageAddUser = new Page_Prog.Work_PageProg.PageAddUser();
            page404Error = new Page_Prog.Work_PageProg.Page404Error();

            // Настройка цветовой политрый страницы из ini файла

            Reg.FixElementSizes(tableProduct.GridWindow, config);
            Reg.FixElementSizes(pageAddUser.GridWindow, config);
            Reg.FixElementSizes(pageEdit.GridWindow, config);
            Reg.FixElementSizes(page404Error.GridWindow, config);
            Reg.FixElementSizes(pageMM.GridWindow, config);
        }

        public string Name_Server = "HOME-PC\\SQLEXPRESS";
        public string Name_Data_Base = "Model_Accounting_Warehouse_DataBase";

        public string PageMain = "Log In";
        public string PageMainStart = "Log In";

        public string Puth_Ini_Roul = @"D:\Model_Accounting_Warehouse\Model_Accounting_Warehouse\Model_Accounting_Warehouse\Rouls";



        #region Изоброжения 
        public string Image = "";
        public List<string> Images = new List<string>();
        #endregion

        #region Файлы Поведений 
        public bool MAIN_CONFIG_ROULS = false;
        public string DawerSettengFile { get; } = "";
        public string SettengFile { get; } = "Main_Config.ini";
        public string BlackList { get; } = "BlackList.json";
        public string Programm { get; } = "Programm.yami";

        #endregion

        #region Файлы
        public string GlobalDirectProgramm = "";
        #endregion

        #region Цвета
        public Color PROGRAM_BACKGROUND;
        public Color PROGRAM_FROGRAM;
        public Color PROGRAM_BUTTON;
        public Color PRODUCT_TABLE_BACKGROUND;
        public Color PRODUCT_TABLE_PROGRAM_FROGRAM;
        #endregion
        public bool TIME_OUT_EXIT = false;


        private void UpdateEditor(object sender, EventArgs e) 
        {



            Console.WriteLine($"Tikc -> {Tick}"); Tick++; 

            if(Tick > Drop_Tick)
            {
                Tick = 0;
                if (TIME_OUT_EXIT) { Application.Current.Shutdown(); }
                else 
                {
                    switch (PageMain)
                    {
                        case "Log In":
                            PageControl.Content = new Page_Prog.PageMainMenu();
                            break;
                        case "Register":
                            // Страница регестрации
                            PageControl.Content = new Page_Prog.Work_PageProg.PageAddUser();
                            break;
                        case "Settengs":
                            // Страница настроек (без достуба к БД)
                            break;
                        case "Load":
                            // Страница Загрузки
                            PageControl.Content = new Page_Prog.PageLoad();
                            break;
                        default:

                            break;
                    }
                }
            }
        }

     
        private void MouseMoveModul(object sender, MouseEventArgs e)
        {
            Tick = 0;
        }

        public class AppConfig
        {

            public MainWindow main = Application.Current.MainWindow as MainWindow;
            public AppConfig()
            {

                if (main != null)
                {
                    main.Content = new PageMainMenu();
                }
            }
        }
    }
}
