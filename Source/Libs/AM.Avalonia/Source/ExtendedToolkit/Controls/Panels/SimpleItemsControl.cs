// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

using AM;

using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

//ported from https://github.com/HandyOrg/HandyControl.git

/// <summary>
/// 'itemscontrol' which use a panel to add it's children
/// </summary>
public class SimpleItemsControl
    : TemplatedControl
{
    private const string ElementPanel = "PART_Panel";

    /// <summary>
    /// Gets or sets ItemTemplate.
    /// </summary>
    [Bindable (true)]
    public IDataTemplate ItemTemplate
    {
        get => GetValue (ItemTemplateProperty);
        set => SetValue (ItemTemplateProperty, value);
    }

    /// <summary>
    /// Defines the ItemTemplate property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
        AvaloniaProperty.Register<SimpleItemsControl, IDataTemplate> (nameof (ItemTemplate));

    /// <summary>
    /// Gets or sets ItemsSource.
    /// </summary>
    public IEnumerable ItemsSource
    {
        get => GetValue (ItemsSourceProperty);
        set => SetValue (ItemsSourceProperty, value);
    }

    /// <summary>
    /// Defines the ItemsSource property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<SimpleItemsControl, IEnumerable> (nameof (ItemsSource));

    /// <summary>
    /// Gets or sets HasItems.
    /// </summary>
    public bool HasItems
    {
        get => GetValue (HasItemsProperty);
        private set => SetValue (HasItemsProperty, value);
    }

    /// <summary>
    /// Defines the HasItems property.
    /// </summary>
    public static readonly StyledProperty<bool> HasItemsProperty =
        AvaloniaProperty.Register<SimpleItemsControl, bool> (nameof (HasItems));

    /// <summary>
    ///
    /// </summary>
    public IEnumerable Items { get; internal set; }

    internal IPanel? ItemsHost { get; set; }

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SimpleItemsControl()
    {
        ItemTemplateProperty.Changed.AddClassHandler<SimpleItemsControl> (OnItemTemplateChanged);
        ItemsSourceProperty.Changed.AddClassHandler<SimpleItemsControl> (OnItemsSourceChanged);

        var items = new AvaloniaList<object>();
        items.CollectionChanged += (s, e) =>
        {
            if (e.NewItems is { Count: not 0 })
            {
                SetValue (HasItemsProperty, true);
            }

            OnItemsChanged (s, e);
        };
        Items = items;
    }

    #endregion

    private void OnItemsChanged
        (
            object? sender,
            NotifyCollectionChangedEventArgs eventArgs
        )
    {
        sender.NotUsed();
        eventArgs.NotUsed();

        Refresh();
        UpdateItems();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void Refresh()
    {
        if (ItemsHost == null)
        {
            return;
        }

        ItemsHost.Children.Clear();
        foreach (var item in Items)
        {
            if (item is TemplatedControl element)
            {
                //element.Style = ItemContainerStyle;
                ItemsHost.Children.Add (element);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void UpdateItems()
    {
        // пустое тело метода
    }

    private void OnItemsSourceChanged
        (
            SimpleItemsControl o,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        o.OnItemsSourceChanged
            (
                (IEnumerable?)eventArgs.OldValue,
                (IEnumerable?)eventArgs.NewValue
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnItemsSourceChanged
        (
            IEnumerable? oldValue,
            IEnumerable? newValue
        )
    {
    }

    /// <inheritdoc cref="TemplatedControl.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        ItemsHost?.Children.Clear();
        base.OnApplyTemplate (eventArgs);

        ItemsHost = eventArgs.NameScope.Find<IPanel> (ElementPanel);
        Refresh();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="o"></param>
    /// <param name="eventArgs"></param>
    protected virtual void OnItemTemplateChanged
        (
            SimpleItemsControl o,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        Refresh();
    }
}
