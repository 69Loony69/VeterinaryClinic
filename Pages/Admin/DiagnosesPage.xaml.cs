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
    public partial class DiagnosesPage : Page
    {
        public static Veterinary_Clinic baza;
        public DiagnosesPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgDiagnosis.ItemsSource = baza.Diagnosis.ToList();
        }

        /// <summary>
        /// Кнопка для редактирования данных диагноза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgDiagnosis.SelectedItem != null)
            {
                var selected= dgDiagnosis.SelectedItem as Diagnosis;
                var editWindow = new Windows.WindowEditDiagnosis(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgDiagnosis.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите диагноз из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для добавления диагноза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Diagnosis diagnosis = new Diagnosis();
            diagnosis.DiagnosisId = 0;
            var editWindow = new Windows.WindowEditDiagnosis(diagnosis);
            if (editWindow.ShowDialog() == true)
            {
                dgDiagnosis.ItemsSource = MainWindow.baza.Diagnosis.ToList();
                dgDiagnosis.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления диагноза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete= dgDiagnosis.SelectedItem as Diagnosis;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить диагноз?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked= MainWindow.baza.Diagnosis.Find(delete.DiagnosisId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Diagnosis.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgDiagnosis.ItemsSource = null;
                        dgDiagnosis.ItemsSource = MainWindow.baza.Diagnosis.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите диагноз из таблицы для удаления");
        }

        /// <summary>
        /// Поиск диагноза по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Diagnosis.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.DiagnosisId.ToString().StartsWith(searchText.ToLower())
                               || p.Name.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            dgDiagnosis.ItemsSource = x;
        }

        /// <summary>
        /// Обновление данных в dgDiagnosis по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgDiagnosis.ItemsSource = null;
            dgDiagnosis.ItemsSource = baza.Diagnosis.ToList();
        }
    }
}
