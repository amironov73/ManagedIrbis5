// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusPlus0.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать содержимое документа полностью - только
    // содержимое полей – &uf('++0')
    // Вид функции: ++0.
    // Назначение: Выдать содержимое документа полностью (формат ALLl).
    // Формат(передаваемая строка):
    // ++0
    //

    //
    // Начиная с версии 2016.1, форматный выход &uf('++0')
    // модифицирован следующим образом
    // &uf('++0,nnn,mmm,...')
    // nnn, mmm - метки полей, которые ИСКЛЮЧАЮТСЯ из форматирования.
    //

    static class UniforPlusPlus0
    {
        #region Public methods

        public static void FormatAll
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var tagsToSkip = new List<int> { 953 };
            if (!string.IsNullOrEmpty(expression))
            {
                foreach (string item in expression.Split(','))
                {
                    if (Utility.TryParseInt32(item, out var tag))
                    {
                        if (tag > 0)
                        {
                            tagsToSkip.Add(tag);
                        }
                    }
                }
            }

            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                var output = new StringBuilder();
                foreach (var field in record.Fields)
                {
                    if (field.Tag == IrbisGuid.Tag)
                    {
                        // Поле GUID не выводится
                        continue;
                    }

                    if (tagsToSkip.Contains(field.Tag))
                    {
                        continue;
                    }

                    if (field.IsEmpty)
                    {
                        continue;
                    }

                    var fieldValue = field.Value;
                    if (!fieldValue.IsEmpty)
                    {
                        output.Append(fieldValue);
                    }

                    foreach (SubField subField in field.Subfields)
                    {
                        output.Append(" ");
                        output.Append(subField.Value);
                    }
                    output.AppendLine();
                }
                context.WriteAndSetFlag(node, output.ToString());
            }
        }

        #endregion
    }
}
