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
using Avalonia.Interactivity;

using AM;
using AM.Avalonia;

using Avalonia;
using Avalonia.Data;
using Avalonia.Layout;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно редактора полей.
/// </summary>
public sealed class FieldEditorWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Сетка полей.
    /// </summary>
    public DataGrid FieldGrid { get; }

    /// <summary>
    /// Подсказка.
    /// </summary>
    public Label HintBox { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public FieldEditorWindow()
    {
        Title = "Field editor";
        Width = MinWidth = 400;
        Height = MinHeight = 300;

        var buttons = new StackPanel
        {
            Spacing = 5,
            Margin = new Thickness (20),
            Orientation = Orientation.Horizontal,
            [DockPanel.DockProperty] = Dock.Bottom,
            HorizontalAlignment = HorizontalAlignment.Right,
            Children =
            {
                new Button { Content = "OK" }
                .OnClick (OkButton_OnClick),

                new Button { Content = "Cancel" }
                .OnClick (CancelButton_OnClick)
            }
        };

        HintBox = new Label
        {
            Margin = new Thickness (5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [DockPanel.DockProperty] = Dock.Bottom
        };

        FieldGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            CanUserReorderColumns = false,
            CanUserSortColumns = false,
            Columns =
            {
                new DataGridTextColumn
                {
                    Header = "Поле",
                    IsReadOnly = true,
                    Binding = new Binding (nameof (FieldLine.Title)),
                    Width = new DataGridLength (1, DataGridLengthUnitType.Star)
                },

                new DataGridTextColumn
                {
                    Header = "Значение",
                    Binding = new Binding (nameof (FieldLine.Data)),
                    Width = new DataGridLength (1, DataGridLengthUnitType.Star)
                }
            }
        };

        Content = new DockPanel
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Children =
            {
                buttons,
                HintBox,
                FieldGrid
            }
        };
    }

    #endregion

    #region Private members

    private IEnumerable<FieldLine>? _lines;

    private void OkButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        sender.NotUsed();
        eventArgs.NotUsed();

        Close (true);
    }

    private void CancelButton_OnClick
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        sender.NotUsed();
        eventArgs.NotUsed();

        Close (false);
    }

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
        Sure.NotNull (lines);

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
