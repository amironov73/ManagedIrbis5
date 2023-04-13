// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DirectiveNode.cs -- обработка директивы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Обработка директивы.
/// </summary>
public sealed class DirectiveNode
    : PseudoNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DirectiveNode
        (
            int line,
            string command,
            string? argument
        )
        : base (line)
    {
        _command = command;
        _argument = argument;
    }

    #endregion

    #region Private members

    private readonly string _command;
    private readonly string? _argument;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        base.Execute (context);

        var interpreter = context.Interpreter.ThrowIfNull();
        var success = false;
        foreach (var directive in interpreter.Settings.KnownDirectives)
        {
            if (directive.Recognize (_command, _argument))
            {
                directive.Execute (context, _argument);
                success = true;
            }
        }

        if (!success)
        {
            context.Commmon.Error?.WriteLine ($"Unknown directive {_command}");
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        DumpHierarchyItem ("Command", level + 1, writer, _command);
        DumpHierarchyItem ("Argument", level + 1, writer, _argument);
    }

    #endregion
}
