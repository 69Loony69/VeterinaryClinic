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
    public partial class OwnersPage : Page
    {
        private Veterinary_Clinic baza;
        public OwnersPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            OwnersList.ItemsSource = baza.Owners.ToList();
            Refresh();
            RefreshPage();
        }
        /// <summary>
        /// Обновление данных в OwnersList
        /// </summary>
        private void Refresh()
        {
            OwnersList.ItemsSource = null;
            OwnersList.ItemsSource = baza.Owners.ToList();
        }

        /// <summary>
        /// Поиск владельца по всем полям 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Owners.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.OwnerId.ToString().ToLower().StartsWith(searchText.ToLower())
                       || p.Surname.ToLower().StartsWith(searchText.ToLower())
                       || p.Name.ToLower().StartsWith(searchText.ToLower())
                       || p.Patronymic.ToLower().StartsWith(searchText.ToLower())
                       || p.Phone.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            OwnersList.ItemsSource = x;
        }

        /// <summary>
        /// Сортировка по фамилии, имени, отчеству владельца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string sortField = "";
        private string sortOrder = "";
        private void SortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (sender as ComboBox).SelectedItem as ComboBoxItem;

            if (selectedOption.Content.ToString() == "Фамилии")
            {
                sortField = "Surname";
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[5] as ComboBoxItem).Visibility = Visibility.Visible;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Collapsed;
            }
            else if (selectedOption.Content.ToString() == "Имени")
            {
                sortField = "Name";
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[5] as ComboBoxItem).Visibility = Visibility.Visible;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Collapsed;
            }
            else if (selectedOption.Content.ToString() == "Отчеству")
            {
                sortField = "Patronymic";
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[5] as ComboBoxItem).Visibility = Visibility.Visible;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Collapsed;
            }
            else if (selectedOption.Content.ToString() == "По возрастанию" || selectedOption.Content.ToString() == "По убыванию")
            {
                sortOrder = selectedOption.Content.ToString();
                SortData();
            }
            else if (selectedOption.Content.ToString() == "Cбросить сортировку")
            {
                sortField = "";
                sortOrder = "";
                OwnersList.ItemsSource = baza.Owners.ToList();
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[5] as ComboBoxItem).Visibility = Visibility.Collapsed;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Visible;
            }
        }
        private void SortData()
        {
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "По возрастанию")
                {
                    OwnersList.ItemsSource = OwnersList.ItemsSource.Cast<Owners>().OrderBy(o => GetPropertyValue(o, sortField));
                }
                else if (sortOrder == "По убыванию")
                {
                    OwnersList.ItemsSource = OwnersList.ItemsSource.Cast<Owners>().OrderByDescending(o => GetPropertyValue(o, sortField));
                }
            }
        }
        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }

        /// <summary>
        /// Кнопка для добавления владельца
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Owners owner = new Owners();
            owner.OwnerId = 0;
            var editWindow = new Windows.WindowAddEditOwners(owner);
            if (editWindow.ShowDialog() == true)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Кнопка для редактирования данных о владельце
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (OwnersList.SelectedItem != null)
            {
                var selected = OwnersList.SelectedItem as Owners;
                var editWindow = new Windows.WindowAddEditOwners(selected);
                if (editWindow.ShowDialog() == true)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите владельца для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для удаления данных о владельце
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = OwnersList.SelectedItem as Owners;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить данные о владельце?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Owners.Find(delete.OwnerId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Owners.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Данные удалены успешно !");
                        OwnersList.ItemsSource = null;
                        OwnersList.ItemsSource = MainWindow.baza.Owners.ToList();
                    }
                }
            }
            else MessageBox.Show("Активируйте запись для удаления!");
        } 

        private int _currentPage = 1;
        private int _count = 10;
        private int _maxPages;

        /// <summary>
        /// Постраничное отображение записей (на странице отбражается 10 записей)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void RefreshPage()
        {
            var list = MainWindow.baza.Owners.ToList();
            _maxPages = (int)Math.Ceiling(list.Count * 1.0 / _count);

            var listPage = list.Skip((_currentPage - 1) * _count).Take(_count).ToList();

            TxtCurrentPage.Text = _currentPage.ToString();
            LblTotalPages.Content = "из " + _maxPages;
            LblInfo.Content = $"Всего {list.Count} записей";

            OwnersList.ItemsSource = listPage;
        }

        /// <summary>
        /// Кнопка для перехода к 1 странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToFirstPage(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            RefreshPage();

        }

        /// <summary>
        /// Кнопка для перелистывания к 1 странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage <= 1) _currentPage = 1;
            else
                _currentPage--;
            RefreshPage();
        }

        /// <summary>
        ///  Кнопка для перелистывания к последней странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToNextPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage >= _maxPages) _currentPage = _maxPages;
            else
                _currentPage++;
            RefreshPage();
        }

        /// <summary>
        /// Кнопка для перехода к последней странице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToLastPage(object sender, RoutedEventArgs e)
        {
            _currentPage = _maxPages;
            RefreshPage();
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
        /// Вывод данных о владельцах в документ Word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToWord(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(@"По умолчанию выводятся данные о всех владельцев. Для вывода данных о конкретном владельце выберите его из списка.");
            var selectedOwners = OwnersList.SelectedItems.Cast<Owners>().ToList();// Получаем данные
            var owners = selectedOwners.Any() ? selectedOwners : baza.Owners.ToList();

            word.Application wordApp = new word.Application();
            word.Document document = wordApp.Documents.Add();
            document.Content.Font.Name = "Times New Roman";// Устанавливаем шрифт документа

            var titleRange = document.Range(0, 0); // Создаем заголовок
            titleRange.Text = "Данные о владельце";
            titleRange.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
            titleRange.Font.Size = 16;
            titleRange.InsertParagraphAfter();

            var tableRange = document.Range(titleRange.End, titleRange.End); // Создаем таблицу
            var table = document.Tables.Add(tableRange, owners.Count + 1, 5);
            table.Borders.Enable = 1; // Включаем границы

            table.Cell(1, 1).Range.Text = "Код владельца"; // Заполняем заголовки таблицы
            table.Cell(1, 2).Range.Text = "Фамилия";
            table.Cell(1, 3).Range.Text = "Имя";
            table.Cell(1, 4).Range.Text = "Отчество";
            table.Cell(1, 5).Range.Text = "Телефон";

            int row = 2;// Заполняем таблицу данными
            foreach (var owner in owners)
            {
                for (int column = 1; column <= 5; column++)
                {
                    var cell = table.Cell(row, column);
                    cell.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphJustify;
                    switch (column)
                    {
                        case 1:
                            cell.Range.Text = owner.OwnerId.ToString();
                            break;
                        case 2:
                            cell.Range.Text = owner.Surname;
                            break;
                        case 3:
                            cell.Range.Text = owner.Name;
                            break;
                        case 4:
                            cell.Range.Text = owner.Patronymic;
                            break;
                        case 5:
                            cell.Range.Text = owner.Phone;
                            break;
                    }
                }
                row++;
            }
            wordApp.Visible = true;
        }

        /// <summary>
        /// Вывод данных о владельцах в документ Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToExcel(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(@"По умолчанию выводятся данные о всех владельцах. Для вывода данных о конкретном владельце выберите его из списка.");

            var selectedOwners = OwnersList.SelectedItems.Cast<Owners>().ToList();// Получаем данные
            var owners = selectedOwners.Any() ? selectedOwners : baza.Owners.ToList();

            excel.Application excelApp = new excel.Application();
            excel.Workbook workbook = excelApp.Workbooks.Add();
            excel.Worksheet worksheet = workbook.Worksheets[1];

            worksheet.Cells[1, 1] = "Данные о владельце"; // Создаем заголовок
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 5]].Merge();
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 5]].HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 5]].Font.Size = 16;

            worksheet.Cells[2, 1] = "Код владельца"; // Заполняем заголовки таблицы
            worksheet.Cells[2, 2] = "Фамилия";
            worksheet.Cells[2, 3] = "Имя";
            worksheet.Cells[2, 4] = "Отчество";
            worksheet.Cells[2, 5] = "Телефон";

            int row = 3; // Заполняем таблицу данными
            foreach (var owner in owners)
            {
                worksheet.Cells[row, 1] = owner.OwnerId.ToString();
                worksheet.Cells[row, 2] = owner.Surname;
                worksheet.Cells[row, 3] = owner.Name;
                worksheet.Cells[row, 4] = owner.Patronymic;
                worksheet.Cells[row, 5] = owner.Phone;
                row++;
            }
            // Добавляем границы и выравниваем столбцы по содержимому
            excel.Range tableRange = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[row - 1, 5]];
            tableRange.Borders.LineStyle = excel.XlLineStyle.xlContinuous;
            tableRange.Columns.AutoFit();

            excelApp.Visible = true;
        }
    }
}
