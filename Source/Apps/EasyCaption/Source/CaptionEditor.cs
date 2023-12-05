// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* CaptionEditor.cs -- редактор подписей к картинкам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

namespace EasyCaption;

/// <summary>
/// Редактор подписей к картинкам.
/// </summary>
public sealed class CaptionEditor
    : UserControl
{
    #region Properties

    /// <summary>
    /// Просмотрщик картинки.
    /// </summary>
    public Image Viewer { get; }

    /// <summary>
    /// Собственно редактор для текста подписи.
    /// </summary>
    public TextBox Editor { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaptionEditor()
    {
        Viewer = new Image
        {
            [Grid.RowProperty] = 0,
            [!Image.SourceProperty] = new Binding (nameof (Caption.Thumbnail)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        var splitter = new GridSplitter
        {
            [Grid.RowProperty] = 1,
            Background = Brushes.Black,
            ResizeDirection = GridResizeDirection.Rows
        };

        Editor = new TextBox
        {
            [Grid.RowProperty] = 2,
            [!TextBox.TextProperty] = new Binding (nameof (Caption.Text)),
            TextWrapping = TextWrapping.Wrap,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        Content = new Grid
        {
            RowDefinitions = RowDefinitions.Parse ("*, 4, 200"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                Viewer,
                splitter,
                Editor
            }
        };
    }

    #endregion
}
