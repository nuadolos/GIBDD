using GIBDD.Entities;
using GIBDD.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для DriverView.xaml
    /// </summary>
    public partial class DriverView : Page
    {
        #region Коструктор страницы DriverView

        public DriverView()
        {
            InitializeComponent();

            ViewDriver.ItemsSource = Transition.Context.Drivers.ToList().Where(p => p.IsDelete == null);
        }

        #endregion
        
        #region Переход на страницу AddEditDriver

        private void AddDriverBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.MainFrame.Navigate(new AddEditDriver(null));
        }

        private void ViewDriver_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Transition.MainFrame.Navigate(new AddEditDriver(ViewDriver.SelectedItem as Drivers));
        }

        #endregion

        #region Удаление водителя и перенос данных в архив

        private void DeleteDriversBtn_Click(object sender, RoutedEventArgs e)
        {
            var driversForRemoving = ViewDriver.SelectedItems.Cast<Drivers>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить {driversForRemoving.Count} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (Drivers archive in driversForRemoving)
                    {
                        archive.IsDelete = true;
                    }

                    Transition.Context.SaveChanges();
                    MessageBox.Show("Данные были удалены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                    ViewDriver.ItemsSource = Transition.Context.Drivers.ToList().Where(p => p.IsDelete == null);
                }
                catch (Exception er)
                {
                    MessageBox.Show($"При сохранении возникла ошибка: {er.Message}", "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void ViewDriver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewDriver.SelectedItems.Count > 0)
                DeleteDriversBtn.Visibility = Visibility.Visible;
            else
                DeleteDriversBtn.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Переход в архив водителей и сохранение данных в .csv файл

        private void ArchiveBtn_Click(object sender, RoutedEventArgs e)
        {
            NoArchivePanel.Visibility = Visibility.Collapsed;
            ArchivePanel.Visibility = Visibility.Visible;
            ViewDriver.ItemsSource = Transition.Context.Drivers.ToList().Where(p => p.IsDelete == true);
        }

        private void CSVBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files(*.csv)|*.csv";

            string pathFile = null;

            if (sfd.ShowDialog() == true)
            {
                pathFile = sfd.FileName;
            }

            if (pathFile == null)
                return;

            using (StreamWriter writer = new StreamWriter(new FileStream(pathFile, FileMode.Create, FileAccess.Write), Encoding.UTF8))
            {
                writer.WriteLine("Номер водителя;Фамилия;Имя;Отчество;Серия паспорта;Номер паспорта;Город;Адрес;Компания;Должность;Телефон;Почта;Фото;Комментарий");
                foreach (Drivers dataFile in Transition.Context.Drivers.ToList().Where(p => p.IsDelete == true))
                {
                    writer.WriteLine($"{dataFile.Id};{dataFile.Surname};{dataFile.Name};{dataFile.Middlename};{dataFile.Passport.Substring(0, 4)};{dataFile.Passport.Substring(5, 6)};" +
                        $"{dataFile.Town};{dataFile.Address};{dataFile.Company};{dataFile.Jobname};{dataFile.Phone};{dataFile.Email};{dataFile.Photo};{dataFile.Comment}");
                }
            }

            MessageBox.Show($"Файл данных был выгружен: {pathFile}", "Файл .csv", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Переход обратно из архива

        private void DriversListBtn_Click(object sender, RoutedEventArgs e)
        {
            NoArchivePanel.Visibility = Visibility.Visible;
            ArchivePanel.Visibility = Visibility.Collapsed;
            ViewDriver.ItemsSource = Transition.Context.Drivers.ToList().Where(p => p.IsDelete == null);
        }

        #endregion

        #region Обновление ListView после добавления или редактирования водителя

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Transition.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ViewDriver.ItemsSource = Transition.Context.Drivers.ToList().Where(p => p.IsDelete == null);
            }
        }

        #endregion
    }
}
