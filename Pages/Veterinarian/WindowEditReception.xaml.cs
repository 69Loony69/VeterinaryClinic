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
using System.Windows.Shapes;


namespace VeterinaryСlinic.Windows.Veterinarian
{
    public partial class WindowEditReception : Window
    {
        private Reception reception;
        public WindowEditReception(Reception reception)
        {
            InitializeComponent();
            this.reception = reception;
            DataContext = reception;

            // Загрузка данных в ComboBox
            cbSpecializations.ItemsSource = MainWindow.baza.Specializations.ToList();
            cbVeterinarian.ItemsSource = MainWindow.baza.Veterinarians.ToList();
            cbPatient.ItemsSource = MainWindow.baza.Patients.ToList();
            cbOwner.ItemsSource = MainWindow.baza.Owners.ToList();
            cbDiagnosis.ItemsSource = MainWindow.baza.Diagnosis.ToList();

            // Обработка событий изменения выбранного элемента
            cbSpecializations.SelectionChanged += CbSpecializations_SelectionChanged;
            cbOwner.SelectionChanged += CbOwner_SelectionChanged;
        }

        /// <summary>
        /// Список для работы с ветеринарами и специализациями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSpecializations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpecialization = cbSpecializations.SelectedItem as Specializations;
            if (selectedSpecialization != null)
            {
                cbVeterinarian.ItemsSource = MainWindow.baza.Veterinarians.Where(v => v.SpecializationsId == selectedSpecialization.SpecializationsId).ToList();
            }
        }
        /// <summary>
        /// Список для работы с владельцами и пациентами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOwner = cbOwner.SelectedItem as Owners;
            if (selectedOwner != null)
            {
                cbPatient.ItemsSource = MainWindow.baza.Patients.Where(p => p.OwnerId == selectedOwner.OwnerId).ToList();
            }
        }

        /// <summary>
        /// Кнопка для закрытия окна с сохранением данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (date.SelectedDate == null || timePicker.SelectedTime == null || cbSpecializations.SelectedValue == null || cbVeterinarian.SelectedValue == null || cbOwner.SelectedValue == null || cbPatient.SelectedValue == null)
                {
                    throw new Exception("Перед сохранением необходимо заполнить все поля.");
                }
                else
                {
                    if (reception.ReceptionId == 0) // Новый прием
                    {
                        // Проверка на существование приема с такой же датой, временем и врачом
                        var existingReception = MainWindow.baza.Reception.FirstOrDefault(r => r.Date.Date == reception.Date.Date && r.Time == reception.Time && r.VeterinarianId == reception.VeterinarianId);
                        if (existingReception != null)
                        {
                            throw new Exception("Врач недоступен для данной даты или времени.");
                        }
                        MainWindow.baza.Reception.Add(reception);
                    }
                    else // Существующий прием
                    {
                        var existing = MainWindow.baza.Reception.FirstOrDefault(c => c.ReceptionId == reception.ReceptionId);
                        if (existing != null)
                        {
                            // Проверка, что данная дата и время уже занято
                            var existingReception = MainWindow.baza.Reception.FirstOrDefault(r => r.Date.Date == reception.Date.Date && r.Time == reception.Time && r.VeterinarianId == reception.VeterinarianId);
                            if (existingReception != null && existingReception.ReceptionId != reception.ReceptionId)
                            {
                                throw new Exception("Данная дата и время уже занято.");
                            }

                            existing.Date = reception.Date;
                            existing.Time = reception.Time;
                            existing.VeterinarianId = reception.VeterinarianId;
                            existing.PatientId = reception.PatientId;
                            existing.Complaints = reception.Complaints;
                            existing.DiagnosisId = reception.DiagnosisId;
                        }
                    }
                    MainWindow.baza.SaveChanges();
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



            /// <summary>
            /// Кнопка для закрытия окна без сохранениея данных
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Close(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Запись не сохранена! Для корректной работы, рекомендуется произвести обновление данных.");
            this.Close();
        }

    }
}
