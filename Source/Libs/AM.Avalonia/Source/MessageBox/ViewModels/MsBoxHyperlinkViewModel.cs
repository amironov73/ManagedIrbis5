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

/* MsBoxHyperlinkViewModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Avalonia.DTO;
using AM.Avalonia.Enums;
using AM.Avalonia.Models;
using AM.Avalonia.Views;

#endregion

#nullable enable

namespace AM.Avalonia.ViewModels;

/// <summary>
///
/// </summary>
public class MsBoxHyperlinkViewModel
    : AbstractMsBoxViewModel
{
    private readonly MsBoxHyperlinkWindow _window;

    /// <summary>
    ///
    /// </summary>
    public MsBoxHyperlinkViewModel
        (
            MessageBoxHyperlinkParams parameters,
            MsBoxHyperlinkWindow msBoxHyperlinkWindow
        )
        : base (parameters, parameters.Icon)
    {
        _window = msBoxHyperlinkWindow;
        HyperlinkContentProvider = parameters.HyperlinkContentProvider;
        SetButtons (parameters.ButtonDefinitions);
    }

    /// <summary>
    ///
    /// </summary>
    public bool IsOkShowed { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsYesShowed { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsNoShowed { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsAbortShowed { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsCancelShowed { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<HyperlinkContent> HyperlinkContentProvider { get; set; }

    private void SetButtons (ButtonEnum paramsButtonDefinitions)
    {
        switch (paramsButtonDefinitions)
        {
            case ButtonEnum.Ok:
                IsOkShowed = true;
                break;

            case ButtonEnum.YesNo:
                IsYesShowed = true;
                IsNoShowed = true;
                break;

            case ButtonEnum.OkCancel:
                IsOkShowed = true;
                IsCancelShowed = true;
                break;

            case ButtonEnum.OkAbort:
                IsOkShowed = true;
                IsAbortShowed = true;
                break;

            case ButtonEnum.YesNoCancel:
                IsYesShowed = true;
                IsNoShowed = true;
                IsCancelShowed = true;
                break;

            case ButtonEnum.YesNoAbort:
                IsYesShowed = true;
                IsNoShowed = true;
                IsAbortShowed = true;
                break;

            default:
                throw new ArgumentOutOfRangeException (nameof (paramsButtonDefinitions), paramsButtonDefinitions,
                    null);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="parameter"></param>
    public void ButtonClick
        (
            string parameter
        )
    {
        _window.ButtonResult = (ButtonResult)Enum.Parse (typeof (ButtonResult), parameter.Trim(), false);
        _window.Close();
    }
}
