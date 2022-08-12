// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* FieldEditorWindow.cs -- окно редактора полей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using AM;

#endregion

#nullable enable

using ManagedIrbis.Workspace;

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно редактора полей.
/// </summary>
public partial class FieldEditorWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FieldEditorWindow()
    {
        AvaloniaXamlLoader.Load (this);
    }

    #endregion

    #region Private members

    private IEnumerable<FieldLine>? _lines;

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public void SetLines
        (
            IEnumerable<FieldLine> lines
        )
    {
        Sure.NotNull ((object?) lines);

        FieldGrid.Items = _lines = lines;
    }

    /// <summary>
    ///
    /// </summary>
    public  IEnumerable<FieldLine> GetLines()
    {
        return _lines.ThrowIfNull();
    }

    /// <summary>
    ///
    /// </summary>
    public void SetHint
        (
            string? hint
        )
    {
        HintBox.Content = hint;
    }

    #endregion
}
