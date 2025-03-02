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
using System.IO;

namespace VeterinaryСlinic.Pages
{
    public partial class UsersPage : Page
    {
        public static Veterinary_Clinic baza;
        string path = "AuthorizationHistory.txt";
        public UsersPage()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            dgUsers.ItemsSource = baza.Users.ToList();
            UpdateListBox();
        }
        /// <summary>
        /// Кнопка для редактирования данных пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem != null)
            {
                var selected = dgUsers.SelectedItem as Users;
                var editWindow = new Windows.WindowEditUsers(selected);
                if (editWindow.ShowDialog() == true)
                {
                    dgUsers.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя из таблицы для редактирования");
            }
        }

        /// <summary>
        /// Функция для чтения и записи данных из файла
        /// </summary>
        private void UpdateListBox()
        {
            ListHistory.Items.Clear();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    ListHistory.Items.Add(line);
                }
            }
            else
            {
                MessageBox.Show("Файл не найден");
            }
        }

        /// <summary>
        /// Кнопка для удалении истории авторизации пользователей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTheHistory(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(path, string.Empty);
            UpdateListBox();
        }

        /// <summary>
        /// Кнопка для добавления учетной записи пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Before(object sender, RoutedEventArgs e)
        {
            Users user = new Users();
            user.UserId = 0;
            var editWindow = new Windows.WindowEditUsers(user);
            if (editWindow.ShowDialog() == true)
            {
                dgUsers.ItemsSource = MainWindow.baza.Users.ToList();
                dgUsers.Items.Refresh();
            }
        }

        /// <summary>
        /// Кнопка для удаления учетной записи пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove(object sender, RoutedEventArgs e)
        {
            var delete = dgUsers.SelectedItem as Users;
            if (delete != null)
            {
                MessageBoxResult result = MessageBox.Show
                ("Вы точно хотите удалить пользователя?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    var tracked = MainWindow.baza.Users.Find(delete.UserId);
                    if (tracked != null)
                    {
                        MainWindow.baza.Users.Remove(tracked);
                        MainWindow.baza.SaveChanges();
                        MessageBox.Show("Запись удалена !");
                        dgUsers.ItemsSource = null;
                        dgUsers.ItemsSource = MainWindow.baza.Users.ToList();
                    }
                }
            }
            else MessageBox.Show("Пожалуйста, выберите пользователя из таблицы для удаления");
        }

        /// <summary>
        /// Обновление данных в dgUsers по нажатию на изображение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            baza = new Veterinary_Clinic();
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = baza.Users.ToList(); 
        }
    }
}
