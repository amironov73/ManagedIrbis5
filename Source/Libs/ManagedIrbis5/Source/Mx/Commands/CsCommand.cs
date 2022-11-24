// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CsCommand.cs -- выполнение однострочиника C#
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using ManagedIrbis.Scripting.Sharping;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands;

/// <summary>
/// Выполнение однострочного скрипта на C#.
/// </summary>
public sealed class CsCommand
    : MxCommand
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CsCommand()
        : base ("cs")
    {
        // пустое тело конструктора
    }

    #endregion

    #region MxCommand members

    /// <inheritdoc/>
    public override bool Execute
        (
            MxExecutive executive,
            MxArgument[] arguments
        )
    {
        OnBeforeExecute();

        var source = arguments.FirstOrDefault()?.Text
            .SafeTrim().EmptyToNull();

        if (!string.IsNullOrEmpty (source))
        {
            try
            {
                var compiler = new ScriptCompiler();
                var options = compiler.ParseArguments (Array.Empty<string>());
                var compilation = compiler.Compile (options);
                var assembly = compiler.EmitAssemblyToMemory (compilation);
                compiler.RunAssembly (assembly, Array.Empty<string>());
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
        return "Execute C# one-liner";
    }

    #endregion
}
