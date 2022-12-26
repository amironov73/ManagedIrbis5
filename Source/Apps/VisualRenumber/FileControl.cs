// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FileControl.cs -- контрол для отображения перенумеруемого файла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

#endregion

#nullable enable

namespace VisualRenumber;

public class FileControl
    : ReactiveUserControl<FileItem>
{
    public FileControl()
    {
        var checkBox = new CheckBox
        {
            Height = 10,
            [!ToggleButton.IsCheckedProperty] = new Binding (nameof (FileItem.IsChecked))
        }
        .DockLeft();
        checkBox.PropertyChanged += CheckBox_Checked;

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            ColumnDefinitions = new ColumnDefinitions("Auto,*,*"),

            Children =
            {
                checkBox,

                new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding (nameof (FileItem.OldName))
                }
                .SetColumn (1),

                new TextBlock
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    [!TextBlock.TextProperty] = new Binding (nameof (FileItem.NewName))
                }
                .SetColumn (2)
            }
        };
    }

    #region Private members

    private void CheckBox_Checked
        (
            object? sender,
            AvaloniaPropertyChangedEventArgs eventArgs
        )
    {
        this.GetParentDataContext<FolderModel>()?.SyncRenumber();
    }

    #endregion
}
