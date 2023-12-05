// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* CaptionControl.cs -- контрол для отображения подписей к картинкам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

namespace EasyCaption;

/// <summary>
/// Контрол для отображения подписей к картинкам.
/// </summary>
public sealed class CaptionControl
    : UserControl
{
    #region Construction

    public CaptionControl()
    {
        var image = new Image
        {
            [Grid.ColumnProperty] = 0,
            [Grid.RowProperty] = 0,
            [Grid.RowSpanProperty] = 2,
            Margin = new Thickness (3),
            [!Image.SourceProperty] = new Binding (nameof (Caption.Thumbnail)),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var shortName = new Label
        {
            [Grid.ColumnProperty] = 1,
            [Grid.RowProperty] = 0,
            FontWeight = FontWeight.Bold,
            [!ContentProperty] = new Binding (nameof (Caption.ShortName))
        };

        var prompt = new Label
        {
            [Grid.ColumnProperty] = 1,
            [Grid.RowProperty] = 1,
            [!ContentProperty] = new Binding (nameof (Caption.Text))
        };

        Content = new Grid
        {
            ColumnDefinitions = ColumnDefinitions.Parse ("50,*"),
            RowDefinitions = RowDefinitions.Parse ("Auto,*"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                image,
                shortName,
                prompt
            }
        };
    }

    #endregion
}
