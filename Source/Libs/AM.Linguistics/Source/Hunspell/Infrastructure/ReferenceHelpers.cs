// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure
{
    internal static class ReferenceHelpers
    {
#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public static void Swap<T> (ref T a, ref T b)
        {
            var tmp = a;
            a = b;
            b = tmp;
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public static T Steal<T> (ref T item) where T : class
        {
            var value = item;
            item = null;
            return value;
        }
    }
}
