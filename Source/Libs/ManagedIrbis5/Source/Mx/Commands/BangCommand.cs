// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BangCommand.cs -- однократное выполнение барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Scripting.Barsik;

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Команда однократного выполнения барсика.
/// </summary>
public sealed class BangCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BangCommand()
        : base ("!")
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

        var source = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        var interpreter = executive.Interpreter;
        if (string.IsNullOrEmpty (source))
        {
            new Repl(interpreter).Loop();
        }
        else
        {
            try
            {
                interpreter.Execute (source);
            }
            catch (Exception exception)
            {
                executive.WriteError (exception.ToString());
            }
        }

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp"/>
    public override string GetShortHelp()
    {
        return "Execute Barsik one-liner";
    }

    #endregion
}
