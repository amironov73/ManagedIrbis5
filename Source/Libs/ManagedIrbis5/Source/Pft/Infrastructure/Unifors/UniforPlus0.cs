// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus0.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать содержимое документа полностью
    // во внутреннем представлении – &uf('+0')
    // Вид функции: +0.
    // Назначение: Выдать содержимое документа полностью (формат ALL).
    // Формат(передаваемая строка):
    // +0
    // Результат расформатирования:
    // 0
    // 2#0
    // 0#1
    // 910#^YДА^PНЮАУ - каф. кримінального права
    // 920#ATHRA
    // 210#^AТацій^BВ. Я.^GВасиль Якович^8ukr
    // 710#^AТаций^BВ. Я.^GВасилий Яковлевич^8rus
    // 907#^A20110301^B111
    // 907#^A20110419^BZhukovskaya
    // 710#^ATatsiy^BV.^8eng
    // 907#^A20110421^BZhukovskaya
    // 907#^A20111108^B111
    //

    static class UniforPlus0
    {
        #region Public methods

        public static void FormatAll
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                var output = new StringBuilder();
                output.AppendLine("0");
                output.AppendLine(string.Format
                    (
                        "{0}#0", record.Mfn
                    ));
                output.AppendLine(string.Format
                    (
                        "0#{0}", record.Version
                    ));

                // Поле GUID выводится
                output.Append(record.ToPlainText());
                context.WriteAndSetFlag(node, output.ToString());
            }
        }

        #endregion
    }
}
