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
    public partial class MedicationPage : Page
    {
        public static Veterinary_Clinic baza;
        public MedicationPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgMedication.ItemsSource = baza.Medication.ToList();
        }

        /// <summary>
        /// Кнопка для добавления препарата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Medication medication = new Medication();
            medication.MedicationId = 0;
            var editWindow = new Windows.WindowAddMedication(medication);
            if (editWindow.ShowDialog() == true)
            {
                dgMedication.ItemsSource = MainWindow.baza.Medication.ToList();
                dgMedication.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для редактирования данных о препарате
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgMedication.SelectedItem != null)
            {
                var selected = dgMedication.SelectedItem as Medication;
                var editWindow = new Windows.WindowEditMedication(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgMedication.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите препарат из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для удаления препарата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete= dgMedication.SelectedItem as Medication;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить данные о препарате?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Medication.Find(delete.MedicationId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Medication.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgMedication.ItemsSource = null;
                        dgMedication.ItemsSource = MainWindow.baza.Medication.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите препарат из таблицы для удаления");
        }

        /// <summary>
        /// Поиск услуги по всем полям 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Medication.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.MedicationId.ToString().StartsWith(searchText.ToLower())
                               || p.Name.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            dgMedication.ItemsSource = x;
        }

        /// <summary>
        /// Обновление данных в dgMedication по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgMedication.ItemsSource = null;
            dgMedication.ItemsSource = baza.Medication.ToList();
        }
    }
}
