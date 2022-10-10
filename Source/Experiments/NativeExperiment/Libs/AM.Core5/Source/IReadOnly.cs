// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IReadOnly -- общий интерфейс для объектов, поддерживающих состояние "только для чтения"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM;

/// <summary>
/// Общий интерфейс для объектов, поддерживающих состояние "только для чтения".
/// </summary>
public interface IReadOnly<out T>
{
    /// <summary>
    /// Получение копии объекта в состоянии "только для чтения".
    /// </summary>
    T AsReadOnly();

    /// <summary>
    /// В настоящее время объект находится в состоянии "только для чтения"?
    /// </summary>
    bool ReadOnly { get; }

    /// <summary>
    /// Установка состояния "только для чтения".
    /// Отмена этого состояния не предусмотрена.
    /// </summary>
    void SetReadOnly();

    /// <summary>
    /// Выброс искоючения <see cref="ReadOnlyException"/>, если объект
    /// находится в состоянии "только для чтения".
    /// </summary>
    void ThrowIfReadOnly();
}
