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

/* Designer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.Controls;
using AM.Avalonia.Dialogs;
using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia;

internal class Designer
{
    public static IDialog Dialog { get; set; }

    private static void InitializeContext()
    {
        var dialog = new Dialog()
        {
            Title = "Title",
            Description = "This              is             toooooo               long                     desc",
            Controls =
            {
                new BoolControl() { Name = "Bool control", Value = true },
                new StringControl() { Name = "I am the string", Value = "Test!" },
                new ProgressControl() { Name = "Loading...", Value = 66, MaxValue = 100 }
            },
        };
        dialog.Buttons.AddOkCancel();
        Dialog = dialog;
    }

    static Designer()
    {
        InitializeContext();
    }
}
