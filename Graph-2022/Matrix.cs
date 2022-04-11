using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Matrix
    {
        private double[,] matrix;

        public double[,] Matr
        {
            get
            {
                return matrix;
            }
            set
            {
                matrix = value;
            }
        }

        public double this[int i, int j]
        {
            get
            {
                if (i > matrix.GetLength(0) || j > matrix.GetLength(1) || i < 0 || j < 0)
                    throw new Exception("Incorrect index");
                return matrix[i, j];
            }
            set
            {
                if (i > matrix.GetLength(0) || j > matrix.GetLength(1) || i < 0 || j < 0)
                    throw new Exception("Incorrect index");
                matrix[i, j] = value;
            }
        }

        public Matrix()
        {
            matrix = new double[1, 1];
        }

        public Matrix(int m)
        {
            if (m <= 0) throw new Exception("Неверно заданы размерности");
            this.matrix = new double[m, m];
            for (int i = 0; i < m; i += 1)
                matrix[i, i] = 1;
        }

        public Matrix(int m, int n)
        {
            if (m <= 0 || n <= 0) throw new Exception("Неверно заданы размерности");
            matrix = new double[m, n];
        }

        public Matrix(Matrix other)
        {
            matrix = (double[,])other.matrix.Clone();
        }

        public Matrix(double[,] a)
        {
            int m = a.GetUpperBound(0) + 1;
            int n = a.Length / m;

            matrix = new double[m, n];
            for (int i = 0; i < m; i += 1)
            {
                for (int j = 0; j < n; j += 1)
                {
                    matrix[i, j] = a[i, j];
                }
            }
        }

        protected int[] GetSize()
        {
            int[] size = new int[2];
            size[0] = this.matrix.GetUpperBound(0) + 1;
            size[1] = this.matrix.Length / size[0];
            return size;
        }

        public void ProcessFunctionOverMatrix(Action<int, int> func)
        {
            int[] size = GetSize();
            for (int i = 0; i < size[0]; i += 1)
            {
                for (int j = 0; j < size[1]; j += 1)
                {
                    func(i, j);
                }
            }
        }

        protected Matrix CreateMatrixWithoutColumn(int column)
        {
            int[] size = GetSize();
            if (column < 0 || column >= size[1])
            {
                throw new ArgumentException("invalid column index");
            }
            var result = new Matrix(size[0], size[1] - 1);
            result.ProcessFunctionOverMatrix((i, j) => result[i, j] = j < column ? this[i, j] : this[i, j + 1]);
            return result;
        }

        protected Matrix CreateMatrixWithoutRow(int row)
        {
            var size = GetSize();
            if (row < 0 || row >= size[0])
            {
                throw new ArgumentException("invalid row index");
            }
            var result = new Matrix(size[0] - 1, size[1]);
            result.ProcessFunctionOverMatrix((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]);
            return result;
        }

        protected static Matrix Swap(ref Matrix res, int row1, int row2)
        {
            int[] size = res.GetSize();
            for (int i = 0; i < size[1]; i += 1)
            {
                double tmp = res[row1, i];
                res[row1, i] = res[row2, i];
                res[row2, i] = tmp;
            }
            return res;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            int[] size1 = m1.GetSize();
            int[] size2 = m2.GetSize();
            if (!size1.SequenceEqual(size2)) throw new Exception("Матрицы имеют разные размерности");
            Matrix res = new Matrix(size1[0], size1[1]);
            for (int i = 0; i < size1[0]; i += 1)
            {
                for (int j = 0; j < size1[1]; j += 1)
                {
                    res[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return res;
        }

        public static Matrix operator +(Matrix m, double k)
        {
            int[] size = m.GetSize();
            Matrix res = new Matrix(size[0], size[1]);

            for (int i = 0; i < size[0]; i += 1)
            {
                for (int j = 0; j < size[1]; j += 1)
                {
                    res[i, j] = m[i, j] + k;
                }
            }

            return res;
        }

        public static Matrix operator *(Matrix m, double v)
        {
            int[] size = m.GetSize();
            Matrix res = new Matrix(size[0], size[1]);

            for (int i = 0; i < size[0]; i += 1)
            {
                for (int j = 0; j < size[1]; j += 1)
                {
                    res[i, j] = m[i, j] * v;
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int[] size1 = m1.GetSize();
            int[] size2 = m2.GetSize();
            if (!size1.SequenceEqual(size2)) throw new Exception("Матрицы имеют разные размерности");

            Matrix res = new Matrix(size1[0], size1[1]);
            res = m1 + (m2 * (-1));
            return res;
        }

        public static Matrix operator -(Matrix m, double v)
        {
            Matrix res = new Matrix(m);

            return res + (-v);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int[] size1 = m1.GetSize();
            int[] size2 = m2.GetSize();
            if (size1[1] != size2[0]) throw new Exception("Матрицы имеют неккоректные размерности");

            Matrix res = new Matrix(size1[0], size2[1]);

            for (int i = 0; i < size1[0]; i += 1)
            {
                for (int j = 0; j < size2[1]; j += 1)
                {
                    for (int k = 0; k < size1[1]; k += 1)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }

            return res;
        }

        public static Matrix operator /(Matrix m, double v)
        {
            int[] size = m.GetSize();
            Matrix res = new Matrix(m);

            for (int i = 0; i < size[0]; i += 1)
            {
                for (int j = 0; j < size[1]; j += 1)
                {
                    res[i, j] /= v;
                }
            }

            return res;
        }

        public static Matrix operator ~(Matrix m) //Транспонирование
        {
            int[] size = m.GetSize();
            if (size[0] != size[1]) throw new Exception("Невозможно транспонировать матрицу");
            Matrix res = new Matrix(size[0], size[1]);
            res.ProcessFunctionOverMatrix((i, j) => res[i, j] = m[j, i]);
            return res;
        }

        public override int GetHashCode()
        {
            return Matr.GetHashCode();
        }

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            int[] size1 = m1.GetSize();
            int[] size2 = m2.GetSize();

            if (!size1.SequenceEqual(size2)) return false;

            for (int i = 0; i < size1[0]; i += 1)
            {
                for (int j = 0; j < size1[1]; j += 1)
                {
                    if (Math.Abs(m1[i, j] - m2[i, j]) > 1.00E-9) return false;
                }
            }

            return true;
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            int[] size = GetSize();

            for (int i = 0; i < size[0]; i += 1)
            {
                for (int j = 0; j < size[1]; j += 1)
                {
                    if (Math.Abs(matrix[i, j]) < 1.00E-9)
                    {
                        res.Append("0 ");
                        continue;
                    }
                    res.Append(matrix[i, j]);
                    res.Append(" ");
                }
                res.Append('\n');
            }
            return res.ToString();
        }

        public override bool Equals(object obj)
        {
            if(obj is Matrix other)
            {
                var size1 = GetSize();
                var size2 = other.GetSize();
                if(!size1.SequenceEqual(size2)) return false;
                   
                for(int i = 0; i < size1[0]; i += 1)
                {
                   for(int j = 0; j < size1[1]; j += 1)
                    {
                        if (this[i, j] != other[i, j]) return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
    