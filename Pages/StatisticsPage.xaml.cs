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
using Excel = Microsoft.Office.Interop.Excel;
using LiveCharts;
using LiveCharts.Wpf;

namespace VeterinaryСlinic.Pages
{
    public partial class StatisticsPage : Page
    {
        List<Patients> patients;
        DataBase clinic = new DataBase(MainWindow.baza);
        public StatisticsPage()
        {
            InitializeComponent();
            var patients = MainWindow.baza.Patients.ToList();
            var animalCounts = patients.GroupBy(p => p.ViewId) // Изменено на ViewId
                                       .Select(g => new { AnimalName = g.First().View.Name, RecordCount = g.Count() }) // Изменено на g.First().View.Name
                                       .OrderByDescending(a => a.RecordCount)
                                       .ToList();

            ChartValues<int> vs = new ChartValues<int>();
            foreach (var breedCount in animalCounts)
            {
                vs.Add(breedCount.RecordCount);
            }

            SeriesCollection seriesViews = new SeriesCollection();
            for (int i = 0; i < animalCounts.Count; i++)
            {
                seriesViews.Add(new PieSeries()
                {
                    Title = animalCounts[i].AnimalName,
                    Values = new ChartValues<int> { vs[i] },
                    PushOut = 15,
                    DataLabels = true
                });
            }

            myChart.Series = seriesViews;
            myChart.LegendLocation = LegendLocation.Right;
            myChart.FontSize = 18;
            DataContext = this;
        }

        private void StatisticsExcel(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workBook = excelApp.Workbooks.Add();
            Excel.Worksheet workSheet = workBook.Worksheets[1];
            excelApp.Visible = true;
            excelApp.UserControl = true;

            var patients = MainWindow.baza.Patients.ToList();
            var animalCounts = patients.GroupBy(p => p.ViewId) // Изменено на ViewId
                                       .Select(g => new { AnimalName = g.First().View.Name, RecordCount = g.Count() }) // Изменено на g.First().View.Name
                                       .OrderByDescending(a => a.RecordCount)
                                       .ToList();

            workSheet.Cells[1, 1] = "Животное";
            workSheet.Cells[1, 2] = "Количество записей";

            int rowNum = 2;
            foreach (var animalCount in animalCounts)
            {
                workSheet.Cells[rowNum, 1] = animalCount.AnimalName;
                workSheet.Cells[rowNum, 2] = animalCount.RecordCount;
                rowNum++;
            }

            Excel.Range dataRange = workSheet.Range["A1:B" + (rowNum - 1).ToString()];
            dataRange.Columns.AutoFit();

            Excel.Range headerRange = workSheet.Range["A1:B1"];
            headerRange.Interior.Color = Excel.XlRgbColor.rgbYellow;
            headerRange.Font.Bold = true;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        }
    }
}
