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

/* Dialog.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM.Avalonia.Buttons;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

using ReactiveUI;

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
///
/// </summary>
public class Dialog
    : ReactiveObject, IDialog
{
    private string title;
    private string description;
    protected CancellationTokenSource? DialogTokenSource;

    public string Title
    {
        get => title;
        set => this.RaiseAndSetIfChanged(ref title, value);
    }

    public string Description
    {
        get => description;
        set => this.RaiseAndSetIfChanged(ref description, value);
    }

    public IButtonCollection Buttons { get; }
    public ICollection<IDialogControl> Controls { get; }

    public virtual async Task<IButton> ShowAsync()
    {
        if (DialogTokenSource?.IsCancellationRequested == false)
            throw new InvalidOperationException("Windows already showed.");

        using (DialogTokenSource = new CancellationTokenSource())
        {
            var dialog = new DialogWindow(this);
            dialog.Closed += (sender, args) => CloseImpl();
            DialogTokenSource.Token.Register(() => Dispatcher.UIThread.InvokeAsync(() => dialog.Close()));

            Window owner = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
                owner = lifetime.Windows.FirstOrDefault(w => w.IsActive);
            if (owner != null)
            {
                await dialog.ShowDialog(owner).ConfigureAwait(true);
            }
            else
            {
                dialog.Show();
                Dispatcher.UIThread.MainLoop(DialogTokenSource.Token);
            }
            return dialog.ResultButton;
        }
    }

    public virtual Task CloseAsync()
    {
        this.CloseImpl();
        return Task.CompletedTask;
    }

    private void CloseImpl()
    {
        if (DialogTokenSource == null)
            return;

        if (!DialogTokenSource.IsCancellationRequested)
            DialogTokenSource.Cancel();
        DialogTokenSource.Dispose();
    }

    public Dialog()
    {
        Buttons = new ButtonCollection();
        Controls = new List<IDialogControl>();
    }
}
