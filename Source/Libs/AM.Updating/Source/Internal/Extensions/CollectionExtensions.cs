// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* CollectionExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Updating.Internal.Extensions;

internal static class CollectionExtensions
{
    public static int AddRange<T> (this HashSet<T> hashSet, IEnumerable<T> sequence) =>
        sequence.Count (hashSet.Add);
}
