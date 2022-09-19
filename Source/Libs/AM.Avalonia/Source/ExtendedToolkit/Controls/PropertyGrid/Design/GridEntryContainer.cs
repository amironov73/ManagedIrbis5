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

using System;

using Avalonia.Controls;
using Avalonia.ExtendedToolkit.Controls.PropertyGrid.Utils;
using Avalonia.Markup.Xaml.Templates;

#endregion

#nullable enable

namespace Avalonia.ExtendedToolkit.Controls.PropertyGrid.Design;

//
// ported from https://github.com/DenisVuyka/WPG
//

/// <summary>
/// Specifies a UI container for <see cref="GridEntry"/>
/// </summary>
public abstract class GridEntryContainer
    : ContentControl
{
    /// <summary>
    /// style key of this control
    /// </summary>
    public Type StyleKey => typeof (GridEntryContainer);

    /// <summary>
    /// Gets or sets the resource locator.
    /// </summary>
    /// <value>The resource locator.</value>
    protected ResourceLocator ResourceLocator { get; set; } = new ();

    /// <summary>
    /// ParentContainer AttachedProperty
    /// </summary>
    public static readonly AttachedProperty<GridEntryContainer> ParentContainerProperty =
        AvaloniaProperty.RegisterAttached<GridEntryContainer, Control, GridEntryContainer> ("ParentContainer");

    /// <summary>
    /// get ParentContainer
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static GridEntryContainer GetParentContainer (Control element)
    {
        return element.GetValue (ParentContainerProperty);
    }

    /// <summary>
    /// set ParentContainer
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetParentContainer (Control element, GridEntryContainer value)
    {
        element.SetValue (ParentContainerProperty, value);
    }

    /// <summary>
    /// get/set gridentry
    /// </summary>
    public GridEntry? Entry
    {
        get => GetValue (EntryProperty);
        set => SetValue (EntryProperty, value);
    }

    /// <summary>
    /// <see cref="Entry"/>
    /// </summary>
    public static readonly StyledProperty<GridEntry?> EntryProperty =
        AvaloniaProperty.Register<GridEntryContainer, GridEntry?> (nameof (Entry));

    /// <summary>
    /// Gets the editor template to present contained entry.
    /// </summary>
    /// <value>The editor template to present contained entry.</value>
    public DataTemplate? EditorTemplate => FindEditorTemplate();

    /// <summary>
    /// Finds the editor template.
    /// </summary>
    /// <returns>DataTemplate the Editor should be applied.</returns>
    protected virtual DataTemplate? FindEditorTemplate()
    {
        var editor = Entry?.Editor;

        if (editor == null)
        {
            return null;
        }

        if (editor.InlineTemplate is DataTemplate template)
        {
            return template;
        }

        return ResourceLocator.GetResource (editor.InlineTemplate) as DataTemplate;
    }

    /// <inheritdoc cref="StyledElement.OnDataContextChanged"/>
    protected override void OnDataContextChanged
        (
            EventArgs eventArgs
        )
    {
        base.OnDataContextChanged (eventArgs);

        if (DataContext is GridEntry)
        {
            Entry = DataContext as GridEntry;
        }
    }
}
