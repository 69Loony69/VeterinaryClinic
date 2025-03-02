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

namespace VeterinaryСlinic.Pages.Employee
{
    public partial class ReceptionPage : Page
    {
        private Veterinary_Clinic baza;

        List<string> patients = new List<string>();
        List<string> owners = new List<string>();
        List<string> veterinarians = new List<string>();
        public ReceptionPage()
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
        /// Кнопка для редактирования данных о приёме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgReception.SelectedItem != null)
            {
                var selected = dgReception.SelectedItem as Reception;
                var editWindow = new Windows.WindowAddEditReception(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgReception.ItemsSource= MainWindow.baza.Reception.ToList();
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите приём для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для добавления приёма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Reception reception = new Reception();
            reception.ReceptionId = 0;
            var editWindow = new Windows.WindowAddEditReception(reception);
            if (editWindow.ShowDialog() == true)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления данных о приёме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgReception.SelectedItem as Reception;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить данные о приёме?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Reception.Find(delete.ReceptionId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Reception.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Данные удалены успешно !");
                        dgReception.ItemsSource = null;
                        dgReception.ItemsSource = MainWindow.baza.Reception.ToList();
                    }
                }
            }
            else MessageBox.Show("Активируйте запись для удаления!");
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
            var detailPage = new Pages.Employee.ReceptionDetails(delete);
            NavigationService.Navigate(detailPage);
        }
        /// <summary>
        /// Добавление услуги к приему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Record(object sender, RoutedEventArgs e)
        {
            if (dgReception.SelectedItem != null)
            {
                Reception selectedReception = (Reception)dgReception.SelectedItem;
                ReceptionServices rs = new ReceptionServices { ReceptionId = selectedReception.ReceptionId };
                Windows.WindowAddEditServicesPatients window = new Windows.WindowAddEditServicesPatients(rs);
                window.ShowDialog();         
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите приём для добавления услуги");
            }
        }
        /// <summary>
        /// вывод чека в Word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cheque(object sender, RoutedEventArgs e)
        {
            if (dgReception.SelectedItem != null)
            {
                Reception selectedReception = (Reception)dgReception.SelectedItem;

                // Создаем новый документ Word
                var wordApp = new word.Application();
                wordApp.Visible = false;
                var wordDoc = wordApp.Documents.Add();

                // Добавляем заголовок "Чек"
                var titleParagraph = wordDoc.Paragraphs.Add();
                titleParagraph.Range.Text = "                                                             Чек\n";

                // Устанавливаем общий шрифт и размер шрифта для документа
                wordDoc.Content.Font.Name = "Times New Roman";
                wordDoc.Content.Font.Size = 14;
                wordDoc.Content.ParagraphFormat.Alignment= word.WdParagraphAlignment.wdAlignParagraphJustify;

                // Добавляем информацию о приеме
                wordDoc.Paragraphs.Add().Range.Text = $"Код приема: {selectedReception.ReceptionId}\n";
                wordDoc.Paragraphs.Add().Range.Text = $"ФИО владельца: {selectedReception.Patients.Owners.FullName}\n";
                wordDoc.Paragraphs.Add().Range.Text = $"Кличка пациента: {selectedReception.Patients.Name}\n";
                wordDoc.Paragraphs.Add().Range.Text = $"ФИО врача: {selectedReception.Veterinarians.FullName}\n";

                // Создаем таблицу для услуг
                var servicesTable = wordDoc.Tables.Add(wordDoc.Paragraphs.Add().Range, selectedReception.ReceptionServices.Count + 1, 2);
                servicesTable.Borders.Enable = 1; // Включаем границы
                servicesTable.Cell(1, 1).Range.Text = "Услуга";
                servicesTable.Cell(1, 2).Range.Text = "Стоимость";
                servicesTable.Rows[1].Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter; // Выравниваем заголовки по центру

                // Заполняем таблицу услуг
                int totalCost = 0;
                for (int i = 0; i < selectedReception.ReceptionServices.Count; i++)
                {
                    var service = selectedReception.ReceptionServices.ElementAt(i).Services;
                    servicesTable.Cell(i + 2, 1).Range.Text = service.Name;
                    servicesTable.Cell(i + 2, 2).Range.Text = service.Cost.ToString();
                    totalCost += service.Cost;
                }

                // Выводим сумму к оплате
                wordDoc.Paragraphs.Add().Range.Text = $"Сумма к оплате: {totalCost}";

                // Отображаем документ
                wordApp.Visible = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите приём для вывовда чека");
            }
        }
    }
}
