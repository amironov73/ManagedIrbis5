// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IPoolingNode.cs -- интерфейс контейнера для хранения элемента коллекции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Интерфейс контейнера для хранения элемента коллекции.
/// </summary>
/// <typeparam name="T">Тип хранимого элемента.</typeparam>
public interface IPoolingNode<T>
    : IDisposable
{
    /// <summary>
    /// Контейнер со следующим элементом.
    /// </summary>
    public IPoolingNode<T>? Next { get; set; }

    /// <summary>
    /// Обращение по индексу.
    /// </summary>
    T this [int index] { get; set; }

    /// <summary>
    /// Инициализация.
    /// </summary>
    IPoolingNode<T> Init (int capacity);

    /// <summary>
    /// Очистка.
    /// </summary>
    void Clear();
}
