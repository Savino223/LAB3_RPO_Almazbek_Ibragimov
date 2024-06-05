using System;

namespace Matrixx
{
    public class Ops_Exceptions
    {
        public class WrongSize : Exception
        {
            public WrongSize() : base("Размеры матриц не совпадают!!") { }
            public WrongSize(string message) : base(message) { }
        }

        public class NotSquare : Exception
        {
            public NotSquare() : base("Матрица не квадратная!") { }
            public NotSquare(string message) : base(message) { }
        }

        public class Singularity : Exception
        {
            public Singularity() : base("Инверсии не существует! Матрица сингулярная!") { }
            public Singularity(string message) : base(message) { }
        }

        public class WrongSizeMulti : Exception
        {
            public WrongSizeMulti() : base("Кол-во колонок в первой матрице должно быть равно кол-ву строк во втором!") { }
            public WrongSizeMulti(string message) : base(message) { }
        }
    }
}