// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Microsoft.DotNet.PlatformAbstractions;

#endregion

#nullable enable

namespace AM.HtmlTags;

internal class MemberExpressionCacheKeyComparer : IEqualityComparer<MemberExpressionCacheKey>
{
    public static readonly MemberExpressionCacheKeyComparer Instance = new ();

    public bool Equals (MemberExpressionCacheKey x, MemberExpressionCacheKey y)
    {
        if (x.ModelType != y.ModelType)
        {
            return false;
        }

        var xEnumerator = x.GetEnumerator();
        var yEnumerator = y.GetEnumerator();

        while (xEnumerator.MoveNext())
        {
            if (!yEnumerator.MoveNext())
            {
                return false;
            }

            // Current is a MemberInfo instance which has a good comparer.
            if (xEnumerator.Current != yEnumerator.Current)
            {
                return false;
            }
        }

        return !yEnumerator.MoveNext();
    }

    public int GetHashCode (MemberExpressionCacheKey obj)
    {
        var hashCodeCombiner = new HashCodeCombiner();
        hashCodeCombiner.Add (obj.ModelType);

        foreach (var member in obj)
        {
            hashCodeCombiner.Add (member);
        }

        return hashCodeCombiner.CombinedHash;
    }
}
