// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusStar.cs -- получение GUID записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Получение GUID записи
    //

    //
    // Встречается в IBIS.FST
    //
    // 1001 0 MHL,if &uf('+*') <> '' then 'GUID=',&uf('+*') fi
    //
    // С помощью v2147483647 получить GUID нельзя
    //

    static class UniforPlusStar
    {
        #region Public methods

        // ================================================================

        public static void GetGuid
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var guid = IrbisGuid.Get(context.Record);
            context.WriteAndSetFlag(node, guid);
        }

        // ================================================================

        #endregion
    }
}
