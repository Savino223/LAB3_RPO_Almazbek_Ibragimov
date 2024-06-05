using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;

namespace Matrixx
{

    public class MatrixScenarios
    {

        public static Matrix MultiplyMatrixArray(Matrix[] matrixes)
        {
            if (matrixes.Length <= 1)
            {
                throw new ArgumentException("Недостаточно матриц!");
            }

            Matrix res = matrixes[0];
            for (int i = 1; i < matrixes.Length; i++)
            {
                res *= matrixes[i];
            }
            return res;
        }

        public static Matrix CreateRandom(int rows, int cols)
        {
            Random rand = new Random();
            double minval = -10.0;
            double maxval = 10.0;
            double[,] values = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    values[i, j] = rand.NextDouble() * (maxval - minval) + minval;
                }
            }
            return new Matrix(values);
        }

        public static void SaveMatrixesToDirectory(Matrix[] matrixes, string dir, string pref, string ext, Func<Matrix, Stream, Task> write_method)
        {
            for (int i = 0; i < matrixes.Length; i++)
            {
                var fileName = $"{pref}{i}{ext}";
                MatrixIO.WriteToFileAsync(dir, fileName, matrixes[i], write_method).Wait();
                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine($"Saved {i + 1} matrices.");
                }
            }
        }

        public static async Task<Matrix[]> ReadMatrixesFromDirectory(string dir, string pref, string ext, Func<Stream, Task<Matrix>> read_method)
        {
            var files = Directory.GetFiles(dir, $"{pref}*{ext}");
            var matrixes = new Matrix[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                matrixes[i] = await MatrixIO.ReadMatrixFromFileAsync(files[i], read_method);
            }
            return matrixes;
        }

    }
}