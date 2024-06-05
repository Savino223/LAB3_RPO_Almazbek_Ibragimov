using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

namespace Matrixx
{
    public static class MatrixIO
    {
        public static async Task WriteTextAsync(Stream stream, Matrix mtrx, char sep = ' ')
        {
            using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"{mtrx.rows} {mtrx.cols}");
            for (int i = 0; i < mtrx.rows; i++)
            {
                for (int j = 0; j < mtrx.cols; j++)
                {
                    await writer.WriteLineAsync(mtrx[i, j].ToString());
                    if (j < mtrx.cols - 1)
                    {
                        await writer.WriteLineAsync(" ");
                    }
                }
                await writer.WriteLineAsync();
            }
        }

        public static async Task<Matrix> ReadTextAsync(Stream stream, char sep = ' ')
        {
            using var reader = new StreamReader(stream);
            var size = (await reader.ReadLineAsync()).Split();
            int rows = int.Parse(size[0]);
            int cols = int.Parse(size[1]);

            var values = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                var line = (await reader.ReadLineAsync()).Split(sep);
                for (int j = 0; j < cols; j++)
                {
                    values[i, j] = double.Parse(line[j]);
                }
            }
            return new Matrix(values);
        }

        public static void WriteBinary(Stream stream, Matrix mtrx)
        {
            using var writer = new BinaryWriter(stream);
            writer.Write(mtrx.rows);
            writer.Write(mtrx.cols);
            for (int i = 0; i < mtrx.rows; i++)
            {
                for (int j = 0; j < mtrx.cols; j++)
                {
                    writer.Write(mtrx[i, j]);
                }
            }
        }

        public static Matrix ReadBinary(Stream stream)
        {
            using var reader = new BinaryReader(stream);
            int rows = reader.ReadInt32();
            int cols = reader.ReadInt32();

            var vals = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    vals[i, j] = reader.ReadDouble();
                }
            }
            return new Matrix(vals);
        }

        public static async Task WriteJsonAsync(Stream stream, Matrix mtrx)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var vals = new double[mtrx.rows][];
            for (int i = 0; i < mtrx.rows; i++)
            {
                vals[i] = new double[mtrx.cols];
                for (int j = 0; j < mtrx.cols; j++)
                {
                    vals[i][j] = mtrx[i, j];
                }
            }
            await JsonSerializer.SerializeAsync(stream, vals, opts);
        }

        public static async Task<Matrix> ReadJsonAsync(Stream stream)
        {
            var vals = await JsonSerializer.DeserializeAsync<double[][]>(stream);
            if (vals == null || vals.Length == 0 || vals[0].Length == 0)
            {
                throw new InvalidOperationException("Неправильные формат JSON для матрицы.");
            }
            var rows = vals.Length;
            var cols = vals[0].Length;
            var arr = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    arr[i, j] = vals[i][j];
                }
            }
            return new Matrix(arr);
        }

        public static void WriteToFile(string dir, string filename, Matrix mtrx, Action<Matrix, Stream> write_method)
        {
            Directory.CreateDirectory(dir);
            var filepath = Path.Combine(dir, filename);
            using var file_stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            write_method(mtrx, file_stream);
        }

        public static async Task WriteToFileAsync(string dir, string filename, Matrix mtrx, Func<Matrix, Stream, Task> write_method)
        {
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, filename);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await write_method(mtrx, fileStream);
        }

        public static Matrix ReadMatrixFromFile(string file_path, Func<Stream, Matrix> read_method)
        {
            using var fileStream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            return read_method(fileStream);
        }

        public static async Task<Matrix> ReadMatrixFromFileAsync(string file_path, Func<Stream, Task<Matrix>> read_method)
        {
            using var fileStream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
            return await read_method(fileStream);
        }
    }
}