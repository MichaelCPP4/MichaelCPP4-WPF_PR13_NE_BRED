using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LibMas
{
    public class ClassArray
    {
        /// <summary>
        /// Метод выполняет поиск столбцов двумерного массива в котором числа расположены в порядке убывания
        /// </summary>
        /// <param name="array">Двумерный целочисленный массив</param>
        /// <returns>Кол-во столбцов</returns>
        static public int FindToColums(int[,] array)
        {
            int kol = array.GetLength(1);

            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    if (i > 0 && array[i, j] > array[i - 1, j])
                    {
                        kol--;
                        break;
                    }
                }
            }
            return kol;
        }

    }
}
