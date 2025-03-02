﻿using System;
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
    public partial class PatientsPage1 : Page
    {
        private Veterinary_Clinic baza;
     

        List<string> view = new List<string>();
        List<string> breed = new List<string>();
        List<string> paul = new List<string>();
        
        public PatientsPage1()
        {
            InitializeComponent();
            baza = new Veterinary_Clinic();

            PatientsList.ItemsSource = baza.Patients.ToList();
            Refresh();
            RefreshPage();

            // Фильтр по полу
            paul.Add("Все типы");
            paul.AddRange(MainWindow.baza.Patients.Select(p => p.Paul).Distinct());
            GenderComboBox.ItemsSource = paul;
            GenderComboBox.SelectedItem = "Все типы";

            // Фильтр по виду
            view.Add("Все типы");
            view.AddRange(MainWindow.baza.View.Select(v => v.Name).Distinct());
            SpeciesComboBox.ItemsSource = view;
            SpeciesComboBox.SelectedItem = "Все типы";

            // Фильтр по наличию породы
            breed.Add("Все типы");
            breed.AddRange(MainWindow.baza.Patients.Select(p => p.Breed).Distinct());
            BreedComboBox.ItemsSource = breed;
            BreedComboBox.SelectedItem = "Все типы";
        }
        /// <summary>
        /// Метод для филтров
        /// </summary>
        private void FilterPatients()
        {
            var selectedGender = GenderComboBox.SelectedItem as string;
            var selectedSpecies = SpeciesComboBox.SelectedItem as string;
            var selectedBreed = BreedComboBox.SelectedItem as string;

            var patients = MainWindow.baza.Patients.ToList();

            if (selectedGender != "Все типы")
            {
                patients = patients.Where(p => p.Paul == selectedGender).ToList();
            }

            if (selectedSpecies != "Все типы")
            {
                patients = patients.Where(p => p.View.Name == selectedSpecies).ToList();
            }

            if (selectedBreed != "Все типы")
            {
                patients = patients.Where(p => p.Breed == selectedBreed).ToList();
            }

            PatientsList.ItemsSource = patients;
        }
        /// <summary>
        /// Фильтр для одбора пациентов по полу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPatients();
        }

        /// <summary>
        /// Фильтр для одбора пациентов по виду
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeciesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPatients();
        }

        /// <summary>
        /// Фильтр для одбора пациентов по наличию породы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BreedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPatients();
        }
        /// <summary>
        /// Кнопка для сброса фильтров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset_filters(object sender, RoutedEventArgs e)
        {
            // Установите выбранный элемент каждого комбо-бокса обратно на "Все типы"
            GenderComboBox.SelectedItem = "Все типы";
            SpeciesComboBox.SelectedItem = "Все типы";
            BreedComboBox.SelectedItem = "Все типы";

            // Вызовите функцию FilterPatients(), чтобы обновить список пациентов
            FilterPatients();
        }

        /// <summary>
        /// Обновление данных в PatientsList
        /// </summary>
        private void Refresh()
        {
            PatientsList.ItemsSource = null;
            PatientsList.ItemsSource = baza.Patients.ToList();
        }

        /// <summary>
        /// Поиск пациента по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var x = MainWindow.baza.Patients.ToList();
            string searchText = Search.Text;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                x = x.Where(p => p.PatientId.ToString().ToLower().StartsWith(searchText.ToLower())
                       || p.Owners.FullName.ToLower().StartsWith(searchText.ToLower())
                       || p.Name.ToLower().StartsWith(searchText.ToLower())
                       || p.View.Name.ToLower().StartsWith(searchText.ToLower())
                       || p.Breed.ToLower().StartsWith(searchText.ToLower())
                       || p.Paul.ToLower().StartsWith(searchText.ToLower())
                       || p.FormattedDayOfBirth.ToLower().StartsWith(searchText.ToLower())).ToList();
            }
            PatientsList.ItemsSource = x;
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
            var list = MainWindow.baza.Patients.ToList();
            _maxPages = (int)Math.Ceiling(list.Count * 1.0 / _count);

            var listPage = list.Skip((_currentPage - 1) * _count).Take(_count).ToList();

            TxtCurrentPage.Text = _currentPage.ToString();
            LblTotalPages.Content = "из " + _maxPages;
            LblInfo.Content = $"Всего {list.Count} записей";

            PatientsList.ItemsSource = listPage;
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

        private string sortField = "";
        private string sortOrder = "";
        private void SortOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOption = (sender as ComboBox).SelectedItem as ComboBoxItem;

            if (selectedOption.Content.ToString() == "Владельцу")
            {
                sortField = "Owners.FullName"; // Измените это на "Owners.FullName"
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Visible;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Collapsed;
            }
            else if (selectedOption.Content.ToString() == "Кличке")
            {
                sortField = "Name";
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Visible;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Collapsed;
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
                PatientsList.ItemsSource = baza.Patients.ToList();
                (SortOptions.Items[2] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[3] as ComboBoxItem).Visibility = Visibility.Collapsed;
                (SortOptions.Items[4] as ComboBoxItem).Visibility = Visibility.Collapsed;

                (SortOptions.Items[0] as ComboBoxItem).Visibility = Visibility.Visible;
                (SortOptions.Items[1] as ComboBoxItem).Visibility = Visibility.Visible;
            }
        }

        private void SortData()
        {
            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "По возрастанию")
                {
                    PatientsList.ItemsSource = PatientsList.ItemsSource.Cast<Patients>().OrderBy(p => GetPropertyValue(p, sortField));
                }
                else if (sortOrder == "По убыванию")
                {
                    PatientsList.ItemsSource = PatientsList.ItemsSource.Cast<Patients>().OrderByDescending(p => GetPropertyValue(p, sortField));
                }
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            // Разделите propertyName на части
            var propertyNames = propertyName.Split('.');
            object value = obj;

            // Пройдите по каждому имени свойства
            foreach (var name in propertyNames)
            {
                if (value == null)
                {
                    return null;
                }

                var propertyInfo = value.GetType().GetProperty(name);
                value = propertyInfo.GetValue(value);
            }

            return value;
        }

    }
}
