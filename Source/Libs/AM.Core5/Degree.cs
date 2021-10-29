// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Degree.cs -- градусы, минуты, секунды
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Градусы, минуты, секунды.
    /// </summary>
    public struct Degree
        : IEquatable<Degree>
    {
        #region Properties

        /// <summary>
        /// Градусы (целая часть).
        /// </summary>
        public int Degrees { get; set; }

        /// <summary>
        /// Минуты (целая часть).
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Секунды (целая и дробная части).
        /// </summary>
        public float Seconds { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Degree
            (
                double degrees
            )
            : this()
        {
            var minus = degrees < 0;

            degrees = Math.Abs (degrees);
            var wholeDegrees = Math.Truncate (degrees);
            Degrees = (int) wholeDegrees;
            if (minus)
            {
                Degrees = -Degrees;
            }

            var minutes = (degrees - wholeDegrees) * 60.0;
            Minutes = (int) minutes;

            Seconds = (float) ((minutes - Minutes) * 60.0);

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Degree
            (
                int degrees,
                int minutes,
                float seconds
            )
        {
            Degrees = degrees;
            Minutes = minutes;
            Seconds = seconds;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Перевод из радиан в градусы.
        /// </summary>
        public static Degree FromRadians (double radians) => new (radians * 180.0 / Math.PI);

        /// <summary>
        /// Преобразование в градусы без минут.
        /// </summary>
        public double ToDouble()
        {
            var minus = Degrees < 0;

            var result = Math.Abs (Degrees) + Minutes / 60.0 + Seconds / 3600.0;
            if (minus)
            {
                result = -result;
            }

            return result;

        } // method ToDouble

        /// <summary>
        /// Перевод в радианы.
        /// </summary>
        public double ToRadians() => ToDouble() / 180.0 * Math.PI;

        /// <summary>
        /// Сложение.
        /// </summary>
        public static Degree operator + (Degree left, Degree right) =>
            new (left.ToDouble() + right.ToDouble());

        /// <summary>
        /// Сложение.
        /// </summary>
        public static Degree operator + (Degree left, double right) =>
            new (left.ToDouble() + right);

        /// <summary>
        /// Вычитание.
        /// </summary>
        public static Degree operator - (Degree left, Degree right) =>
            new (left.ToDouble() - right.ToDouble());

        /// <summary>
        /// Вычитание.
        /// </summary>
        public static Degree operator - (Degree left, double right) =>
            new (left.ToDouble() - right);

        /// <summary>
        /// Умножение.
        /// </summary>
        public static Degree operator * (Degree left, double right) =>
            new (left.ToDouble() * right);

        /// <summary>
        /// Деление.
        /// </summary>
        public static Degree operator / (Degree left, double right) =>
            new (left.ToDouble() / right);

        /// <summary>
        /// Равенство.
        /// </summary>
        public static bool operator == (Degree left, Degree right) =>
            left.ToDouble() == right.ToDouble();

        /// <summary>
        /// Равенство.
        /// </summary>
        public static bool operator == (Degree left, double right) =>
            left.ToDouble() == right;

        /// <summary>
        /// Неравенство.
        /// </summary>
        public static bool operator != (Degree left, Degree right) =>
            left.ToDouble() != right.ToDouble();

        /// <summary>
        /// Неравенство.
        /// </summary>
        public static bool operator != (Degree left, double right) =>
            left.ToDouble() != right;

        /// <summary>
        /// Меньше.
        /// </summary>
        public static bool operator < (Degree left, Degree right) =>
            left.ToDouble() < right.ToDouble();

        /// <summary>
        /// Меньше.
        /// </summary>
        public static bool operator < (Degree left, double right) =>
            left.ToDouble() < right;

        /// <summary>
        /// Меньше или равно.
        /// </summary>
        public static bool operator <= (Degree left, Degree right) =>
            left.ToDouble() <= right.ToDouble();

        /// <summary>
        /// Меньше или равно.
        /// </summary>
        public static bool operator <= (Degree left, double right) =>
            left.ToDouble() <= right;

        /// <summary>
        /// Больше.
        /// </summary>
        public static bool operator > (Degree left, Degree right) =>
            left.ToDouble() > right.ToDouble();

        /// <summary>
        /// Больше.
        /// </summary>
        public static bool operator > (Degree left, double right) =>
            left.ToDouble() > right;

        /// <summary>
        /// Больше или равно.
        /// </summary>
        public static bool operator >= (Degree left, Degree right) =>
            left.ToDouble() >= right.ToDouble();

        /// <summary>
        /// Больше или равно.
        /// </summary>
        public static bool operator >= (Degree left, double right) =>
            left.ToDouble() >= right;

        #endregion

        #region Object members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals (Degree other) => ToDouble() == other.ToDouble();

        /// <inheritdoc cref="ValueType.Equals(object)"/>
        [ExcludeFromCodeCoverage]
        public override bool Equals (object? obj) => obj is Degree other && Equals (other);

        /// <inheritdoc cref="ValueType.GetHashCode"/>
        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => HashCode.Combine (Degrees, Minutes, Seconds);

        /// <inheritdoc cref="ValueType.ToString"/>
        public override string ToString() => string.Format
            (
                CultureInfo.InvariantCulture,
                "{0}\u00B0 {1}' {2:F2}\"",
                Degrees,
                Minutes,
                Seconds
            );

        #endregion

    } // struct Degree

} // namespace AM
