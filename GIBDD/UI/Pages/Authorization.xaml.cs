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

namespace GIBDD.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        #region Поля страницы Authorization

        int countTry;

        #endregion

        #region Конструктор страницы Authorization

        public Authorization()
        {
            InitializeComponent();

            countTry = 3;
        }

        #endregion

        #region Проверка аккаунта

        private void EntryBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(LogTBox.Text))
                error.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(PasTBox.Password))
                error.AppendLine("Введите пароль");

            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }

            var account = Transition.Context.Account
                .FirstOrDefault(p => p.Login == LogTBox.Text && p.Password == PasTBox.Password);

            if (account != null)
                Transition.MainFrame.Navigate(new DriverView());
            else
                MessageBox.Show($"Логин или пароль введены неверно!\nКоличество оставшихся попыток: {--countTry}");

            if (countTry <= 0)
            {
                MessageBox.Show("Лимит попыток превышен!\nПовторить попытку возможно через 60 секунд");
                countTry = 3;
            }
        }

        #endregion

        #region Закрытие приложения

        private void ClosingBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
