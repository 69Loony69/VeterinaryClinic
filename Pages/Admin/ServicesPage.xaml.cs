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

namespace VeterinaryСlinic.Pages
{
    public partial class ServicesPage : Page
    {
        public static Veterinary_Clinic baza;
        public ServicesPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgServices.ItemsSource = baza.Services.ToList();
        }

        /// <summary>
        /// Кнопка для добавления усулги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Services service = new Services();
            service.ServiceId = 0;
            var editWindow = new Windows.WindowAddServices(service);
            if (editWindow.ShowDialog() == true)
            {
                dgServices.ItemsSource = MainWindow.baza.Services.ToList();
                dgServices.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для редактирования данных об усулги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgServices.SelectedItem != null)
            {
                var selected = dgServices.SelectedItem as Services; 
                var editWindow = new Windows.WindowEditServices(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgServices.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для удаления услуги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgServices.SelectedItem as Services;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить услугу?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Services.Find(delete.ServiceId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Services.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgServices.ItemsSource = null;
                        dgServices.ItemsSource = MainWindow.baza.Services.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите услугу из таблицы для удаления");
        }

        /// <summary>
        /// Поиск услуги по всем полям 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Services.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.ServiceId.ToString().StartsWith(searchText.ToLower())
                               || p.Name.ToLower().StartsWith(searchText.ToLower())
                               || p.Cost.ToString().StartsWith(searchText)).ToList();
            }
            dgServices.ItemsSource = x;
        }

        /// <summary>
        /// Обновление данных в dgServices по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgServices.ItemsSource = null;
            dgServices.ItemsSource = baza.Services.ToList();
        }
    }
}
