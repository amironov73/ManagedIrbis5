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

/* MsBoxStandardViewModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia.Threading;

using AM.Avalonia.DTO;
using AM.Avalonia.Enums;
using AM.Avalonia.ViewModels.Commands;
using AM.Avalonia.Views;

#endregion

#nullable enable

namespace AM.Avalonia.ViewModels;

/// <summary>
///
/// </summary>
public class MsBoxStandardViewModel
    : AbstractMsBoxViewModel
{
    public readonly ClickEnum _enterDefaultButton;
    public readonly ClickEnum _escDefaultButton;

    private readonly MsBoxStandardWindow _window;

    public MsBoxStandardViewModel
        (
            MessageBoxStandardParams parameters,
            MsBoxStandardWindow msBoxStandardWindow
        ) :
        base(parameters, parameters.Icon)
    {
        _enterDefaultButton = parameters.EnterDefaultButton;
        _escDefaultButton = parameters.EscDefaultButton;
        _window = msBoxStandardWindow;
        SetButtons(parameters.ButtonDefinitions);
        ButtonClickCommand = new RelayCommand(o => ButtonClick(o.ToString()));
        EnterClickCommand = new RelayCommand(o => EnterClick());
        EscClickCommand = new RelayCommand(o => EscClick());
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
    public RelayCommand ButtonClickCommand { get; }

    /// <summary>
    ///
    /// </summary>
    public RelayCommand EnterClickCommand { get; }

    /// <summary>
    ///
    /// </summary>
    public RelayCommand EscClickCommand { get; }

    private void SetButtons(ButtonEnum paramsButtonDefinitions)
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
                throw new ArgumentOutOfRangeException
                    (
                        nameof(paramsButtonDefinitions),
                        paramsButtonDefinitions,
                        null
                    );
        }
    }

    private void EscClick()
    {
        switch (_escDefaultButton)
        {
            case ClickEnum.Ok:
                ButtonClick (ButtonResult.Ok);
                return;

            case ClickEnum.Yes:
                ButtonClick (ButtonResult.Yes);
                return;

            case ClickEnum.No:
                ButtonClick (ButtonResult.No);
                return;

            case ClickEnum.Abort:
                ButtonClick (ButtonResult.Abort);
                return;

            case ClickEnum.Cancel:
                ButtonClick (ButtonResult.Cancel);
                return;

            case ClickEnum.None:
                ButtonClick (ButtonResult.None);
                return;

            case ClickEnum.Default:
            {
                if (IsCancelShowed)
                {
                    ButtonClick(ButtonResult.Cancel);
                    return;
                }

                if (IsAbortShowed)
                {
                    ButtonClick(ButtonResult.Abort);
                    return;
                }

                if (IsNoShowed)
                {
                    ButtonClick(ButtonResult.No);
                    return;
                }
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        ButtonClick(ButtonResult.None);
    }

    private void EnterClick()
    {
        switch (_enterDefaultButton)
        {
            case ClickEnum.Ok:
                ButtonClick (ButtonResult.Ok);
                return;

            case ClickEnum.Yes:
                ButtonClick (ButtonResult.Yes);
                return;

            case ClickEnum.No:
                ButtonClick (ButtonResult.No);
                return;

            case ClickEnum.Abort:
                ButtonClick (ButtonResult.Abort);
                return;

            case ClickEnum.Cancel:
                ButtonClick (ButtonResult.Cancel);
                return;

            case ClickEnum.None:
                ButtonClick (ButtonResult.None);
                return;

            case ClickEnum.Default:
            {
                if (IsOkShowed)
                {
                    ButtonClick(ButtonResult.Ok);
                    return;
                }

                if (IsYesShowed)
                {
                    ButtonClick(ButtonResult.Yes);
                    return;
                }
            }
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async void ButtonClick
        (
            string parameter
        )
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _window.ButtonResult = (ButtonResult)Enum.Parse(typeof(ButtonResult), parameter.Trim(), true);
            _window.Close();
        });
    }

    /// <summary>
    ///
    /// </summary>
    public async void ButtonClick
        (
            ButtonResult buttonResult
        )
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            _window.ButtonResult = buttonResult;
            _window.Close();
        });
    }
}
