// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SimpleStatement.cs -- простой стейтмент
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Kotik.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Простой стейтмент.
/// </summary>
internal sealed class SimpleStatement
    : StatementBase
{
    #region Properties

    /// <summary>
    /// Выражение.
    /// </summary>
    public AtomNode Expression { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SimpleStatement
        (
            int line,
            AtomNode expression
        )
        : base (line)
    {
        Expression = expression;
    }

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        Sure.NotNull (context);

        base.Execute (context);

        try
        {
            Expression.Compute (context);
        }
        catch (Exception exception)
        {
            if (!exception.Data.Contains (KotikUtility.BarsikInternals))
            {
                Magna.Logger.LogError
                    (
                        exception,
                        "Error at line {Line}",
                        Line
                    );

                context.Commmon.Error?.WriteLine
                    (
                        $"Error {exception.Message} at line {Line}"
                    );
            }

            throw;
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

       Expression.DumpHierarchyItem ("Expression", level + 1, writer);
    }

    #endregion

}
