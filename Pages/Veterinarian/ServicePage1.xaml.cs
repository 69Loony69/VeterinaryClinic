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

namespace VeterinaryСlinic.Pages.Veterinarian
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage1 : Page
    {
        private Veterinary_Clinic baza;
        List<string> patients = new List<string>();
        List<string> owners = new List<string>();
        List<string> veterinarians = new List<string>();
        List<string> services = new List<string>();
        public ServicePage1()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgService.ItemsSource = MainWindow.baza.ReceptionServices.ToList();
            Refresh();

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

            services.Add("Все услуги");
            services.AddRange(MainWindow.baza.Services.Select(s => s.Name).Distinct());
            ServiceComboBox.ItemsSource = services;
            ServiceComboBox.SelectedItem = "Все услуги";
        }

        private void FilterServices()
        {
            var selectedPatient = PatientComboBox.SelectedItem as string;
            var selectedOwner = OwnersComboBox.SelectedItem as string;
            var selectedVeterinarian = VeterinarianComboBox.SelectedItem as string;
            var selectedService = ServiceComboBox.SelectedItem as string;

            var services = MainWindow.baza.ReceptionServices.ToList();

            if (selectedPatient != "Все пациенты")
            {
                services = services.Where(s => s.Reception.Patients.Name == selectedPatient).ToList();
            }

            if (selectedOwner != "Все владельцы")
            {
                services = services.Where(s => s.Reception.Patients.Owners.Surname == selectedOwner).ToList();
            }

            if (selectedVeterinarian != "Все ветеринары")
            {
                services = services.Where(s => s.Reception.Veterinarians.Surname == selectedVeterinarian).ToList();
            }

            if (selectedService != "Все услуги")
            {
                services = services.Where(s => s.Services.Name == selectedService).ToList();
            }

            dgService.ItemsSource = services;
        }

        private void PatientComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void OwnersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void VeterinarianComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void Reset_filters(object sender, RoutedEventArgs e)
        {
            PatientComboBox.SelectedItem = "Все пациенты";
            OwnersComboBox.SelectedItem = "Все владельцы";
            VeterinarianComboBox.SelectedItem = "Все ветеринары";
            ServiceComboBox.SelectedItem = "Все услуги";

            FilterServices();
        }

    /// <summary>
    /// Обновление данных в dgService
    /// </summary>
    private void Refresh()
        {
            baza = new Veterinary_Clinic();
            dgService.ItemsSource = null;
            dgService.ItemsSource = baza.ReceptionServices.ToList();
        }
        /// <summary>
        /// Поиск записи пациента на услугу по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchQuery = Search.Text.ToLower();
            var allServices = MainWindow.baza.ReceptionServices.ToList(); // Получаем все услуги в память
            var filteredList = allServices.Where(rs =>
                rs.ReceptionId.ToString().ToLower().Contains(searchQuery) ||
                rs.Reception.FormattedDate.ToLower().Contains(searchQuery) ||
                rs.Reception.Time.ToString().ToLower().Contains(searchQuery) ||
                rs.Reception.Patients.Owners.FullName.ToLower().Contains(searchQuery) ||
                rs.Reception.Patients.Name.ToLower().Contains(searchQuery) ||
                rs.Reception.Veterinarians.FullName.ToLower().Contains(searchQuery) ||
                rs.Services.Name.ToLower().Contains(searchQuery)
            ).ToList();
            dgService.ItemsSource = filteredList;

        }
    }
}
