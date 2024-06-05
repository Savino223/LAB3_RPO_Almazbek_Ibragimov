using System;
using Matrixx;

namespace Matrixx
{

    public static class MatrixOps
    {

        public static Matrix Transpose(Matrix mtrx)
        {
            double[,] res = new double[mtrx.cols, mtrx.rows];
            for (int i = 0; i < mtrx.rows; i++)
            {
                for (int j = 0; j < mtrx.cols; j++)
                {
                    res[j, i] = mtrx.matrix[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix MultiplyByNum(Matrix mtrx, double num)
        {
            double[,] res = new double[mtrx.cols, mtrx.rows];
            for (int i = 0; i < mtrx.rows; i++)
            {
                for (int j = 0; j < mtrx.cols; j++)
                {
                    res[j, i] = mtrx.matrix[i, j] * num;
                }
            }
            return new Matrix(res);
        }

        public static Matrix MplusM(Matrix mtrx1, Matrix mtrx2)
        {
            if (mtrx1.rows != mtrx2.rows || mtrx1.cols != mtrx2.cols)
            {
                throw new Ops_Exceptions.WrongSize();
            }

            double[,] res = new double[mtrx1.cols, mtrx1.rows];
            for (int i = 0; i < mtrx1.rows; i++)
            {
                for (int j = 0; j < mtrx1.cols; j++)
                {
                    res[j, i] = mtrx1.matrix[i, j] + mtrx2.matrix[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix MminusM(Matrix mtrx1, Matrix mtrx2)
        {
            if (mtrx1.rows != mtrx2.rows || mtrx1.cols != mtrx2.cols)
            {
                throw new Ops_Exceptions.WrongSize();
            }

            double[,] res = new double[mtrx1.cols, mtrx1.rows];
            for (int i = 0; i < mtrx1.rows; i++)
            {
                for (int j = 0; j < mtrx1.cols; j++)
                {
                    res[j, i] = mtrx1.matrix[i, j] - mtrx2.matrix[i, j];
                }
            }
            return new Matrix(res);
        }

        public static Matrix Multiply(Matrix mtrx1, Matrix mtrx2)
        {
            if (mtrx1.cols != mtrx2.rows)
            {
                throw new Ops_Exceptions.WrongSizeMulti();
            }

            Matrix transposed = MatrixOps.Transpose(mtrx2);
            double[,] res = new double[mtrx1.cols, mtrx2.rows];

            Parallel.For(0, mtrx1.rows, i =>
            {
                for (int j = 0; j < transposed.rows; j++)
                {
                    double sum = 0;
                    for (int f = 0; f < mtrx1.cols; f++)
                    {
                        sum += mtrx1.matrix[i, f] * transposed.matrix[j, f];
                    }
                    res[i, j] = sum;
                }
            });
            return new Matrix(res);
        }

        public static (Matrix, double) Inverse(Matrix mtrx)
        {
            if (mtrx.rows != mtrx.cols)
            {
                throw new Ops_Exceptions.NotSquare();
            }

            int n = mtrx.rows;
            double det = 1.0;
            Matrix A = new Matrix(n, n);
            A.matrix = (double[,])mtrx.matrix.Clone();

            double[,] inverse = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                inverse[i, i] = 1.0;
            }

            for (int i = 0; i < n; i++)
            {
                int pRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(A.matrix[j, i]) > Math.Abs(A.matrix[pRow, i]))
                    {
                        pRow = j;
                    }
                }

                if (pRow != i)
                {
                    for (int k = 0; k < n; k++)
                    {
                        double temp = A.matrix[i, k];
                        A.matrix[i, k] = A.matrix[pRow, k];
                        A.matrix[pRow, k] = temp;

                        temp = inverse[i, k];
                        inverse[i, k] = inverse[pRow, k];
                        inverse[pRow, k] = temp;
                    }
                    det *= -1;
                }

                double p = A.matrix[i, i];
                if (p == 0)
                {
                    throw new Ops_Exceptions.Singularity();
                }

                det *= p;
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = A.matrix[j, i] / p;
                        for (int k = 0; k < n; k++)
                        {
                            A.matrix[j, k] -= factor * A.matrix[i, k];
                            inverse[j, k] -= factor * inverse[i, k];
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                double scale = 1.0 / A.matrix[i, i];
                for (int j = 0; j < n; j++)
                {
                    A.matrix[i, j] *= scale;
                    inverse[i, j] *= scale;
                }
            }

            return (new Matrix(inverse), det);
        }

        public static void Display(Matrix mtrx)
        {
            for (int i = 0; i < mtrx.rows; i++)
            {
                for (int j = 0; j < mtrx.cols; j++)
                {
                    Console.Write(mtrx.matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}