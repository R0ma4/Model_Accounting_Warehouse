using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using static Model_Accounting_Warehouse.Page_Prog.PageLoad;

namespace Model_Accounting_Warehouse.Modul
{
    public class ConfigurationReder
    {
        #region Очень важные переменные

        protected string Begraund = "#000034";
        protected string Fontgund = "#FF0800";
        protected string ButtonGraud = "#E3B505";


        #endregion


        public class AppConfig
        {

            public MainWindow main = Application.Current.MainWindow as MainWindow;
            public AppConfig() 
            {

            }

            public static MainWindow Prise(MainWindow main)
            {
                return main = Application.Current.MainWindow as MainWindow;
            }
        }

        private DispatcherTimer Updater;
        public ProgressBar ProgressBarStaber;
        public string Puth_Ini_Roul;
        public ConfigurationReder()
        {
            Console.Write("START _ READER ");
            Updater = new DispatcherTimer();
            Updater.Interval = TimeSpan.FromMilliseconds(1);
            Updater.Tick += UpdateEditor;
            Updater.Start();
        
        }
        #region Кароче, то что нужно вернуть) 


       

        #endregion


        private void UpdateEditor(object sender, EventArgs e)
        {
            try
            {
                AppConfig config = new AppConfig();
                string[] lines = File.ReadAllLines(Puth_Ini_Roul + @"\" + config.main.SettengFile);

                string currentBlock = null;
                int totalLines = lines.Length;
                int currentLine = 0;

                foreach (string line in lines)
                {
                    currentLine++;
                    string trimmedLine = line.Trim();

                    // Обновляем прогресс-бар на основе процента выполнения
                    double progress = (double)currentLine / totalLines * 100;
                    if (ProgressBarStaber != null)
                    {
                        ProgressBarStaber.Value = Math.Min(progress, 100);
                    }

                    if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith(";") || trimmedLine.StartsWith("#")) { }
                    else
                    {
                        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                        {
                            currentBlock = trimmedLine.Substring(1, trimmedLine.Length - 2).Trim();
                            continue;
                        }

                        if (currentBlock != null && trimmedLine.Contains('='))
                        {
                            string[] parts = trimmedLine.Split('=');
                            if (parts.Length >= 2)
                            {
                                string key = parts[0].Trim();
                                string[] value = parts[1].Split(';');
                                value[0] = value[0].Trim();
                                switch (currentBlock)
                                {
                                    case "Castomizaut":
                                        ColorConfig(config, key, value[0]);
                                        break;
                                    case "Window":
                                        Console.WriteLine("КЛЮЧ ОКНА: ", key, value[0]);
                                        ProcessWindowConfig(config, key, value[0]);
                                        break;
                                    case "Rouls":
                                        ProcessRoulsConfig(config, key, value[0]);
                                        // Обработка Rouls
                                        break;
                                    case "File":
                                        // Обработка File
                                        break;
                                    case "Icone":
                                        // Обработка Icone
                                        break;
                                    case "Procces":
                                        // Обработка Icone
                                        break;

                                    default:
                                        ShowBlockError(currentBlock);
                                        break;
                                }
                            }
                        }
                    }
                    Console.WriteLine(line);
                }

                if (ProgressBarStaber != null)
                {
                    ProgressBarStaber.Value = 100;
                }
                Updater.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+'\n'+ex.StackTrace, "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();

            }
        }

        public void ColorConfig(AppConfig config, string key, string value)
        {
            value = value.Trim().Replace("\'", null);
            Console.WriteLine("Обноружен пораметор" + key + " = " + value);


            Begraund = value;
            Console.WriteLine($"КЛЮЧ ЦВЕТА: {value} От пораметра {key}");
            
            switch (key)
            {
                case "PROGRAM_BACKGROUND":
                    Begraund = value;
                    break;
                case "PROGRAM_FRAGEGROUND":
                    Fontgund = value;
                    break;
                case "PROGRAM_BUTTON":
                    ButtonGraud = value;
                    break;
                default:
                    ShowParameterError(key, "Window");
                    break;
            }
        }

        public void ProcessWindowConfig(AppConfig config, string key, string value)
        {
            Console.WriteLine("Обноружен пораметор"+ key);
            switch (key)
            {
                case "WINDOW_HEIGHT":
                    if (int.TryParse(value, out int height)) {
                        if (height > 100 && height < 2000)
                        {
                            Console.WriteLine("Высота " + height);
                            config.main.MinHeight = height;
                            Console.WriteLine("Высота экрана " + config.main.MinHeight);

                        }
                        else
                        {
                            ShowParameterValueError(value, "Window");
                            Application.Current.Shutdown();
                        }
                    };
                    break;
                case "WINDOW_WIDTH":
                    if (int.TryParse(value, out int width)) 
                    {
                        if (width > 100 && width < 2000)
                        {
                            config.main.MinWidth = width;
                        }
                        else
                        {
                            ShowParameterValueError(key, "Window");
                            Application.Current.Shutdown();
                        }
                    }
                    ;
                    break;
                case "WINDOW_PANEL":
                    if (bool.TryParse(value, out bool PanelStatus)) 
                    {
                        if (PanelStatus)
                        {
                            config.main.WindowStyle = WindowStyle.SingleBorderWindow;
                        }
                        else if (!PanelStatus) 
                        {
                            config.main.WindowStyle = WindowStyle.None;
                        }
                        else
                        {
                            ShowParameterValueError(key, "Window");
                            Application.Current.Shutdown();
                        }
                    }
                    ;
                    break;
                case "DROP_TICK":
                    if (int.TryParse(value, out int dropTick)) config.main.Drop_Tick = dropTick;
                    break;
                case "FONEREGISTER":
                    if (bool.TryParse(value, out bool foneRegister))
                    {
                        Console.WriteLine("SS^ "+value);
                        if (value == "false" || value == "true") { config.main.FoneRegister = foneRegister; }
                        else 
                        {
                            ShowParameterValueError(value, "Rouls");
                            Application.Current.Shutdown();
                        }
                    }
                    break;
                case "MAXINIZITE":
                    if (bool.TryParse(value, out bool Max))
                    {
                        Console.WriteLine("SS^ " + value);
                        if (value == "false" || value == "true") { config.main.WindowState = WindowState.Maximized; }
                        else
                        {
                            config.main.WindowState = WindowState.Normal;
                        }
                    }
                    break;
                case "MINNIZITE":
                    if (bool.TryParse(value, out bool Min))
                    {
                        Console.WriteLine("SS^ " + value);
                        if (value == "false" || value == "true") { config.main.WindowState = WindowState.Minimized; }
                      
                        else
                        {
                            config.main.WindowState = WindowState.Normal;
                        }
                    }
                    break;
                case "TIME_TICK":
                    if (int.TryParse(value, out int timeTick)) config.main.Time_Tick = timeTick;
                    break;
                default:
                    ShowParameterError(key, "Window");
                    break;
            }
        }

        public void ProcessRoulsConfig(AppConfig config, string key, string value)
        {
            Console.WriteLine("Обноружен пораметор" + key);
            switch (key)
            {
                case "IMAGE":
                    config.main.Image = value;
                    break;
                case "GLOBALDIRECT":
                    if (Directory.Exists(value))
                    {
                        config.main.Image = value;
                    }
                    else
                    {
                        ShowParameterValueError(value, "Window");
                        Application.Current.Shutdown();
                    }
                    break;
                case "MAIN_PAGE":
                    config.main.PageMain = value;
                    break;
                case "START_PAGE":
                    config.main.PageMainStart = value;
                    break;
                case "TIMEOUT_EXIT":
                        Console.WriteLine("Time Uot Clouser " + value);
                        
                    if (bool.TryParse(value, out bool TimeOutExit))
                    {
                        if (TimeOutExit == false || TimeOutExit == true) { config.main.TIME_OUT_EXIT = TimeOutExit; }
                        else
                        {
                            ShowParameterValueError(value, "Window");
                            Application.Current.Shutdown();
                        }
                    }
                    break;
                case "DIRECTORI_MAIN":
                    if (Directory.Exists(value) || value != "standart")
                    {
                        config.main.Puth_Ini_Roul = value;
                    }
                    else
                    {
                        ShowParameterValueError(value, "Window");
                        Application.Current.Shutdown();
                    }
                    break;
                default:
                    ShowParameterError(key, "Window");
                    break;
            }
        }

        public void ShowParameterValueError(string parameter, string block)
        {
            MessageBox.Show(
                $"{parameter}"+ " - Имел не верные настройки\n" +
                "Проверьте правильность написания параметра, или обратитесь к документации программы",
                "Ошибка чтения параметра",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        // Метод для получения всех элементов
        private List<FrameworkElement> GetAllElements(DependencyObject parent)
        {
            var list = new List<FrameworkElement>();

            if (parent is FrameworkElement fe) list.Add(fe);

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                list.AddRange(GetAllElements(child));
            }

            return list;
        }
        public void FixElement(Grid grid)
        {
            // Список типов, у которых есть Background
            foreach (FrameworkElement element in GetAllElements(grid))
            {
                // Проверяем основные типы
                if (element is Control || element is Border || element is Panel)
                {
                    var brush = GetBackgroundBrush(element);
                    if (brush != null && brush.Color.A > 0)
                    {
                        Console.WriteLine($"Element Correct: {element.Name ?? element.GetType().Name} : {element.GetType().Name} Color: [ ButtonGraud = {ButtonGraud}| Fontgund = {Fontgund} | Begraund = {Begraund}] ");
                        if (element is Button button)
                        {
                            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Begraund));// Begraund - string
                            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                        if (element is TextBlock textBlock)
                        {
                            textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                        if (element is TextBox textbox)
                        {
                            textbox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ButtonGraud));
                            textbox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                        if (element is ComboBox comboBox)
                        {
                            comboBox.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ButtonGraud));
                            comboBox.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                        if (element is ComboBoxItem comboBoxItem)
                        {
                            // comboBoxItem.Background = new SolidColorBrush(Begraund);
                            comboBoxItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                        if (element is Grid grid_color)
                        {
                           //  grid_color.Background = new SolidColorBrush(Begraund);
                        }
                        if (element is Menu menu)
                        {
                         //   menu.Background = new SolidColorBrush(Begraund);
                            menu.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }

                        if (element is MenuItem menuItem)
                        {
                          //  menuItem.Background = new SolidColorBrush(Begraund);
                            menuItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Fontgund));
                        }
                    }
                }
            }
        }

        private SolidColorBrush GetBackgroundBrush(FrameworkElement element)
        {
            if (element is Control c) return c.Background as SolidColorBrush;
            if (element is Border b) return b.Background as SolidColorBrush;
            if (element is Panel p) return p.Background as SolidColorBrush;
            return null;
        }


        private void ShowParameterError(string parameter, string block)
        {
            MessageBox.Show(
                $"{parameter}"+" - Не является аргументом блока "+$"{block}"+"\n" +
                "Проверьте правильность написания параметра, или лбратитесь к документации программы",
                "Ошибка чтения параметра",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        public void ShowBlockError(string block)
        {
            MessageBox.Show(
                $"{block}"+ " - Неизвестный блок конфигурации\n" + 
                "Проверьте правильность написания параметра, или лбратитесь к документации программы",
                "Ошибка чтения блока",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}
