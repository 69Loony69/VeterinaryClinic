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
    public partial class ReceptionDetails : Page
    {
        private Reception reception;
        public ReceptionDetails(Reception reception)
        {
            InitializeComponent();
            this.reception = reception;

            // Создайте коллекцию и добавьте в нее reception
            var receptionList = new List<Reception> { reception };

            // Установите коллекцию в качестве DataContext
            DataContext = receptionList;

            dgReceptionDetails.ItemsSource = DataContext as List<Reception>;

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReceptionPage());
        }
        /// <summary>
        /// Вывод данных о приёме в Word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToWord(object sender, RoutedEventArgs e)
        {
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            var document = wordApp.Documents.Add();

            var titleRange = document.Content;
            titleRange.Text = "Данные о приеме\n\n";
            titleRange.Font.Name = "Times New Roman";
            titleRange.Font.Size = 16;
            titleRange.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;

            var contentRange = document.Content;
            contentRange.InsertAfter($"ФИО владельца: {reception.Patients.Owners.FullName}\n");
            contentRange.InsertAfter($"Телефон: {reception.Patients.Owners.Phone}\n");
            contentRange.InsertAfter($"Кличка: {reception.Patients.Name}\n");
            contentRange.InsertAfter($"Вид: {reception.Patients.View.Name}\n");
            contentRange.InsertAfter($"Наличие породы: {reception.Patients.Breed}\n");
            contentRange.InsertAfter($"Пол: {reception.Patients.Paul}\n");
            contentRange.InsertAfter($"Дата рождения: {reception.Patients.FormattedDayOfBirth}\n");
            contentRange.Font.Name = "Times New Roman";
            contentRange.Font.Size = 14;
            contentRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify; // Выравнивание по ширине

            var range = document.Content;
            range.InsertAfter("\n");
            range.Start = range.End;
            var table = document.Tables.Add(range, dgReceptionDetails.Items.Count + 1, dgReceptionDetails.Columns.Count);
            table.Borders.Enable = 1; // Включить границы таблицы
            for (int i = 0; i < dgReceptionDetails.Columns.Count; i++)
            {
                var cell = table.Cell(1, i + 1);
                cell.Range.Text = dgReceptionDetails.Columns[i].Header.ToString();
                cell.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter; // Выравнивание по центру
            }
            for (int i = 0; i < dgReceptionDetails.Items.Count; i++)
            {
                for (int j = 0; j < dgReceptionDetails.Columns.Count; j++)
                {
                    table.Cell(i + 2, j + 1).Range.Text = (dgReceptionDetails.Columns[j].GetCellContent(dgReceptionDetails.Items[i]) as TextBlock).Text;
                }
            }
            wordApp.Visible = true;
        }

        /// <summary>
        /// Вывод данных о приёме в Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            var excelApp = new excel.Application();
            var workbook = excelApp.Workbooks.Add();
            var worksheet = workbook.Worksheets[1];

            worksheet.Cells[1, 1].Value = "Данные о приеме";
            worksheet.Cells[1, 1].Font.Size = 14;
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 2]].Merge();
            worksheet.Cells[1, 1].HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;

            worksheet.Cells[2, 1].Value = "ФИО владельца:";
            worksheet.Cells[2, 2].Value = reception.Patients.Owners.FullName;
            worksheet.Cells[3, 1].Value = "Телефон:";
            worksheet.Cells[3, 2].Value = reception.Patients.Owners.Phone;
            worksheet.Cells[4, 1].Value = "Кличка:";
            worksheet.Cells[4, 2].Value = reception.Patients.Name;
            worksheet.Cells[5, 1].Value = "Вид:";
            worksheet.Cells[5, 2].Value = reception.Patients.View.Name;
            worksheet.Cells[6, 1].Value = "Наличие породы:";
            worksheet.Cells[6, 2].Value = reception.Patients.Breed;
            worksheet.Cells[7, 1].Value = "Пол:";
            worksheet.Cells[7, 2].Value = reception.Patients.Paul;
            worksheet.Cells[8, 1].Value = "Дата рождения:";
            worksheet.Cells[8, 2].Value = reception.Patients.FormattedDayOfBirth;

            var startRow = 10;
            for (int i = 0; i < dgReceptionDetails.Columns.Count; i++)
            {
                worksheet.Cells[startRow, i + 1].Value = dgReceptionDetails.Columns[i].Header.ToString();
                worksheet.Cells[startRow, i + 1].HorizontalAlignment = excel.XlHAlign.xlHAlignCenter;
            }
            for (int i = 0; i < dgReceptionDetails.Items.Count; i++)
            {
                for (int j = 0; j < dgReceptionDetails.Columns.Count; j++)
                {
                    worksheet.Cells[i + startRow + 1, j + 1].Value = (dgReceptionDetails.Columns[j].GetCellContent(dgReceptionDetails.Items[i]) as TextBlock).Text;
                }
            }
            excel.Range tableRange = worksheet.Range[worksheet.Cells[10, 1], worksheet.Cells[startRow + dgReceptionDetails.Items.Count, 6]];
            tableRange.Borders.LineStyle = excel.XlLineStyle.xlContinuous;
            excelApp.Visible = true;
        }
    }
}
