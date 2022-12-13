using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using LibMas;

namespace Pr21
{
    public partial class MainWindow : Window
    {
        //Объевление массива
        int[,] mas;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Событие кнопки "Запиать"
        private void Zap_Click(object sender, RoutedEventArgs e)
        {
            vivod.Clear();

            int randMin;

            int randMax;

            int count;

            int row;

            Random rnd = new Random();

            if (int.TryParse(diapozonMin.Text, out randMin) && int.TryParse(diapozon.Text, out randMax) && int.TryParse(columnCount.Text, out count) && int.TryParse(rowKol.Text, out row))
            {
                if (randMin>randMax)
                {
                    MessageBox.Show("Минимальный диапозон не может быть больше максимального", "ОШИБКА");
                }
                else
                {
                    mas = new int[row, count];

                    for (int i = 0; i < mas.GetLength(0); i++)
                    {
                        for (int j = 0; j < mas.GetLength(1); j++)
                        {
                            mas[i, j] = rnd.Next(randMin, randMax + 1);
                        }
                    }

                    dataGrid.ItemsSource = VisualArray.ToDataTable(mas).DefaultView;
                }
            }
            else
            {
                MessageBox.Show("Введите значения", "ОШИБКА");
            }
        }

        //Событие кнопки "Расчитать"
        private void Ras_Click(object sender, RoutedEventArgs e)
        {
            vivod.Clear();
            if (mas != null)
            {
                /*
                 * int kol = mas.GetLength(1);

                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    for (int i = 0; i < mas.GetLength(0); i++)
                    {
                        //current = mas[i, j];
                        if (i > 0 && mas[i, j] > mas[i - 1, j])
                        {
                            kol--;
                            break;
                        }
                    }
                }*/

                vivod.Text = vivod.Text + "\nКолличество столбцов: " + ClassArray.FindToColums(mas);
            }
        }

        //Событие кнопки "Создать"
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            vivod.Clear();

            int count;
            int row;

            if (int.TryParse(columnCount.Text, out count) && int.TryParse(rowKol.Text, out row))
            {
                mas = new int[row, count];

                dataGrid.ItemsSource = VisualArray.ToDataTable(mas).DefaultView;
            }
            else
            {
                MessageBox.Show("Введите значения", "ОШИБКА");
            }
        }

        //Событие кнопки "Очистить"
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            mas = null;
        }

        //Событие кнопки "Выход"
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Событие редактирования ячеек
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int indexColumn = e.Column.DisplayIndex;

            int indexRow = e.Row.GetIndex();

            mas[indexRow, indexColumn] = Convert.ToInt32(((TextBox)e.EditingElement).Text);
        }

        //Событие кнопки "О программе"
        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана матрица размера M * N. Найти количество ее столбцов, элементы которых\r\nупорядочены по убыванию.\r\nВыполнил Иванов Михаил ИСП-31", "О программе");
        }

        //Объявляем таймер
        DispatcherTimer timer;

        //Событие запуска окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.IsEnabled = true;

            //number.Text = "Задание №1";
        }

        //Событие таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mas != null)
            {

                textBlockCurrentCell.Text = $"Размер таблицы: {mas.GetLength(0)}x{mas.GetLength(1)}";
            }
            DataGrid x = (DataGrid)this.FindName("dataGrid");
            //var index = x.SelectedIndex;
            if (x.SelectedIndex != -1)
            {
                textBlockSizeTab.Text = $"Текущая ячейка: {x.SelectedIndex+1}";
            }
        }

        //Событие кнопки "Сохранить"
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            save.FilterIndex = 2;
            save.Title = "Сохранение таблицы";

            if (save.ShowDialog() == true)
            {
                StreamWriter file = new StreamWriter(save.FileName);

                file.WriteLine(mas.GetLength(0));
                file.WriteLine(mas.GetLength(1));

                for (int i = 0; i < mas.GetLength(0); i++)
                {
                    for (int j = 0; j < mas.GetLength(1); j++)
                    {
                        file.WriteLine(mas[i, j]);
                    }
                }
                file.Close();
            }
        }

        //Событие кнопки "Открыть"
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";

            if (open.ShowDialog() == true)
            {
                StreamReader file = new StreamReader(open.FileName);

                int row = Convert.ToInt32(file.ReadLine());
                int column = Convert.ToInt32(file.ReadLine());

                mas = new Int32[row, column];

                for (int i = 0; i < mas.GetLength(0); i++)
                {
                    for (int j = 0; j < mas.GetLength(1); j++)
                    {
                        mas[i, j] = Convert.ToInt32(file.ReadLine());
                    }
                }

                file.Close();

                dataGrid.ItemsSource = VisualArray.ToDataTable(mas).DefaultView;
            }
        }

        //Событие кнопки "Очистка таблицы и результата при вводе новых данных"
        private void TextBoxVvod_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            mas = null;
            vivod.Clear();
        }
    }
}