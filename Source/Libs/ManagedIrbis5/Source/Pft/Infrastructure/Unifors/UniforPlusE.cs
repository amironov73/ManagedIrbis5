// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusE.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Возвращает порядковый номер заданного поля в записи – &uf('+E
    // Вид функции: +E.
    // Назначение: Возвращает порядковый номер заданного поля в записи.
    // При отсутствии в записи заданного поля возвращается пустота.
    // Присутствует в версиях ИРБИС с 2009.1.
    // Формат (передаваемая строка):
    // +Etag#occ
    // где:
    // tag – метка поля;
    // occ – номер повторения поля(по умолчанию – 1).
    // Может принимать значение* – это означает номер текущего
    // повторения в повторяющейся группе.
    //

    static class UniforPlusE
    {
        #region Public methods

        /// <summary>
        /// Get field index.
        /// </summary>
        public static void GetFieldIndex
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null)
                && !string.IsNullOrEmpty(expression))
            {
                var parts = expression.Split('#');
                var tag = parts[0].SafeToInt32();
                var occText = parts.Length > 1
                    ? parts[1]
                    : "1";
                int occ;
                if (occText == "*")
                {
                    occ = context.Index;
                }
                else if (occText == string.Empty)
                {
                    occ = 0;
                }
                else
                {
                    if (!Utility.TryParseInt32(occText, out occ))
                    {
                        return;
                    }
                    occ--;
                }

                var field = record.Fields.GetField(tag, occ);
                if (!ReferenceEquals(field, null))
                {
                    var index = record.Fields.IndexOf(field) + 1;
                    var output = index.ToInvariantString();
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
