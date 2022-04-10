// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

/* Node.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Collections.TernarySearchTree;

internal sealed class Node<TValue>
{
    private TValue value;

    public Node(char splitCharacter)
    {
        SplitCharacter = splitCharacter;
    }

    public char SplitCharacter { get; private set; }

    public Node<TValue> HigherNode { get; set; }

    public Node<TValue> EqualNode { get; set; }

    public Node<TValue> LowerNode { get; set; }

    public bool HasValue { get; private set; }

    public TValue Value
    {
        get
        {
            return value;
        }

        set
        {
            this.value = value;
            HasValue = true;
        }
    }

    internal void ClearValue()
    {
        value = default(TValue);
        HasValue = false;
    }

    public bool CanBeRemoved => HigherNode == null && LowerNode == null && EqualNode == null && HasValue == false;

    public bool CanBeSimplified => EqualNode == null && HasValue == false && (LowerNode == null) != (HigherNode == null);
}
