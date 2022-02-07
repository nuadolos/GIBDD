using GIBDD.UI.Pages;
using GIBDD.Utilities;
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
using System.Windows.Threading;

namespace GIBDD
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Поля окна MainWindow

        DispatcherTimer timer;
        DateTime oneMinute;

        #endregion

        #region Конструктор окна MainWindow

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new Authorization());
            Transition.MainFrame = MainFrame;

            timer = new DispatcherTimer();

            timer.Tick += new EventHandler(TimerTickEvent);
            timer.Interval = new TimeSpan(1000);
        }

        #endregion

        #region Обратный переход по страницам

        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            if (Transition.MainFrame.CanGoBack)
                BackBtn.Visibility = Visibility.Visible;
            else
                BackBtn.Visibility = Visibility.Hidden;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.MainFrame.GoBack();
        }

        #endregion

        #region Активация таймера при бездействии

        public void TimerTickEvent(object sender, EventArgs e)
        {
            if (oneMinute <= DateTime.Now)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Transition.MainFrame.CanGoBack)
                        Transition.MainFrame.GoBack();
                    else
                    {
                        timer.Stop();
                        break;
                    }
                }
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            oneMinute = DateTime.Now.AddSeconds(10);
            timer.Start();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            timer.Stop();
        }

        #endregion
    }
}
