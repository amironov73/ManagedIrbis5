// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Results.cs -- предопределенные результаты
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Results
{
    //
    // ПРЕДОПРЕДЕЛЕННЫЕ РЕЗУЛЬТАТЫ
    //

    /// <summary>
    /// Результат: да.
    /// </summary>
    public readonly struct Yes { }

    /// <summary>
    /// Результат: нет.
    /// </summary>
    public readonly struct No { }

    /// <summary>
    /// Результат: может быть.
    /// </summary>
    public readonly struct Maybe { }

    /// <summary>
    /// Результат: неизвестно.
    /// </summary>
    public readonly struct Unknown { }

    /// <summary>
    /// Результат: истина.
    /// </summary>
    public readonly struct True { }

    /// <summary>
    /// Результат: ложь.
    /// </summary>
    public readonly struct False { }

    /// <summary>
    /// Результат: все.
    /// </summary>
    public readonly struct All { }

    /// <summary>
    /// Результат: некоторые.
    /// </summary>
    public readonly struct Some { }

    /// <summary>
    /// Результат: ни одного.
    /// </summary>
    public readonly struct None
    {
        /// <summary>
        /// Конструирование значения.
        /// </summary>
        public static OneOf<T, None> Of<T>(T t) => new None();
    } // struct None

    /// <summary>
    /// Результат: не найдено.
    /// </summary>
    public readonly struct NotFound { }

    /// <summary>
    /// Результат: успех.
    /// </summary>
    public readonly struct Success { }

    /// <summary>
    /// Результат: успех со значением.
    /// </summary>
    public readonly struct Success<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Success(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Хранимое значение.
        /// </summary>
        public T Value { get; }
    } // struct Success

    /// <summary>
    /// Результат.
    /// </summary>
    public readonly struct Result<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Result(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Хранимое значение.
        /// </summary>
        public T Value { get; }
    } // struct Result

    /// <summary>
    /// Результат: ошибка.
    /// </summary>
    public readonly struct Error { }

    /// <summary>
    /// Результат: ошибка со значением.
    /// </summary>
    public readonly struct Error<T>
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Error(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Хранимое значение.
        /// </summary>
        public T Value { get; }
    } // struct Error

} // namespace AM.Results
