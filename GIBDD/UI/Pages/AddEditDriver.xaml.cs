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
    /// Логика взаимодействия для AddEditDriver.xaml
    /// </summary>
    public partial class AddEditDriver : Page
    {
        private Drivers addDriver;
        private string pathProject = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\UI\photo\"));


        public AddEditDriver(Drivers transferDr)
        {
            InitializeComponent();

            addDriver = transferDr ?? new Drivers();
            this.Title = transferDr == null ? "Добавление водителя" : "Редактирование водителя";

            if (transferDr != null)
            {
                ImageAgent.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(pathProject + transferDr.Photo);
            }

            DataContext = addDriver;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(addDriver.Surname))
                error.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(addDriver.Name))
                error.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(addDriver.Middlename))
                error.AppendLine("Укажите отчество");
            if (string.IsNullOrWhiteSpace(addDriver.Passport))
                error.AppendLine("Укажите паспорт");
            if (string.IsNullOrWhiteSpace(addDriver.Town))
                error.AppendLine("Укажите город");
            if (string.IsNullOrWhiteSpace(addDriver.Address))
                error.AppendLine("Укажите адрес");
            if (string.IsNullOrWhiteSpace(addDriver.Company))
                error.AppendLine("Укажите место работы");
            if (string.IsNullOrWhiteSpace(addDriver.Jobname))
                error.AppendLine("Укажите должность");
            if (string.IsNullOrWhiteSpace(addDriver.Phone))
                error.AppendLine("Укажите телефон");
            if (!addDriver.Email.Contains("@"))
                error.AppendLine("Укажите корректно email");
            if (string.IsNullOrWhiteSpace(addDriver.Photo))
                error.AppendLine("Загрузите фото");

            if (error.Length > 0)
            {
                MessageBox.Show($"Данные не соотвествуют следующим критериям:\n{error}",
                    "Сохранение водителя", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (addDriver.Id == 0)
                Transition.Context.Drivers.Add(addDriver);

            try
            {
                Transition.Context.SaveChanges();
                MessageBox.Show($"Данные успешно сохранены",
                    "Сохранение водителя", MessageBoxButton.OK, MessageBoxImage.Information);
                Transition.MainFrame.GoBack();
            }
            catch (ApplicationException er)
            {
                MessageBox.Show($"При сохранении водителя возникла ошибка:\n{er.Message}",
                    "Сохранение водителя", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DownloadImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png;*.jpg)|*.png;*jpg";
            ofd.InitialDirectory = pathProject;

            if (ofd.ShowDialog() == true)
            {
                if (!File.Exists(pathProject + ofd.SafeFileName))
                    File.Copy(ofd.FileName, pathProject + ofd.SafeFileName);

                addDriver.Photo = ofd.SafeFileName;
                ImageAgent.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(pathProject + ofd.SafeFileName);
            }
        }
    }
}
