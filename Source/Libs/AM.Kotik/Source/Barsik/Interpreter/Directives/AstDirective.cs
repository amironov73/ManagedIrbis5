// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AstDirective.cs -- переключение флага "Dump AST"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Directives;

/// <summary>
/// Директива: переключение флага "Dump AST".
/// </summary>
public sealed class AstDirective
    : DirectiveBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstDirective()
        : base ("ast")
    {
        // пустое тело метода
    }

    #endregion

    #region DirectiveBase members

    /// <inheritdoc cref="DirectiveBase.Execute"/>
    public override void Execute
        (
            Context context,
            string? argument
        )
    {
        var interpreter = context.Interpreter;
        if (interpreter is not null)
        {
            var flag = !interpreter.Settings.DumpAst;
            var onoff = flag ? "on" : "off";
            interpreter.Settings.DumpAst = flag;
            context.Commmon.Output?.WriteLine ($"Dump AST is {onoff} now");
        }
    }

    #endregion
}
