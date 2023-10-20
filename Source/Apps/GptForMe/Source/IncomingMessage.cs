// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IncomingMessage.cs -- входящее сообщение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

namespace GptForMe;

/// <summary>
/// Входящее сообщение.
/// </summary>
internal sealed class IncomingMessage
    : UserControl, IChatMessage
{
    #region Properties

    /// <inheritdoc cref="IChatMessage.MessageContent"/>
    public string? MessageContent { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public IncomingMessage()
    {
        DataContext = this;
    }

    #endregion

    #region UserControl members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Content = new Grid
        {
            Margin = new Thickness (5),
            HorizontalAlignment  = HorizontalAlignment.Stretch,
            ColumnDefinitions = new ColumnDefinitions("5*,*"),
            Children =
            {
                new Border
                {
                    Padding = new Thickness (5),
                    BorderBrush = Brushes.Blue,
                    BorderThickness = new Thickness (1),
                    CornerRadius = new CornerRadius (5, 5, 5, 0),
                    BoxShadow = new BoxShadows (new BoxShadow
                    {
                        Color = Colors.DarkBlue,
                        OffsetX = 3.0,
                        OffsetY = 3.0,
                        Spread = 3,
                    }),
                    Background = Brushes.LightBlue,
                    Child = new TextBlock
                    {
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.Wrap,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        [!TextBlock.TextProperty] = new Binding (nameof (MessageContent))
                    }
                },

                new Label
                {
                    [Grid.ColumnProperty] = 1
                }
            }
        };
    }

    #endregion
}
