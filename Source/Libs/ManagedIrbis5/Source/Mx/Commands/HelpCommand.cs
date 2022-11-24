// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HelpCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

using System.Linq;

using AM;
using AM.Reflection;

namespace ManagedIrbis.Mx.Commands;

/// <summary>
///
/// </summary>
public sealed class HelpCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public HelpCommand()
        : base ("help")
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

        var commandName = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        if (string.IsNullOrEmpty (commandName))
        {
            var items = executive.Commands
                .Select (x => new NameValue<string?>
                    (
                        x.Name,
                        x.GetShortHelp()
                    ))
                .OrderBy (x => x.Name)
                .ToArray();

            var tablefier = new Tablefier();
            var output = tablefier.Print (items, "Name", "Value");
            executive.WriteOutput (output);
        }
        else
        {
            var command = executive.GetCommand (commandName);
            if (ReferenceEquals (command, null))
            {
                executive.WriteError ($"Unknown command '{commandName}'");
            }
            else
            {
                executive.WriteMessage
                    (
                        command.GetLongHelp()
                    );
            }
        }

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string? GetShortHelp()
    {
        return "Get some help from the interpreter";
    }

    #endregion
}
