// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* WinFormsModule.cs -- Barsik-модуль для WinForms
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM.Scripting.Barsik;
using AM.Windows.Forms;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace AM.Scripting.WinForms;

/// <summary>
/// Barsik-модуль для WinForms.
/// </summary>
public sealed class WinFormsModule
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "alert", new FunctionDescriptor ("alert", Alert) },
        { "confirm", new FunctionDescriptor ("confirm", Confirm) },
        { "prompt", new FunctionDescriptor ("prompt", Prompt) },
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Выдача простого сообщения.
    /// </summary>
    public static dynamic? Alert
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        // MessageBox.Show (text, "Barsik", MessageBoxButtons.OK);

        var icon = Resources.Barsik;
        TaskDialog.ShowDialog (new TaskDialogPage()
        {
            Caption = "Barsik interpreter",
            Text = text,
            AllowCancel = true,
            Icon = new TaskDialogIcon (icon),
            Buttons =
            {
                TaskDialogButton.OK
            }
        });

        return null;
    }

    /// <summary>
    /// Простейшее подтверждение действия у пользователя.
    /// </summary>
    public static dynamic? Confirm
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);

        var icon = Resources.Barsik;
        var result = TaskDialog.ShowDialog (new TaskDialogPage()
        {
            Caption = "Barsik interpreter",
            Text = text,
            AllowCancel = true,
            Icon = new TaskDialogIcon (icon),
            Buttons =
            {
                TaskDialogButton.OK,
                TaskDialogButton.Cancel
            }
        });

        return result == TaskDialogButton.OK;
    }

    /// <summary>
    /// Простейший ввод строки.
    /// </summary>
    public static dynamic? Prompt
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return null;
        }

        var text = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (text))
        {
            return null;
        }

        var defaultValue = Compute (context, args, 1) as string ?? string.Empty;
        var result = defaultValue;
        if (InputBox.Query ("Barsik interpreter", text, ref result)
            != DialogResult.OK)
        {
            result = null;
        }

        return result;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "WinForms";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
