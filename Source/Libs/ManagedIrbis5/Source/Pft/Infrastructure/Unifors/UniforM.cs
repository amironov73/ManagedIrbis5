// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforM.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Отсортировать повторения заданного поля – &uf('M')
    // Вид функции: M.
    // Назначение: Отсортировать повторения заданного поля
    // (имеется в виду строковая сортировка) – функция ничего
    // не возвращает. Можно применять только в глобальной корректировке.
    // Формат (передаваемая строка):
    // MX<tag>^<delims>
    // где:
    // X – вид сортировки: I – по возрастанию; D – по убыванию.
    // <tag> – метка поля.
    // <delims> – разделители подполей, определяющих ключ сортировки.
    //
    // Пример:
    //
    // &unifor('MI910^BD')
    //

    static class UniforM
    {
        #region Private members

        class FieldToSort
        {
            public Field Field { get; set; }

            public string Text { get; set; }
        }

        static void SortField
            (
                Record record,
                int tag,
                char code,
                bool descending
            )
        {
            var found = record.Fields.GetField(tag);
            var fields = new FieldToSort[found.Length];

            for (var i = 0; i < found.Length; i++)
            {
                fields[i] = new FieldToSort
                {
                    Field = found[i],
                    Text = code == '\0'
                        ? found[i].ToText()
                        : found[i].GetFirstSubFieldValue(code).ToString()
                };
                record.Fields.Remove(found[i]);
            }

            fields =
                (
                    descending
                    ? fields.OrderByDescending(field => field.Text)
                    : fields.OrderBy(field => field.Text)
                )
                .ToArray();

            record.Fields.AddRange
                (
                    fields.Select
                        (
                            field => field.Field
                        )
                    .ToArray()
                );
        }

        #endregion

        #region Public methods

        public static void Sort
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression)
                && !ReferenceEquals(context.Record, null))
            {
                var navigator = new TextNavigator(expression);
                var direction = char.ToLower(navigator.ReadChar());
                if (direction != 'i' && direction != 'd')
                {
                    return;
                }

                string tagText = navigator.ReadUntil('^').ToString();
                if (string.IsNullOrEmpty(tagText))
                {
                    return;
                }

                int tag = tagText.SafeToInt32();
                var code = '\0';
                if (!navigator.IsEOF)
                {
                    navigator.ReadChar();
                    code = navigator.ReadChar();
                }

                SortField
                    (
                        context.Record,
                        tag,
                        code,
                        direction != 'i'
                    );
            }
        }

        #endregion
    }
}
