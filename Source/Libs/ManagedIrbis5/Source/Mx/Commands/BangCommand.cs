// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BangCommand.cs -- однократное выполнение барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Scripting.Barsik;

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
        var foundScript = BarsikCommand.FindScript (source);
        if (!string.IsNullOrEmpty (foundScript))
        {
            new BarsikCommand()
                .Execute
                    (
                        executive,
                        new [] { new MxArgument { Text = source } }
                    );
        }
        else
        {
            var repl = new Repl (interpreter);
            if (string.IsNullOrEmpty (source))
            {
                var executionResult = repl.Loop();
                MxUtility.HandleExecutionResult (executive, executionResult);
            }
            else
            {
                try
                {
                    if (repl.Evaluate (source, out var result))
                    {
                        repl.Context.Variables["$$"] = result;
                        if (repl.Echo)
                        {
                            BarsikUtility.PrintObject (repl.Output, result);
                            repl.Output.WriteLine();
                        }
                    }
                    else
                    {
                        repl.Output.ResetCounter();
                        repl.Execute (source);
                        if (repl.Output.Counter != 0)
                        {
                            repl.Output.WriteLine();
                        }
                    }
                }
                catch (Exception exception)
                {
                    executive.WriteError (exception.ToString());
                }
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
