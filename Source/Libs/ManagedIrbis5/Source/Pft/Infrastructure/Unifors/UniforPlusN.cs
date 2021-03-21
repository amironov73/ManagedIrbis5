// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusN.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать количество повторений поля – &uf('+N')
    // Вид функции: +N.
    // Назначение: Выдать количество повторений поля,
    // метка которого указана после идентификатора функции.
    // Формат (передаваемая строка): +N
    //
    // Пример:
    //
    // &unifor('+N910')
    //

    static class UniforPlusN
    {
        #region Public methods

        /// <summary>
        /// Get field count.
        /// </summary>
        public static void GetFieldCount
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var tag = expression.SafeToInt32();
            var count = 0;
            var record = context.Record;

            if (!ReferenceEquals(record, null)
                && tag > 0)
            {
                count = record.Fields.GetFieldCount(tag);
            }

            var output = count.ToInvariantString();
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
