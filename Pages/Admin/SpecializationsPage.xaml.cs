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

namespace VeterinaryСlinic.Pages.Admin
{
    public partial class SpecializationsPage : Page
    {
        public static Veterinary_Clinic baza;
        public SpecializationsPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgSpecializations.ItemsSource = baza.Specializations.ToList();
        }

        /// <summary>
        /// Кнопка для редактирования данных специализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgSpecializations.SelectedItem != null)
            {
                var selected= dgSpecializations.SelectedItem as Specializations;
                var editWindow = new Windows.WindowEditSpecializations(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgSpecializations.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите специализацию из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для добавления специализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Specializations specialization = new Specializations();
            specialization.SpecializationsId = 0;
            var editWindow = new Windows.WindowEditSpecializations(specialization);
            if (editWindow.ShowDialog() == true)
            {
                dgSpecializations.ItemsSource = MainWindow.baza.Specializations.ToList();
                dgSpecializations.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления специализации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgSpecializations.SelectedItem as Specializations;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить специализацию?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Specializations.Find(delete.SpecializationsId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Specializations.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgSpecializations.ItemsSource = null;
                        dgSpecializations.ItemsSource = MainWindow.baza.Specializations.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите специализацию из таблицы для удаления");
        }

        /// <summary>
        /// Поиск специализации по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Specializations.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.SpecializationsId.ToString().StartsWith(searchText.ToLower())
                               || p.Name.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            dgSpecializations.ItemsSource = x;
        }

        /// <summary>
        /// Обновление данных в dgSpecializations по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgSpecializations.ItemsSource = null;
            dgSpecializations.ItemsSource = baza.Specializations.ToList();
        }
    }
}
