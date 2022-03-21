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

/* RelayCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Input;

#endregion

#nullable enable

namespace AM.Avalonia.ViewModels.Commands;

public class RelayCommand : ICommand
{
    private readonly Func<object, bool> canExecute;
    private readonly Action<object> execute;
    private EventHandler? _canExecuteChanged;

    public RelayCommand (Action<object> execute, Func<object, bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => _canExecuteChanged += value;
        remove => _canExecuteChanged -= value;
    }

    public bool CanExecute (object parameter)
    {
        return this.canExecute == null || this.canExecute (parameter);
    }

    public void Execute (object parameter)
    {
        this.execute (parameter);
    }
}
