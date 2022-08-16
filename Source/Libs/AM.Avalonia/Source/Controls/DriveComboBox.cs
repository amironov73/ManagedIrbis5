// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* DriveComboBox.cs -- комбобокс, отображающий список дисков
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Avalonia.Converters;

using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Styling;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Комбобокс, отображающий список дисков.
/// </summary>
public sealed class DriveComboBox
    : ComboBox, IStyleable
{
    #region Properties

    /// <summary>
    /// Выбранный диск.
    /// </summary>
    public DriveInfo? SelectedDrive => SelectedItem as DriveInfo;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DriveComboBox()
    {
        ItemTemplate = new FuncDataTemplate<DriveInfo> ((value, namescope) =>
            new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Children =
                {
                    new Label
                    {
                        Width = 50,
                        [!ContentControl.ContentProperty] = new Binding ("Name")
                    },
                    new Label
                    {
                        Width = 100,
                        [!ContentControl.ContentProperty] = new Binding ("VolumeLabel")
                    },
                    new Label
                    {
                        [!ContentControl.ContentProperty] = new Binding ("TotalSize")
                        {
                            Converter = GigaConverter.Instance
                        }
                    }
                }
            });
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Обновление списка дисков.
    /// </summary>
    public void RefreshDriveList()
    {
        try
        {
            Items = DriveInfo.GetDrives();
        }
        catch
        {
            Items = null;
        }
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="SelectingItemsControl.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        RefreshDriveList();
    }

    #endregion

    #region IStyleable members

    Type IStyleable.StyleKey => typeof (ComboBox);

    #endregion
}
