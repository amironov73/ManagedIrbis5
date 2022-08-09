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

/* MultiDawg.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
public sealed class MultiDawg<TPayload>
{
    private readonly TPayload[][] payloads;
    private readonly YaleGraph yaleGraph;

    internal MultiDawg (YaleGraph yaleGraph, TPayload[][] payloads)
    {
        this.yaleGraph = yaleGraph;
        this.payloads = payloads;
    }

    /// <summary>
    /// Tries to find as many space-separated words as it can.
    /// </summary>
    /// <param name="words">The words to find</param>
    /// <param name="wordsFound">How many words were actually matched.</param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public IEnumerable<TPayload> MultiwordFind (
        IEnumerable<IEnumerable<char>> words,
        out int wordsFound,
        char separator = ' ')
    {
        var last_word_end_node_i = -1;
        var last_word_count = 0;
        var node_i = -1;
        var wordCount = 0;

        // ReSharper disable AccessToModifiedClosure
        IEnumerable<char> GetChars()
        {
            foreach (var word in words)
            {
                foreach (var c in word)
                {
                    yield return c;
                }

                ++wordCount;

                if (HasPayload (node_i))
                {
                    last_word_end_node_i = node_i;
                    last_word_count = wordCount;
                }

                if (yaleGraph.IsLeaf (node_i))
                {
                    break;
                }

                yield return separator;
            }
        }

        // ReSharper restore AccessToModifiedClosure

        foreach (var i in yaleGraph.GetPath (GetChars()))
        {
            node_i = i;
        }

        wordsFound = last_word_count;

        return GetPayloads (last_word_end_node_i);
    }

    public IEnumerable<TPayload> this [IEnumerable<char> key] =>
        GetPayloads (yaleGraph.GetPath (key).Last());

    private IEnumerable<TPayload> GetPayloads (int i)
    {
        if (i == -1)
        {
            yield break;
        }

        foreach (var arr in payloads)
        {
            if (arr.Length <= i)
            {
                break;
            }

            yield return arr[i];
        }
    }

    public IEnumerable<KeyValuePair<string, IEnumerable<TPayload>>> MatchPrefix (IEnumerable<char> prefix)
    {
        var prefixStr = prefix.AsString();

        var sb = new StringBuilder (prefixStr);

        foreach (var node_i in yaleGraph.MatchPrefix (sb, yaleGraph.GetPath (prefixStr).Last()))
        {
            if (HasPayload (node_i))
            {
                yield return new KeyValuePair<string, IEnumerable<TPayload>> (sb.ToString(), GetPayloads (node_i));
            }
        }
    }

    private bool HasPayload (int node_i) => payloads.Length > 0 && node_i < payloads[0].Length;

    public int GetNodeCount() => yaleGraph.NodeCount;

    public int MaxPayloads => payloads.Length;

    public IEnumerable<KeyValuePair<string, IEnumerable<TPayload>>> MatchTree (IEnumerable<IEnumerable<char>> tree)
    {
        return yaleGraph.MatchTree (tree)
            .Select (pair => new KeyValuePair<string, IEnumerable<TPayload>> (pair.Key, GetPayloads (pair.Value)));
    }
}
