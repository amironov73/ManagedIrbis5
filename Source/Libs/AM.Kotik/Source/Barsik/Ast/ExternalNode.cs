// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExternalNode.cs -- выполнение внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Выполнение некоторого внешнего по отношению к Барсику кода.
/// Полагается на глобальный обработчик, заданный в контексте
/// интерпретатора.
/// </summary>
public sealed class ExternalNode
    : StatementBase
{
    #region Properties

    /// <summary>
    /// Произвольный код, понятный обработчику внешнего кода.
    /// </summary>
    public string? Code { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExternalNode
        (
            int line,
            string? code
        )
        : base (line)
    {
        Code = code;
    }

    #endregion

    #region StatementNode example

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var handler = context.Interpreter?.ExternalCodeHandler;
        if (handler is null)
        {
            context.Error?.WriteLine ("Missing external code handler");
            return;
        }

        try
        {
            handler (context, this);
        }
        catch (Exception exception)
        {
            context.Error?.WriteLine (exception.Message);
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
        
        DumpHierarchyItem ("Code", level + 1, writer, Code);
    }

    #endregion
}
