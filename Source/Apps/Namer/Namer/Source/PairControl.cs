// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PairControl.cs -- контрол для отображения пар имен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using NamerCommon;

#endregion

#nullable enable

namespace Namer;

/// <summary>
/// Контрол для отображения пар имен.
/// </summary>
public class PairControl
    : ReactiveUserControl<NamePair>
{
    #region Construciton
    
    public PairControl()
    {
        var checkBox = new CheckBox
            {
                Height = 10,
                [!ToggleButton.IsCheckedProperty] = new Binding (nameof (NamePair.IsChecked))
            }
            .DockLeft();

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            ColumnDefinitions = new ColumnDefinitions("Auto,*,*,*"),

            Children =
            {
                checkBox,

                new TextBlock
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        [!TextBlock.TextProperty] = new Binding (nameof (NamePair.Old))
                    }
                    .SetColumn (1),

                new TextBlock
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        [!TextBlock.TextProperty] = new Binding (nameof (NamePair.New))
                    }
                    .SetColumn (2),
                
                new TextBlock
                    {
                        Foreground = Brushes.Red,
                        VerticalAlignment = VerticalAlignment.Center,
                        [!TextBlock.TextProperty] = new Binding (nameof (NamePair.ErrorMessage))
                    }
                    .SetColumn (3)
            }
        };
    }
    
    #endregion
}
