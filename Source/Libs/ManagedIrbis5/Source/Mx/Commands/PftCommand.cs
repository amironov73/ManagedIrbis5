// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
///
/// </summary>
public sealed class PftCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftCommand()
        : base ("pft")
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

        var provider = executive.Provider;
        if (!provider.IsConnected)
        {
            executive.WriteError ("Not connected");

            return false;
        }

        var source = arguments.FirstOrDefault()
            ?.Text.SafeTrim().EmptyToNull();
        if (!string.IsNullOrEmpty (source))
        {
            var text = executive.FormatRemote (source);
            executive.WriteLine (text);
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
