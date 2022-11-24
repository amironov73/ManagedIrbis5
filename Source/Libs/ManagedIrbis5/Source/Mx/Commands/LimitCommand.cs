// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LimitCommand.cs -- ограничение количества найденных записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Установка ограничения количества найденных записей.
/// </summary>
public sealed class LimitCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LimitCommand()
        : base ("limit")
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

        if (arguments.Length != 0)
        {
            var argument = arguments[0].Text;
            if (!string.IsNullOrEmpty (argument))
            {
                var invariant = CultureInfo.InvariantCulture;
                if (!int.TryParse (argument, invariant, out var newLimit))
                {
                    executive.WriteError ("bad integer format");
                }

                executive.Limit = newLimit;
                executive.WriteMessage ($"Limit changed to {executive.Limit}");
            }
        }
        else
        {
            executive.WriteMessage ($"Limit is: {executive.Limit}");
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
