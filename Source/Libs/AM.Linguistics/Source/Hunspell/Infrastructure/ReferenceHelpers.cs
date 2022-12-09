// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ReferenceHelpers.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure
{
    internal static class ReferenceHelpers
    {
        public static void Swap<T> (ref T a, ref T b)
        {
            (a, b) = (b, a);
        }

        public static T Steal<T> (ref T item) where T : class
        {
            var value = item;
            item = default!;
            return value;
        }
    }
}
