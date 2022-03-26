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

/* MsBoxCustomViewModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Avalonia.DTO;
using AM.Avalonia.Models;
using AM.Avalonia.Views;

#endregion

#nullable enable

namespace AM.Avalonia.ViewModels;

/// <summary>
///
/// </summary>
public class MsBoxCustomViewModel
    : AbstractMsBoxViewModel
{
    private readonly MsBoxCustomWindow _window;

    /// <summary>
    ///
    /// </summary>
    public MsBoxCustomViewModel
        (
            MsCustomParams parameters,
            MsBoxCustomWindow msBoxCustomWindow
        )
        : base (parameters, parameters.Icon, parameters.BitmapIcon)
    {
        _window = msBoxCustomWindow;
        ButtonDefinitions = parameters.ButtonDefinitions;
    }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<ButtonDefinition> ButtonDefinitions { get; }

    /// <summary>
    ///
    /// </summary>
    public void ButtonClick
        (
            string parameter
        )
    {
        foreach (var bd in ButtonDefinitions)
        {
            if (!parameter.Equals (bd.Name)) continue;
            _window.ButtonResult = bd.Name;
            break;
        }

        _window.Close();
    }
}
