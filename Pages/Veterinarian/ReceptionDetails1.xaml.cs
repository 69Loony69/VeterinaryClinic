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
    public partial class ReceptionDetails1 : Page
    {
        private Reception reception; 
        public ReceptionDetails1(Reception reception)
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
            NavigationService.Navigate(new ReceptionPage1());
        }
        private void Refresh()
        {
            dgReceptionDetails.ItemsSource = null;
            dgReceptionDetails.ItemsSource = new List<Reception> { reception };
        }

        /// <summary>
        /// Редактировать запись
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgReceptionDetails.Items.Count > 0)
            {
                var firstItem = dgReceptionDetails.Items[0] as Reception;
                var editWindow = new Windows.Veterinarian.WindowEditReception(firstItem);
                if (editWindow.ShowDialog() == true)
                {
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show("Список пациентов пуст");
            }
        }
        /// <summary>
        /// Назначение лечения по приему 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Purpose_treatment(object sender, RoutedEventArgs e)
        {   if (dgReceptionDetails.SelectedItem != null)
            {
                Reception selectedReception = (Reception)dgReceptionDetails.SelectedItem;
                Treatment rs = new Treatment { ReceptionId = selectedReception.ReceptionId };
                Windows.Veterinarian.WindowAddEditTreatmentPatients window = new Windows.Veterinarian.WindowAddEditTreatmentPatients(rs);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Список пациентов пуст");
            }
        }
    }
}
