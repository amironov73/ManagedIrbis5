// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* TreeStringSerializationExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
///
/// </summary>
public static class TreeStringSerializationExtensions
{
    /// <summary>
    /// Convert node sequence to string
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="sequence">Input node sequence</param>
    /// <param name="toTextLine">Node to string converter</param>
    /// <param name="indention">Spacing to indent each level of nesting</param>
    /// <returns></returns>
    public static string ToString<TNode>
        (
            this IEnumerable<TNode> sequence,
            Func<TNode, string> toTextLine,
            int indention = 5
        )
        where TNode : TreeNode<TNode>
    {
        Sure.NotNull (sequence);
        Sure.NotNull (toTextLine);
        Sure.NonNegative (indention);

        var builder = StringBuilderPool.Shared.Get();
        sequence.ForEach (n =>
        {
            var renderedIndent = string.Empty.PadRight (n.Level * indention);
            builder.AppendLine ($"{renderedIndent}{toTextLine (n)}");
        });

        return builder.ReturnShared();
    }

    /// <summary>
    /// Convert node sequence to string
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="sequence">Input node sequence</param>
    /// <param name="appendLine">Action defining how node gets appended</param>
    /// <param name="indention">Spacing to indent each level of nesting</param>
    /// <returns></returns>
    public static string ToString<TNode>
        (
            this IEnumerable<TNode> sequence,
            Action<TNode, string, StringBuilder> appendLine,
            int indention = 5
        )
        where TNode : TreeNode<TNode>
    {
        Sure.NotNull (sequence);
        Sure.NotNull (appendLine);
        Sure.NonNegative (indention);

        var builder = StringBuilderPool.Shared.Get();

        sequence.ForEach (n =>
        {
            var renderedIndent = string.Empty.PadRight (n.Level * indention);
            appendLine (n, renderedIndent, builder);
        });

        return builder.ReturnShared();
    }
}
