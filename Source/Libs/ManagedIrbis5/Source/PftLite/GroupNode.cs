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
using System.IO;
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

        var count = 0;
        foreach (var item in Items)
        {
            if (item is FieldNode field)
            {
                count = Math.Max (count, field.Prepare (context));

            }
        }

        if (count == 0)
        {
            context.CurrentGroup = null;
            return;
        }

        context.RepeatCount = count;
        for (context.CurrentRepeat = 0;
             context.CurrentRepeat < context.RepeatCount;
             context.CurrentRepeat++
            )
        {
            foreach (var node in Items)
            {
                node.Execute (context);
            }
        }

        context.CurrentGroup = null;
    }

    #endregion

    #region MereSerializer members

    /// <inheritdoc cref="PftNode.MereSerialize"/>
    public override void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
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
