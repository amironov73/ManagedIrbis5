// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* HistoryCommand.cs -- история поиска, возвращение к предыдущему поиску
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// История поиска. Возвращение к предыдущему поиску.
/// </summary>
public sealed class HistoryCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HistoryCommand()
        : base ("history")
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

        var command = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        var history = executive.History.ToArray();

        if (string.IsNullOrEmpty (command))
        {
            for (var i = 0; i < history.Length; i++)
            {
                executive.WriteOutput ($"{i + 1}: {history[i]}");
            }
        }
        else
        {
            var invariant = CultureInfo.InvariantCulture;
            if (int.TryParse (command, invariant, out var index))
            {
                var argument = history.GetOccurrence
                    (
                        history.Length - index
                    );
                if (string.IsNullOrEmpty (argument))
                {
                    executive.WriteError ("No such entry");
                }
                else
                {
                    var searchCommand = executive.GetCommand<SearchCommand>();
                    executive.WriteLine (argument);
                    var newArguments = new[]
                    {
                        new MxArgument { Text = argument }
                    };
                    searchCommand.Execute (executive, newArguments);
                    executive.History.Pop();
                }
            }
        }

        OnAfterExecute();

        return true;
    }

    #endregion
}
