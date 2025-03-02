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
using word = Microsoft.Office.Interop.Word;
using excel = Microsoft.Office.Interop.Excel;

namespace VeterinaryСlinic.Pages.Veterinarian
{
    public partial class ReceptionPage1 : Page
    {
        private Veterinary_Clinic baza;

        List<string> patients = new List<string>();
        List<string> owners = new List<string>();
        List<string> veterinarians = new List<string>();
        public ReceptionPage1()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgReception.ItemsSource = baza.Reception.ToList();

            patients.Add("Все пациенты");
            patients.AddRange(MainWindow.baza.Patients.Select(p => p.Name).Distinct());
            PatientComboBox.ItemsSource = patients;
            PatientComboBox.SelectedItem = "Все пациенты";

            owners.Add("Все владельцы");
            owners.AddRange(MainWindow.baza.Owners.Select(o => o.Surname).Distinct());
            OwnersComboBox.ItemsSource = owners;
            OwnersComboBox.SelectedItem = "Все владельцы";

            veterinarians.Add("Все ветеринары");
            veterinarians.AddRange(MainWindow.baza.Veterinarians.Select(v => v.Surname).Distinct());
            VeterinarianComboBox.ItemsSource = veterinarians;
            VeterinarianComboBox.SelectedItem = "Все ветеринары";
        }

        /// <summary>
        /// Метод для фильтров
        /// </summary>
        private void FilterReceptions()
        {
            var selectedPatient = PatientComboBox.SelectedItem as string;
            var selectedOwner = OwnersComboBox.SelectedItem as string;
            var selectedVeterinarian = VeterinarianComboBox.SelectedItem as string;

            var receptions = MainWindow.baza.Reception.ToList();

            if (selectedPatient != "Все пациенты")
            {
                receptions = receptions.Where(r => r.Patients.Name == selectedPatient).ToList();
            }

            if (selectedOwner != "Все владельцы")
            {
                receptions = receptions.Where(r => r.Patients.Owners.Surname == selectedOwner).ToList();
            }

            if (selectedVeterinarian != "Все ветеринары")
            {
                receptions = receptions.Where(r => r.Veterinarians.Surname == selectedVeterinarian).ToList();
            }

            dgReception.ItemsSource = receptions;
        }

        /// <summary>
        /// Фильтр для выбора пациентов по имени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterReceptions();
        }

        /// <summary>
        /// Фильтр для выбора владельцев по фамилии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OwnersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterReceptions();
        }

        /// <summary>
        /// Фильтр для выбора ветеринаров по фамилии
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VeterinarianComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterReceptions();
        }

        /// <summary>
        /// Кнопка для сброса фильтров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_filters(object sender, RoutedEventArgs e)
        {
            // Установите выбранный элемент каждого комбо-бокса обратно на "Все типы"
            PatientComboBox.SelectedItem = "Все пациенты";
            OwnersComboBox.SelectedItem = "Все владельцы";
            VeterinarianComboBox.SelectedItem = "Все ветеринары";

            FilterReceptions();
        }

        /// <summary>
        /// Обновление данных в dgReception
        /// </summary>
        private void Refresh()
        {
            baza = new Veterinary_Clinic();
            dgReception.ItemsSource = null;
            dgReception.ItemsSource = baza.Reception.ToList();
        }

        /// <summary>
        /// Обновление данных по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Refresh();
        }
        /// <summary>
        /// Поиск приёма по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Reception.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.ReceptionId.ToString().ToLower().Contains(searchText)
                           || p.FormattedDate.Contains(searchText)
                           || p.Time.ToString().ToLower().Contains(searchText)
                           || p.Patients.Owners.FullName.ToLower().StartsWith(searchText.ToLower())
                           || p.Veterinarians.FullName.ToLower().StartsWith(searchText.ToLower())
                           || p.Patients.Name.ToLower().StartsWith(searchText.ToLower())
                           || (p.Complaints?.ToLower().Contains(searchText) ?? false)
                           || (p.Diagnosis?.Name?.ToLower().Contains(searchText) ?? false)).ToList();
            }
            dgReception.ItemsSource = x;
        }
        /// <summary>
        /// Открытие страинце с деталями приёма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var delete = dgReception.SelectedItem as Reception;
            var detailPage = new Pages.Veterinarian.ReceptionDetails1(delete);
            NavigationService.Navigate(detailPage);
        }



    }
}
