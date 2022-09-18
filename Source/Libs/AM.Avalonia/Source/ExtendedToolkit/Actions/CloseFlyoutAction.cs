// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.ExtendedToolkit.Controls;
using Avalonia.ExtendedToolkit.TriggerExtensions;
using Avalonia.VisualTree;

#endregion

namespace Avalonia.ExtendedToolkit.Actions;

//ported from https://github.com/MahApps/MahApps.Metro

/// <summary>
/// command action which closes
/// the <see cref="Flyout"/>
/// </summary>
public class CloseFlyoutAction
    : CommandTriggerAction
{
    private Flyout? associatedFlyout;

    private Flyout AssociatedFlyout => associatedFlyout ??= AssociatedObject.GetVisualParent<Flyout>();

    /// <inheritdoc cref="CommandTriggerAction.Invoke"/>
    protected override void Invoke
        (
            object parameter
        )
    {
        if (AssociatedObject == null || (AssociatedObject != null && !AssociatedObject.IsEnabled))
        {
            return;
        }

        var command = Command;
        if (command != null)
        {
            var commandParameter = GetCommandParameter();
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
        else
        {
            AssociatedFlyout?.SetValue(Flyout.IsOpenProperty, false);
        }
    }

    /// <inheritdoc cref="CommandTriggerAction.GetCommandParameter"/>
    protected override object GetCommandParameter()
    {
        return CommandParameter ?? AssociatedFlyout;
    }
}
