// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* MsBoxInputWindow.xaml.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Markup.Xaml;

using AM.Avalonia.BaseWindows.Base;
using AM.Avalonia.DTO;

#endregion

namespace AM.Avalonia.Views;

/// <summary>
///
/// </summary>
public class MsBoxInputWindow
    : BaseWindow, IWindowGetResult<MessageWindowResultDTO>
{
    /// <summary>
    ///
    /// </summary>
    public MsBoxInputWindow() : base()
    {
        InitializeComponent();
    }

    public string ButtonResult { get; set; } = null;
    public string MessageResult { get; set; } = null;

    public MessageWindowResultDTO GetResult() => new MessageWindowResultDTO(MessageResult, ButtonResult);


    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
