// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ValueWrapper.cs -- обертка над захваченной скриптом переменной
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Обертка над захваченной скриптом переменной.
/// </summary>
public sealed class ValueWrapper<T>
    : IValueWrapper
{
    #region Delegates

    /// <summary>
    /// Получение значения переменной.
    /// </summary>
    public Func<T> GetValue { get; }

    /// <summary>
    /// Установка значения переменной.
    /// </summary>
    public Action<T>? SetValue { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ValueWrapper
        (
            Func<T> getValue,
            Action<T>? setValue = null
        )
    {
        Sure.NotNull (getValue);

        GetValue = getValue;
        SetValue = setValue;
    }

    #endregion

    #region IValueWrapper members

    /// <inheritdoc cref="IValueWrapper.GetValue"/>
    object? IValueWrapper.GetValue()
    {
        return GetValue.Invoke();
    }

    /// <inheritdoc cref="IValueWrapper.SetValue"/>
    void IValueWrapper.SetValue
        (
            object? value
        )
    {
        if (SetValue is null)
        {
            throw new InvalidOperationException ("Can't set value");
        }

        SetValue.Invoke ((T) value!);
    }

    #endregion
}
