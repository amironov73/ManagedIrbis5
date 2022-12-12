// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* LevelBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal sealed class LevelBuilder<TPayload>
{
    #region Nested classes

    private sealed class StackFrame
    {
        /// <summary>
        ///
        /// </summary>
        public Node<TPayload>? Node;

        /// <summary>
        ///
        /// </summary>
        public IEnumerator<KeyValuePair<char, Node<TPayload>>>? ChildIterator;

        /// <summary>
        ///
        /// </summary>
        public int Level;
    }

    #endregion

    #region Contstruction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LevelBuilder
        (
            IEqualityComparer<TPayload>? comparer = null
        )
    {
        _comparer = new LevelBuilderEqualityComparer<TPayload>
            (
                comparer ?? EqualityComparer<TPayload>.Default
            );
    }

    #endregion

    #region Private members

    private readonly LevelBuilderEqualityComparer<TPayload> _comparer;

    private Dictionary<Node<TPayload>, Node<TPayload>> NewLevel() => new (_comparer);

    private static void Push
        (
            Stack<StackFrame> stack, Node<TPayload> node
        )
    {
        stack.Push (new StackFrame { Node = node, ChildIterator = node.Children.ToList().GetEnumerator() });
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public void MergeEnds
        (
            Node<TPayload> root
        )
    {
        Sure.NotNull (root);

        var levels = new[] { NewLevel() }.ToList();
        var stack = new Stack<StackFrame>();
        Push (stack, root);

        while (stack.Count > 0)
        {
            if (stack.Peek().ChildIterator!.MoveNext())
            {
                // depth first
                Push (stack, stack.Peek().ChildIterator!.Current.Value);
                continue;
            }

            var current = stack.Pop();

            if (stack.Count == 0)
            {
                continue;
            }

            var parent = stack.Peek();

            if (levels.Count <= current.Level)
            {
                levels.Add (NewLevel());
            }

            var level = levels[current.Level];
            var currentNode = current.Node;

            if (level.TryGetValue (currentNode!, out var existing))
            {
                parent.Node!.Children[parent.ChildIterator!.Current.Key] = existing;
            }
            else
            {
                level.Add (currentNode!, currentNode!);
            }

            var parentLevel = current.Level + 1;

            if (parent.Level < parentLevel)
            {
                parent.Level = parentLevel;
            }
        }
    }
}
