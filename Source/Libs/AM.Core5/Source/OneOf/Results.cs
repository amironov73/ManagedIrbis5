// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Results.cs -- предопределенные результаты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Results;

//
// ПРЕДОПРЕДЕЛЕННЫЕ РЕЗУЛЬТАТЫ
//

/// <summary>
/// Результат: да.
/// </summary>
[PublicAPI]
public readonly struct Yes
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Yes Value;
}

/// <summary>
/// Результат: да плюс значение.
/// </summary>
[PublicAPI]
public readonly struct Yes<T>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Yes (T value) => Value = value;

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public T Value { get; }
}

/// <summary>
/// Результат: нет.
/// </summary>
[PublicAPI]
public readonly struct No
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly No Value;
}

/// <summary>
/// Результат: может быть.
/// </summary>
[PublicAPI]
public readonly struct Maybe
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Maybe Value;
}

/// <summary>
/// Результат: неизвестно.
/// </summary>
[PublicAPI]
public readonly struct Unknown
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Unknown Value;
}

/// <summary>
/// Результат: истина.
/// </summary>
[PublicAPI]
public readonly struct True
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly True Value;
}

/// <summary>
/// Результат: ложь.
/// </summary>
[PublicAPI]
public readonly struct False
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly False Value;
}

/// <summary>
/// Результат: все.
/// </summary>
[PublicAPI]
public readonly struct All
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly All Value;
}

/// <summary>
/// Результат: некоторые.
/// </summary>
[PublicAPI]
public readonly struct Some
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Some Value;
}

/// <summary>
/// Результат: достигнут конец.
/// </summary>
[PublicAPI]
public readonly struct End
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly End Value;
}

/// <summary>
/// Результат: пропуск.
/// </summary>
[PublicAPI]
public readonly struct Skip
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Skip Value;
}

/// <summary>
/// Результат: ни одного.
/// </summary>
[PublicAPI]
public readonly struct None
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly None Value;
}

/// <summary>
/// Результат: не найдено.
/// </summary>
[PublicAPI]
public readonly struct NotFound
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly NotFound Value;
}

/// <summary>
/// Результат: успех.
/// </summary>
[PublicAPI]
public readonly struct Success
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Success Value;
}

/// <summary>
/// Результат: успех плюс значение.
/// </summary>
[PublicAPI]
public readonly struct Success<T>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Success (T value) => Value = value;

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public T Value { get; }
}

/// <summary>
/// Результат плюс значение либо сообщением об ощибке.
/// </summary>
[PublicAPI]
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
    /// Общее значение для сбоя.
    /// </summary>
    public static readonly Result<T> Failure = Fail();

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
    public static Result<T> Fail
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

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString() => IsSuccess
        ? Convert.ToString (Value) ?? "(empty)"
        : "FAILURE";

    #endregion
}

/// <summary>
/// Результат: ошибка.
/// </summary>
[PublicAPI]
public readonly struct Error
{
    /// <summary>
    /// Общее значение.
    /// </summary>
    public static readonly Error Value;
}

/// <summary>
/// Результат: ошибка плюс значение.
/// </summary>
public readonly struct Error<T>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Error (T value) => Value = value;

    /// <summary>
    /// Хранимое значение.
    /// </summary>
    public T Value { get; }
}
