// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IValueWrapper.cs -- интерфейс обертки над захваченной скриптом переменной
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Интерфейс обертки над захваченной скриптом переменной.
/// </summary>
public interface IValueWrapper
{
    /// <summary>
    /// Получение значения переменной.
    /// </summary>
    object? GetValue();

    /// <summary>
    /// Установка значения паременной.
    /// </summary>
    /// <param name="value">Новое значение переменной.</param>
    void SetValue (object? value);
}
