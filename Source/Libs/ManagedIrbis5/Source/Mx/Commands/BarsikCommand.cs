// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BarsikCommand.cs -- работа со скриптами Барсика
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
                interpreter.ExecuteFile (fileName);
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
    public override string? GetShortHelp()
    {
        return "Execute Barsik script";
    }

    #endregion
}
