// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* GroupNode.cs -- повторяющая группа узлов.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Повторяющаяся группа узлов.
/// </summary>
sealed class GroupNode
    : PftNode
{
    #region Properties

    /// <summary>
    /// Элементы группы.
    /// </summary>
    public List<PftNode> Items { get; } = new ();

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        context.CurrentGroup = this;

        do
        {
            context.OutputFlag = false;
            foreach (var node in Items)
            {
                node.Execute (context);
            }

        } while (context.OutputFlag);

        context.CurrentGroup = null;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append ("<group<");
        var first = true;
        foreach (var item in Items)
        {
            if (!first)
            {
                builder.Append (", ");
            }

            builder.Append (item);
            first = false;
        }
        builder.Append (">>");

        return builder.ToString();
    }

    #endregion
}
