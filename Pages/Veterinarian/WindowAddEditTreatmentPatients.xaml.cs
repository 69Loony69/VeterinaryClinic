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
    public partial class WindowAddEditTreatmentPatients : Window
    {
        private Treatment t;
        public WindowAddEditTreatmentPatients(Treatment t)
        {
            InitializeComponent();
            this.t = t;
            DataContext = t;
            cbMedication.ItemsSource = MainWindow.baza.Medication.ToList();
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
        /// <summary>
        /// Кнопка для закрытия окна с сохранением данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (t.ReceptionId == 0 || cbMedication.SelectedValue == null)
                {
                    throw new Exception("Перед сохранением необходимо заполнить все поля.");
                }
                else
                {
                    if (t.TreatmentId == 0) // Новый пользователь
                    {
                        var existingTreatment = MainWindow.baza.Treatment.FirstOrDefault(c => c.ReceptionId == t.ReceptionId && c.MedicationId == t.MedicationId);
                        if (existingTreatment != null)
                        {
                            throw new Exception("Такой препарат уже назначен для данного пациента приёма.");
                        }
                        MainWindow.baza.Treatment.Add(t);
                    }
                    else
                    {
                        var existingTreatment = MainWindow.baza.Treatment.FirstOrDefault(c => c.TreatmentId == t.TreatmentId);
                        if (existingTreatment != null)
                        {
                            existingTreatment.ReceptionId = t.ReceptionId;
                            existingTreatment.MedicationId = t.MedicationId;
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
    }
}
