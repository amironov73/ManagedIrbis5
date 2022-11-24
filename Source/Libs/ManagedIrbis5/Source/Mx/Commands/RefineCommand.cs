// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RefineCommand.cs -- уточнение предыдущего поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Уточнение предыдущего поиска.
/// </summary>
public sealed class RefineCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RefineCommand()
        : base ("refine")
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

        var history = executive.History;
        var previous = history.Count == 0
            ? null
            : history.Peek();
        var expression = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        if (!string.IsNullOrEmpty (previous)
            && !string.IsNullOrEmpty (expression))
        {
            expression = $"({previous}) * ({expression})";
            var searchCommand = new SearchCommand();
            var newArguments = new[]
            {
                new MxArgument { Text = expression }
            };
            searchCommand.Execute (executive, newArguments);
        }

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string GetShortHelp()
    {
        return "Refine the last search";
    }

    #endregion
}
