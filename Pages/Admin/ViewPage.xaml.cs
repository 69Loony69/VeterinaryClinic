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
    public partial class ViewPage : Page
    {
        public static Veterinary_Clinic baza;
        public ViewPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgView.ItemsSource = baza.View.ToList();
        }

        /// <summary>
        /// Кнопка для редактирования данных вида животного
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgView.SelectedItem != null)
            {
                var selected = dgView.SelectedItem as View;
                var editWindow = new Windows.WindowEditView(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgView.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите вид животного из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Кнопка для добавления диагноза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            View view = new View();
            view.ViewId = 0;
            var editWindow = new Windows.WindowEditView(view);
            if (editWindow.ShowDialog() == true)
            {
                dgView.ItemsSource = MainWindow.baza.View.ToList();
                dgView.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления вида
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgView.SelectedItem as View;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить вид?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.View.Find(delete.ViewId);
                    if (tracked != null)
                    {
                        MainWindow.baza.View.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgView.ItemsSource = null;
                        dgView.ItemsSource = MainWindow.baza.View.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите вид животного из таблицы для удаления");
        }

        /// <summary>
        /// Поиск вида животного по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.View.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.ViewId.ToString().StartsWith(searchText.ToLower())
                               || p.Name.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            dgView.ItemsSource = x;
        }

        /// <summary>
        /// Обновление данных в dgView по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgView.ItemsSource = null;
            dgView.ItemsSource = baza.View.ToList();
        }
    }
}
