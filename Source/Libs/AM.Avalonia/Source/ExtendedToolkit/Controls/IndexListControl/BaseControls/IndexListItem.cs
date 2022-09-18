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
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls;

/// <summary>
/// selectable contencontrol
/// </summary>
public class IndexListItem : ContentControl, ISelectable
{
    private IControl? _header;

    /// <summary>
    /// Defines the <see cref="IsSelected"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsSelectedProperty =
        ListBoxItem.IsSelectedProperty.AddOwner<IndexListItem>();

    /// <summary>
    /// Gets or sets the selection state of the item.
    /// </summary>
    public bool IsSelected
    {
        get => GetValue (IsSelectedProperty);
        set => SetValue (IsSelectedProperty, value);
    }

    /// <summary>
    /// overrides some default values
    /// </summary>
    static IndexListItem()
    {
        SelectableMixin.Attach<IndexListItem> (IsSelectedProperty);
        FocusableProperty.OverrideDefaultValue<IndexListItem> (true);
        RequestBringIntoViewEvent.AddClassHandler<IndexListItem> ((x, e) => x.OnRequestBringIntoView (e));
        DataContextProperty.Changed.AddClassHandler<IndexListItem> (OnDataContextChanged);
    }

    private static void OnDataContextChanged (IndexListItem indexListItem, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is IndexItemModel model)
        {
            var binding = new Binding
            {
                Source = model,
                Path = nameof (model.IsVisible),
                Mode = BindingMode.TwoWay
            };

            indexListItem.Bind (IsVisibleProperty, binding);
        }
    }

    /// <summary>
    /// tries to bring the content to the view
    /// </summary>
    /// <param name="e"></param>
    protected virtual void OnRequestBringIntoView (RequestBringIntoViewEventArgs e)
    {
        if (e.TargetObject == this && _header != null)
        {
            var m = _header.TransformToVisual (this);

            if (m.HasValue)
            {
                var bounds = new Rect (_header.Bounds.Size);
                var rect = bounds.TransformToAABB (m.Value);
                e.TargetRect = rect;
            }
        }
    }

    /// <inheritdoc cref="TemplatedControl.OnApplyTemplate"/>
    protected override void OnApplyTemplate
        (
            TemplateAppliedEventArgs eventArgs
        )
    {
        base.OnApplyTemplate (eventArgs);
        _header = eventArgs.NameScope.Find<IControl> ("PART_Header");
    }
}
