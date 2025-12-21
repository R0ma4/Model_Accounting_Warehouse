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
    public class LogTable
    {
        public string Log_In { get; set; }
        public string Password_User { get; set; }
        public int UserInfoId { get; set; }
        public ICollection<UserInfo> UserInfoLost { get; set; }
        public int Info_Shop { get; set; }
        public ICollection<ShopInfo> ShopInfoLost { get; set; }
        public LogTable() { }
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Avotar { get; set; }
        public string Post { get; set; }
        public string Employee_Name { get; set; }
        public string Last_Name { get; set; }
        public string Patronymic { get; set; }
        public string Pasport { get; set; }
        public int Are { get; set; }
    }

    public class Products
    {
        public int Id { get; set; }
        public string Product_icon { get; set; }
        public string Product_Name { get; set; }
        public string Product_Description { get; set; }
        public string Product_Cotigory { get; set; }
        public int Product_Remains { get; set; }
        public string Product_Status { get; set; }
        public DateTime Product_DeliveryDate { get; set; }
        public string Product_Place { get; set; }
        public int Product_Are_Pay { get; set; }
        public int Product_Supplier { get; set; }
        public int Product_Pay { get; set; }
        public int ShopInfor { get; set; }
    }

    public class Supplier
    {
        // Если в таблице столбец называется "Id"
        public int Id { get; set; }

        // Если в таблице "Supplier_Name"
        public string Supplier_Name { get; set; }

    }
    public class ShopInfo   
    {
        public int Id { get; set; }
        public string Supplier_Name { get; set; }
        public double Sales { get; set; }

        public string INN { get; set; }
        public string KPP { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BIK { get; set; }
        public string LegalAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }

    public class EmtityPrise
    {
        public int Id { get; set; }
        public double GlobalPrise { get; set; }
        public double BayPrise { get; set; }
        public double ShopPayPrise { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Окно и Обновления
       public Modul.ConfigurationReder Reg  = new Modul.ConfigurationReder();
        private DispatcherTimer Updater;
        public int Drop_Tick = 150;
        public int Tick = 0;
        public int Time_Tick = 3000;
        public bool FoneRegister = false;
        #endregion

        public MainWindow()
        {
            
           // Task.Delay(100);
            InitializeComponent();
           
            this.Loaded += MainWindow_Loaded;
            Reg.Puth_Ini_Roul = Puth_Ini_Roul;

            Updater = new DispatcherTimer();
            Updater.Interval = TimeSpan.FromMilliseconds(Time_Tick);
            Updater.Tick += UpdateEditor;
            Updater.Start();
            
            


          
        }

        public Page_Prog.Work_PageProg.PageTableProduct tableProduct;
        public Page_Prog.Work_PageProg.PageAddUser pageAddUser; 
        public Page_Prog.Work_PageProg.PageEditProduct pageEdit;
        public Page_Prog.Work_PageProg.Page404Error page404Error;
        public Page_Prog.PageMainMenu pageMM;
        public Page_Prog.PageLoad pageLoad;
        public Page_Prog.PageWork pageWork;
        public Page_Prog.PageCreateShop pageCreateShop;
        public Page_Prog.Work_PageProg.FierEmployee pageFierEmployee;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var config = new ConfigurationReder.AppConfig();

            pageMM = new PageMainMenu();
            pageWork = new PageWork();
            pageLoad = new PageLoad();

            pageFierEmployee = new Page_Prog.Work_PageProg.FierEmployee();
            pageEdit = new Page_Prog.Work_PageProg.PageEditProduct();
            tableProduct = new Page_Prog.Work_PageProg.PageTableProduct();
            pageMM = new Page_Prog.PageMainMenu();
            pageAddUser = new Page_Prog.Work_PageProg.PageAddUser();
            page404Error = new Page_Prog.Work_PageProg.Page404Error();

            pageCreateShop = new PageCreateShop();
            // Настройка цветовой политрый страницы из ini файла
            PageControl.Content = pageMM;
            PageControl.Content = pageAddUser;
            PageControl.Content = pageLoad;
            PageControl.Content = pageEdit;

            // Reg.FixElement(pageMM.GridWindow); ИСПРОВЛЕНИЕ, [#000000 (Значения не приравневаються, возможно ставяться после создания ключа Rig - так как у них нет значения по умолчанию) ]

            // Первая страница - по правиилам .ini файла
            switch (PageMainStart)
            {
                case "CreateShop":
                    // Страница регистрации Магазина
                    PageControl.Content = pageCreateShop;
                    break;
                case "Log In":
                    PageControl.Content = pageMM;
                    break;
                case "Register":
                    // Страница регестрации
                    PageControl.Content = pageAddUser;
                    break;
                case "Settengs":
                    // Страница настроек (без достуба к БД)
                    break;
                case "Load":
                    // Страница Загрузки
                    PageControl.Content = pageLoad;
                    break;
                default:

                    break;
            }
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
        public string GlobalDirectProgramm = "_d";
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
                            PageControl.Content = pageMM;
                            break;
                        case "Register":
                            // Страница регестрации
                            PageControl.Content = pageAddUser;
                            break;
                        case "Settengs":
                            // Страница настроек (без достуба к БД)
                            break;
                        case "Load":
                            // Страница Загрузки
                            PageControl.Content = pageLoad;
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
