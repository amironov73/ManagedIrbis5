// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BarsikCommand.cs -- работа со скриптами Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Работа со скриптами Барсика.
/// </summary>
public sealed class BarsikCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BarsikCommand()
        : base ("barsik")
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    internal static string? FindOnPath
        (
            string name,
            params string[] directories
        )
    {
        foreach (var directory in directories)
        {
            var candidate = Path.Combine (directory, name);
            if (File.Exists (candidate))
            {
                return candidate;
            }

            candidate = Path.Combine (directory, name + ".barsik");
            if (File.Exists (candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    internal static string? FindScript
        (
            string? name
        )
    {
        if (string.IsNullOrEmpty (name))
        {
            return name;
        }

        if (Path.IsPathRooted (name))
        {
            if (File.Exists (name))
            {
                return name;
            }

            name += ".barsik";
            if (File.Exists (name))
            {
                return name;
            }

            return null;
        }

        return FindOnPath (name, ".", "scripts");
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

        var fileName = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        var interpreter = executive.Interpreter;
        if (string.IsNullOrEmpty (fileName))
        {
            executive.WriteError ("Barsik script name required");
        }
        else
        {
            try
            {
                var foundScript = FindScript (fileName);
                if (string.IsNullOrEmpty (foundScript))
                {
                    executive.WriteError ($"Can't find script '{fileName}'");
                    return false;
                }

                var executionResult = interpreter.ExecuteFile (foundScript);
                MxUtility.HandleExecutionResult (executive, executionResult);
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
        return "Execute Barsik script";
    }

    #endregion
}
