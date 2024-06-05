using System;

namespace Matrixx
{

    public class Matrix
    {
        public double[,] matrix;

        public int cols { get { return matrix.GetLength(1); } }
        public int rows { get { return matrix.GetLength(0); } }
        public int? Hash;

        public Matrix(int r, int c)
        {
            matrix = new double[r, c];
            Hash = null;
        }

        public Matrix(double[,] vals)
        {
            matrix = vals;
            Hash = null;
        }

        public double this[int i, int j]
        {
            get { return matrix[i, j]; }
        }

        public static Matrix Zero(int r, int c)
        {
            double[,] vals = new double[r, c];
            return new Matrix(vals);
        }

        public static Matrix Zero(int n)
        {
            return new Matrix(n, n);
        }

        public static Matrix Identity(int n)
        {
            double[,] vals = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                vals[i, i] = 1.0;
            }
            return new Matrix(vals);
        }

        public Matrix Transpose()
        {
            return MatrixOps.Transpose(this);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result += matrix[i, j] + " ";
                }
                result += "\n";
            }
            return result;
        }

        public override bool Equals(object oo)
        {
            if (oo == null || oo is not Matrix)
            {
                return false;
            }

            Matrix srav = (Matrix)oo;

            if (srav.rows != rows || srav.cols != cols)
            {
                return false;
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (matrix[i, j] != srav[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public override int GetHashCode()
        {
            if (Hash != null)
            {
                return Hash.Value;
            }

            int hash = CountHash();
            Hash = hash;
            return hash;
        }

        private int CountHash()
        {
            int hash = 17;
            hash = hash * 23 + rows.GetHashCode();
            hash = hash * 23 + cols.GetHashCode();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    hash = hash * 23 + matrix[i, j].GetHashCode();
                }
            }
            return hash;
        }

        public static Matrix operator *(Matrix matrix, double scalar)
        {
            return MatrixOps.MultiplyByNum(matrix, scalar);
        }

        public static Matrix operator -(Matrix matrix)
        {
            return MatrixOps.MultiplyByNum(matrix, -1.0);
        }

        public static Matrix operator ~(Matrix matrix)
        {
            return MatrixOps.Transpose(matrix);
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            return MatrixOps.Multiply(matrix1, matrix2);
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            return MatrixOps.MplusM(matrix1, matrix2);
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            return MatrixOps.MminusM(matrix1, matrix2);
        }

        public static Matrix operator !(Matrix matrix)
        {
            return MatrixOps.Inverse(matrix).Item1;
        }

        public static Matrix operator /(Matrix matrix1, Matrix matrix2)
        {
            return matrix1 * !matrix2;
        }

    }
}