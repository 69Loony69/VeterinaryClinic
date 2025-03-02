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
    public partial class TreatmentPage : Page
    {
        private Veterinary_Clinic baza;
        List<string> patients = new List<string>();
        List<string> owners = new List<string>();
        List<string> veterinarians = new List<string>();
        List<string> medication = new List<string>();
        public TreatmentPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgTreatment.ItemsSource = MainWindow.baza.Treatment.ToList();
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

            medication.Add("Все препараты");
            medication.AddRange(MainWindow.baza.Medication.Select(s => s.Name).Distinct());
            MedicationComboBox.ItemsSource = medication;
            MedicationComboBox.SelectedItem = "Все препараты";
        }

        private void FilterServices()
        {
            var selectedPatient = PatientComboBox.SelectedItem as string;
            var selectedOwner = OwnersComboBox.SelectedItem as string;
            var selectedVeterinarian = VeterinarianComboBox.SelectedItem as string;
            var selectedMedication = MedicationComboBox.SelectedItem as string;

            var services = MainWindow.baza.Treatment.ToList();

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

            if (selectedMedication != "Все препараты")
            {
                services = services.Where(s => s.Medication.Name == selectedMedication).ToList();
            }

            dgTreatment.ItemsSource = services;
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
        private void MedicationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterServices();
        }

        private void Reset_filters(object sender, RoutedEventArgs e)
        {
            PatientComboBox.SelectedItem = "Все пациенты";
            OwnersComboBox.SelectedItem = "Все владельцы";
            VeterinarianComboBox.SelectedItem = "Все ветеринары";
            MedicationComboBox.SelectedItem = "Все препараты";

            FilterServices();
        }

    /// <summary>
    /// Обновление данных в dgService
    /// </summary>
    private void Refresh()
        {
            baza = new Veterinary_Clinic();
            dgTreatment.ItemsSource = null;
            dgTreatment.ItemsSource = baza.Treatment.ToList();
        }
        /// <summary>
        /// Поиск записи пациента на услугу по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchQuery = Search.Text.ToLower();
            var allServices = MainWindow.baza.Treatment.ToList(); // Получаем все услуги в память
            var filteredList = allServices.Where(rs =>
                rs.ReceptionId.ToString().ToLower().Contains(searchQuery) ||
                rs.Reception.FormattedDate.ToLower().Contains(searchQuery) ||
                rs.Reception.Time.ToString().ToLower().Contains(searchQuery) ||
                rs.Reception.Patients.Owners.FullName.ToLower().Contains(searchQuery) ||
                rs.Reception.Patients.Name.ToLower().Contains(searchQuery) ||
                rs.Reception.Veterinarians.FullName.ToLower().Contains(searchQuery) ||
                rs.Medication.Name.ToLower().Contains(searchQuery)
            ).ToList();
            dgTreatment.ItemsSource = filteredList;

        }
        /// <summary>
        /// Кнопка для редактирования данных о лечение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgTreatment.SelectedItem != null)
            {
                var selected = dgTreatment.SelectedItem as Treatment;
                var editWindow = new Windows.Veterinarian.WindowAddEditTreatmentPatients(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgTreatment.ItemsSource = MainWindow.baza.Treatment.ToList();
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу для редактирования");
            }
        }
        /// <summary>
        /// Кнопка для удаления данных о лечении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgTreatment.SelectedItem as Treatment;
              if (delete != null)
              {
                  MessageBoxResult result = MessageBox.Show
                  ("Вы точно хотите удалить данные о записи пациента на определенную услугу?", "Внимание!",
                  MessageBoxButton.YesNo, MessageBoxImage.Error);
                  if (result == MessageBoxResult.Yes)
                  {
                      var tracked = MainWindow.baza.Treatment.Find(delete.TreatmentId);
                      if (tracked != null)
                      {
                          MainWindow.baza.Treatment.Remove(tracked);
                          MainWindow.baza.SaveChanges();
                          MessageBox.Show("Данные удалены успешно !");
                        dgTreatment.ItemsSource = null;
                        dgTreatment.ItemsSource = MainWindow.baza.Treatment.ToList();
                      }
                  }
              }
              else MessageBox.Show("Активируйте запись для удаления!");
        }
    }
}
