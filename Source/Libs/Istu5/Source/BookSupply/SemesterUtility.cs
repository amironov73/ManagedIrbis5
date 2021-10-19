// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SemesterUtility.cs -- сборник методов для работы с перечислением семестров
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using AM.Collections;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Сборник методов для работы с перечислением семестров.
    /// </summary>
    public static class SemesterUtility
    {
        #region Constants

        /// <summary>
        /// В перечислении ни одного семестра - строковое представление.
        /// </summary>
        public const string NoneString = "--";

        #endregion

        #region Public methods

        /// <summary>
        /// Формирует перечисление семестров из массива целых.
        /// </summary>
        public static Semester FromArray
            (
                int[] array
            )
        {
            var result = Semester.None;
            unchecked
            {
                foreach (var semester in array)
                {
                    result |= (Semester)(1 << (semester - 1));
                }
            }

            return result;
        } // method FromArray

        /// <summary>
        /// Определяет, четные ли указанные семестры
        /// (хотя бы некоторые).
        /// </summary>
        public static bool IsEven (Semester semester) => (semester & Semester.EvenSemesters) != Semester.None;

        /// <summary>
        /// Определяет, нечетные ли указанные семестры
        /// (хотя бы некоторые).
        /// </summary>
        public static bool IsOdd (Semester semester) => (semester & Semester.OddSemesters) != Semester.None;

        /// <summary>
        /// Определяет, пересекаются ли указанные наборы семестров
        /// (например, в первом и во втором наборе одновремено есть четные семесты
        /// или же наоборот - одновременно есть нечетные).
        /// </summary>
        public static bool IsOverlap (Semester first, Semester second) =>
            IsEven (first) == IsEven (second) || IsOdd (first) == IsOdd (second);

        /// <summary>
        /// Разбирает строковое представление перечисления семестров.
        /// </summary>
        public static Semester Parse
            (
                string value
            )
        {
            var result = Semester.None;
            if (value != NoneString)
            {
                var parts = value.Split
                    (
                        new[] { ' ', ';', ',' },
                        StringSplitOptions.RemoveEmptyEntries
                    );

                foreach (var semester in parts)
                {
                    try
                    {
                        result |= (Semester)(1 << (int.Parse (semester, CultureInfo.InvariantCulture) - 1));
                    }
                    catch
                    {
                        result |= (Semester)Enum.Parse (typeof (Semester), semester);
                    }
                } // foreach
            } // if

            return result;
        } // method Parse

        /// <summary>
        /// Преобразует перечисление семестров в массив целых чисел.
        /// </summary>
        public static int[] ToArray
            (
                Semester value
            )
        {
            var result = new LocalList<int>();
            for (var i = 0; i < 32; i++)
            {
                if (((int)value & (1 << i)) != 0)
                {
                    result.Add (i + 1);
                }
            } // for

            return result.ToArray();
        } // method ToArray

        /// <summary>
        /// Преобразует перечисление семестров в массив номеров курсов
        /// (с нумерацией от 1).
        /// </summary>
        public static int[] ToCourses
            (
                Semester value
            )
        {
            var result = new HashSet<int>();

            foreach (var semester in ToArray (value))
            {
                result.Add ((semester + 1) / 2);
            }

            return result.ToArray();
        } // method ToCourses

        /// <summary>
        /// Преобразует перечисление семестров в строку.
        /// </summary>
        public static string ToString (Semester value)
            => value == Semester.None
                ? NoneString
                : string.Join (";", ToArray (value));

        #endregion

    } // class SemesterUtility

} // namespace Istu.BookSupply
