// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PocketCommand.cs -- работа с карманом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Работа с карманом.
/// </summary>
public sealed class PocketCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PocketCommand()
        : base ("pocket")
    {
        // пустое тело конструктора
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute" />
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        string? argument = null;
        if (arguments.Length != 0)
        {
            argument = arguments[0].Text.SafeTrim().EmptyToNull();
        }

        if (!string.IsNullOrEmpty (argument))
        {
            argument = argument.ToLowerInvariant();
        }

        OnAfterExecute();
        switch (argument)
        {
            case "add":
                break;

            case "clear":
                break;

            case "exclude":
                break;

            case "export":
                break;

            case "format":
                break;

            case "load":
                break;

            case "save":
                break;

            case null:
                break;
        }


        return true;
    }

    #endregion
}
