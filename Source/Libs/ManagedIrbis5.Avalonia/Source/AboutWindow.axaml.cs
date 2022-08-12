// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

/* AboutWindow.cs -- окно "О продукте"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace ManagedIrbis.Avalonia;

/// <summary>
/// Окно "О продукте".
/// </summary>
public partial class AboutWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Наименование продукта
    /// </summary>
    [Reactive]
    public string? ProductTitle { get; set; }

    /// <summary>
    /// Версия продукта.
    /// </summary>
    [Reactive]
    public string? ProductVersion { get; set; }

    /// <summary>
    /// Коммит Git.
    /// </summary>
    [Reactive]
    public string? GitCommit { get; set; }

    /// <summary>
    /// Дата коммита.
    /// </summary>
    [Reactive]
    public string? ProductDate { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public AboutWindow()
    {
        AvaloniaXamlLoader.Load (this);

        ProductTitle = "Проект ARS MAGNA";
        ProductVersion = ThisAssembly.AssemblyFileVersion;
        GitCommit = ThisAssembly.GitCommitId;
        ProductDate = ThisAssembly.GitCommitDate.ToString (CultureInfo.CurrentUICulture);

        DataContext = this;

#if DEBUG
        this.AttachDevTools();
#endif
    }

    #endregion

    #region Private members

    private void OkButton_OnClick (object? sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion
}

