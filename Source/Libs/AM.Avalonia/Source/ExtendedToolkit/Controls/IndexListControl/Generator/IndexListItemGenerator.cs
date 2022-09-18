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

using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Data;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

/// <summary>
/// generator for indexlist items
/// partial taken from <see cref="TreeItemContainerGenerator{T}"/>
/// </summary>
public class IndexListItemGenerator
    : ItemContainerGenerator<IndexListHeaderItem>

{
    /// <summary>
    /// Gets the item container's Items property.
    /// </summary>
    protected AvaloniaProperty ItemsProperty { get; }

    /// <summary>
    /// sets the itemsproperty parameter
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="contentProperty"></param>
    /// <param name="contentTemplateProperty"></param>
    /// <param name="itemsProperty"></param>
    public IndexListItemGenerator
        (
            IControl owner,
            AvaloniaProperty contentProperty,
            AvaloniaProperty contentTemplateProperty,
            AvaloniaProperty itemsProperty
        )
        : base (owner, contentProperty, contentTemplateProperty)
    {
        ItemsProperty = itemsProperty;
    }

    /// <inheritdoc cref="ItemContainerGenerator{T}.CreateContainer"/>
    protected override IControl? CreateContainer
        (
            object? item
        )
    {
        if (item is null)
        {
            return null;
        }

        if (item is IndexListHeaderItem container)
        {
            return container;
        }

        var owner = Owner as IndexList;

        var template = GetTreeDataTemplate (item, ItemTemplate);
        var result = new IndexListHeaderItem();

        result.SetValue (ContentProperty, template.Build (item), BindingPriority.Style);
        var binding = new Binding
        {
            Source = owner,
            Path = nameof (owner.ShowEmptyItems)
        };
        result.Bind (IndexListHeaderItem.ShowEmptyItemsProperty, binding, BindingPriority.Style);

        var itemsSelector = template.ItemsSelector (item);

        if (itemsSelector != null)
        {
            BindingOperations.Apply (result, ItemsProperty, itemsSelector, null);
        }

        if (item is not IControl)
        {
            result.DataContext = item;
        }

        return result;
    }

    private class WrapperTreeDataTemplate
        : ITreeDataTemplate
    {
        private readonly IDataTemplate _inner;

        public WrapperTreeDataTemplate (IDataTemplate inner) => _inner = inner;

        public IControl Build (object param) => _inner.Build (param);

        //public bool SupportsRecycling => _inner.SupportsRecycling;

        public bool Match (object data) => _inner.Match (data);

        public InstancedBinding? ItemsSelector (object item) => null;
    }

    private ITreeDataTemplate GetTreeDataTemplate
        (
            object item,
            IDataTemplate primary
        )
    {
        var template = Owner.FindDataTemplate (item, primary) ?? FuncDataTemplate.Default;
        var treeTemplate = template as ITreeDataTemplate ?? new WrapperTreeDataTemplate (template);
        return treeTemplate;
    }
}
