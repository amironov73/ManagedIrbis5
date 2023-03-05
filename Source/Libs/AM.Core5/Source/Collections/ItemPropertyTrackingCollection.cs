// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ItemPropertyTrackingCollection.cs -- коллекция, умеющая отслеживать изменение свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Коллекция, умеющая отслеживать изменение свойств
/// отдельых элементов, помещенных в нее.
/// </summary>
[PublicAPI]
public class ItemPropertyTrackingCollection<TItem>
    : ObservableCollection<TItem>
    where TItem: INotifyPropertyChanged
{
    #region Events

    /// <summary>
    /// Событие, возникающее при изменении любого
    /// (отслеживаемого) свойства у элемента коллекции.
    /// </summary>
    public event PropertyChangedEventHandler? ItemPropertyChanged;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ItemPropertyTrackingCollection()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ItemPropertyTrackingCollection
        (
            IEnumerable<TItem> collection
        ) 
        : base (collection)
    {
        // пустое тело конструктор
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ItemPropertyTrackingCollection
        (
            List<TItem> list
        )
        : base (list)
    {
        // пустое тело конструктора
    }

    #endregion

    #region ObservableCollection<TItem> members

    /// <inheritdoc cref="ObservableCollection{T}.OnCollectionChanged"/>
    protected override void OnCollectionChanged
        (
            NotifyCollectionChangedEventArgs eventArgs
        )
    {
        base.OnCollectionChanged (eventArgs);

        if (eventArgs.OldItems is not null)
        {
            foreach (INotifyPropertyChanged oldItem in eventArgs.OldItems)
            {
                oldItem.PropertyChanged -= _ItemPropertyChanged;
            }
        }

        if (eventArgs.NewItems is not null)
        {
            foreach (INotifyPropertyChanged newItem in eventArgs.NewItems)
            {
                newItem.PropertyChanged += _ItemPropertyChanged;
            }
        }
    }

    #endregion

    #region Private members

    private void _ItemPropertyChanged
        (
            object? sender,
            PropertyChangedEventArgs eventArgs
        )
    {
        ItemPropertyChanged?.Invoke (sender, eventArgs);
    }

    #endregion
}
