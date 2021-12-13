// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforY.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;
using AM.Collections;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Возвращает данные обо всех свободных (не выданных) экземплярах по всем местах хранения – &uf('Y')
    // Вид функции: Y.
    // Назначение: Возвращает данные обо всех свободных(не выданных) экземплярах по всем местах хранения.
    // Формат(передаваемая строка):
    // нет
    //
    // Пример:
    //
    // &unifor('Y')
    //

    static class UniforY
    {
        #region Public methods

        /// <summary>
        /// Вспомогательный метод.
        /// </summary>
        public static string FreeExemplars
            (
                Record record
            )
        {
            var exemplars = ExemplarInfo.ParseRecord(record);
            var counter
                = new DictionaryCounterInt32<string>();

            foreach (var exemplar in exemplars)
            {
                var place = exemplar.Place ?? string.Empty;

                switch (exemplar.Status)
                {
                    case ExemplarStatus.Summary:
                    case ExemplarStatus.BiblioNet:
                        var amountText = exemplar.Amount;
                        if (Utility.TryParseInt32(amountText, out var amount))
                        {
                            var onHandText = exemplar.OnHand;
                            if (Utility.TryParseInt32(onHandText, out var onHand))
                            {
                                amount -= onHand;
                            }
                        }
                        counter.Augment(place, amount);
                        break;

                    case ExemplarStatus.Free:
                        counter.Augment(place, 1);
                        break;
                }
            }

            var result = new StringBuilder();

            var first = true;
            foreach (var key in counter.Keys.OrderBy(_ => _))
            {
                var value = counter[key];
                if (!first)
                {
                    result.Append(", ");
                }
                first = false;
                result.AppendFormat
                    (
                        "{0}({1})",
                        key,
                        value
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Реализация &amp;uf('y').
        /// </summary>
        public static void FreeExemplars
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            if (!ReferenceEquals(context.Record, null))
            {
                var output = FreeExemplars(context.Record);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
