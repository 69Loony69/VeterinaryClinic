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
using System.IO;

namespace VeterinaryСlinic.Pages
{
    public partial class VeterinariansPage : Page
    {
        public static Veterinary_Clinic baza;
        public VeterinariansPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            VeterinariansList.ItemsSource = baza.Veterinarians.ToList();
        }

        /// <summary>
        /// Кнопка для редактирования данных ветеринара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var selectedVeterinarian = editButton.DataContext as Veterinarians;
            var editWindow = new Windows.WindowEditVeterinarians(selectedVeterinarian);
            if (editWindow.ShowDialog() == true)
            {
                VeterinariansList.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления данных о ветеринаре 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = VeterinariansList.SelectedItem as Veterinarians;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить данные о ветеринаре?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Veterinarians.Find(delete.VeterinarianId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Veterinarians.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Данные удалены успешно !");
                        VeterinariansList.ItemsSource = null;
                        VeterinariansList.ItemsSource = MainWindow.baza.Veterinarians.ToList();
                    }
                }
            }
            else MessageBox.Show("Активируйте запись для удаления!");
        }

        /// <summary>
        /// Поиск ветеринара по всем полям 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Veterinarians.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.VeterinarianId.ToString().ToLower().StartsWith(searchText.ToLower())
                       || p.Surname.ToLower().StartsWith(searchText.ToLower())
                       || p.Name.ToLower().StartsWith(searchText.ToLower())
                       || p.Patronymic.ToLower().StartsWith(searchText.ToLower())
                       || p.Specializations.Name.ToString().ToLower().StartsWith(searchText.ToLower())
                       || p.Phone.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            VeterinariansList.ItemsSource = x;
        }

        /// <summary>
        /// Кнопка для добавления ветеринара
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Veterinarians veterinarian = new Veterinarians();
            veterinarian.VeterinarianId = 0;
            var editWindow = new Windows.WindowEditVeterinarians(veterinarian);
            if (editWindow.ShowDialog() == true)
            {
                VeterinariansList.ItemsSource = MainWindow.baza.Veterinarians.ToList();
                VeterinariansList.Items.Refresh();
            }
        }

        /// <summary>
        /// Обновление данных в VeterinariansList по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            VeterinariansList.ItemsSource = null;
            VeterinariansList.ItemsSource = baza.Veterinarians.ToList();
        }
    }
}
