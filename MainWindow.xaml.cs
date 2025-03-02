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

namespace VeterinaryСlinic
{
    public partial class MainWindow : Window
    {
        public static Veterinary_Clinic baza;
        public MainWindow()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();
            cbUser.ItemsSource = MainWindow.baza.Users.ToList();
        }
        
        /// <summary>
        /// Кнопка авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Authorization(object sender, RoutedEventArgs e)
        {
            string logFilePath = "AuthorizationHistory.txt";
            if (cbUser.SelectedItem is Users selectedUser)
            {
                string user = selectedUser.User;
                string password = PasswordUser?.Password;
                string logEntry = $"{DateTime.Now}: Попытка входа пользователя {user}. ";

                try
                {
                    if (string.IsNullOrEmpty(user)) throw new Exception("Пожалуйста, выберите пользователя из списка.");
                    if (string.IsNullOrEmpty(password)) throw new Exception("Введите пароль.");

                    if (VerifyPassword(user, password))
                    {
                        logEntry += "Статус авторизации: успешный.\n";
                        File.AppendAllText(logFilePath, logEntry);
                        switch (user)
                        {
                            case "Администратор БД":
                                Windows.AdminWindow aw = new Windows.AdminWindow();
                                aw.Show();
                                break;
                            case "Ветеринар":
                                Windows.VeterinarianWindow vw = new Windows.VeterinarianWindow();
                                vw.Show();
                                break;
                            case "Сотрудник регистратуры":
                                Windows.EmployeeWindow ew = new Windows.EmployeeWindow();
                                ew.Show();
                                break;
                        }
                        this.Close();
                    }
                    else
                    {
                        logEntry += "Статус авторизации: неудачный.\n";
                        File.AppendAllText(logFilePath, logEntry);
                    }
                }
                catch (Exception ex)
                {
                    logEntry += $"Ошибка: {ex.Message}\n";
                    File.AppendAllText(logFilePath, logEntry);
                    MessageBox.Show($"Ошибка: {ex.Message}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя из списка.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
       
    /// <summary>
    /// Проверка ввода пароля
    /// </summary>
    /// <param name="accessLevel"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool VerifyPassword(string accessLevel, string password)
        {
            try
            {
                var user = baza.Users.FirstOrDefault(u => u.User == accessLevel && u.Password == password);

                if (user == null)
                {
                    MessageBox.Show("Ошибка: Неверный пароль.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
