// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AliasCommand.cs -- создание, удаление и прочая работа с алиасам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Создание, удаление и прочая работа работа с алиасами.
/// </summary>
public sealed class AliasCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AliasCommand()
        : base ("alias")
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private void ListAliases
        (
            MxExecutive executive
        )
    {
        var aliases = executive.Aliases;
        var keys = aliases.Keys.Order();
        foreach (var alias in keys)
        {
            executive.WriteLine ($"{alias} => {aliases[alias]}");
        }
    }

    private void ProcessOneArgument
        (
            MxExecutive executive,
            MxArgument argument
        )
    {
        var text = argument.Text;
        if (string.IsNullOrWhiteSpace (text))
        {
            return;
        }

        var aliases = executive.Aliases;
        var parts = text.Split ('=', 2);
        if (parts.Length == 1)
        {
            if (!aliases.ContainsKey (text))
            {
                executive.WriteLine ("No such alias");
            }
            else
            {
                executive.WriteLine ($"{text} => {aliases[text]}");
            }
        }
        else
        {
            var key = parts[0].Trim();
            if (string.IsNullOrWhiteSpace (key))
            {
                return;
            }

            var value = parts[1].Trim();
            if (string.IsNullOrEmpty (value))
            {
                aliases.Remove (key);
            }
            else
            {
                aliases[key] = value;
            }
        }
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc cref="MxCommand.Execute"/>
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        if (arguments.IsNullOrEmpty())
        {
            ListAliases (executive);
        }
        else
        {
            foreach (var argument in arguments)
            {
                ProcessOneArgument (executive, argument);
            }
        }

        OnAfterExecute();

        return true;
    }

    /// <inheritdoc cref="MxCommand.GetShortHelp" />
    public override string GetShortHelp()
    {
        return "Create or delete aliases";
    }

    #endregion
}
