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

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Results;

//
// ПРЕДОПРЕДЕЛЕННЫЕ РЕЗУЛЬТАТЫ
//

/// <summary>
/// Результат: да.
/// </summary>
public readonly struct Yes { }

/// <summary>
/// Результат: да со значением.
/// </summary>
public readonly struct Yes<T>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Yes (T value)
    {
        Value = value;
    }

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public T Value { get; }
}

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
/// Результат: достигнут конец.
/// </summary>
public readonly struct End { }

/// <summary>
/// Результат: пропуск.
/// </summary>
public readonly struct Skip { }

/// <summary>
/// Результат: ни одного.
/// </summary>
public readonly struct None
{
    /// <summary>
    /// Конструирование значения.
    /// </summary>
    public static OneOf<T, None> Of<T>(T t) => new None();
}

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
}

/// <summary>
/// Результат со значением.
/// </summary>
public readonly struct Result<T>
{
    #region Properties

    /// <summary>
    /// Признак успешного завершения.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public T Value
    {
        get
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор для случая успеха.
    /// </summary>
    public Result
        (
            T value
        )
    {
        IsSuccess = true;
        _value = value;
    }

    #endregion

    #region Private members

    private readonly T _value;

    #endregion

    #region Public methods

    /// <summary>
    /// Сбой.
    /// </summary>
    public static Result<T> Failure
        (
            string? message = null
        )
    {
        return new Result<T>
        {
            Message = message
        };
    }

    #endregion
}

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
}
