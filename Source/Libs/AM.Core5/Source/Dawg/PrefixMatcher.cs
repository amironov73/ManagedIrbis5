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

/* PrefixMatcher.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

class PrefixMatcher<TPayload>
{
    readonly StringBuilder sb;

    public PrefixMatcher (StringBuilder sb)
    {
        this.sb = sb;
    }

    public IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix (Node<TPayload> node)
    {
        if (!EqualityComparer<TPayload>.Default.Equals (node.Payload, default))
        {
            yield return new KeyValuePair<string, TPayload> (sb.ToString(), node.Payload);
        }

        foreach (var child in node.Children)
        {
            sb.Append (child.Key);

            foreach (var kvp in MatchPrefix (child.Value))
            {
                yield return kvp;
            }

            --sb.Length;
        }
    }
}
