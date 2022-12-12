// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* DisposableCollection.cs -- коллекция, состоящая из disposable-элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Коллекция, состоящая из <see cref="IDisposable"/> элементов.
/// Принимает в том числе <c>null</c>, это не приводит к ошибке.
/// </summary>
[DebuggerDisplay ("Count = {" + nameof (Count) + "}")]
public class DisposableCollection<T>
    : Collection<T?>,
    IDisposable
    where T : IDisposable
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DisposableCollection()
    {
        // пустое тело метода
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DisposableCollection
        (
            IList<T?> list
        )
        : base (list)
    {
        // пустое тело метода
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DisposableCollection
        (
            IEnumerable<T?> list
        )
    {
        Sure.NotNull (list);

        foreach (var disposable in list)
        {
            Add (disposable);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление нескольких элементов.
    /// </summary>
    public void AddRange
        (
            IEnumerable<T?> list
        )
    {
        Sure.NotNull (list);

        foreach (var disposable in list)
        {
            Add (disposable);
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        for (var i = 0; i < Count; i++)
        {
            var item = this[i];
            item?.Dispose();
        }
    }

    #endregion
}
