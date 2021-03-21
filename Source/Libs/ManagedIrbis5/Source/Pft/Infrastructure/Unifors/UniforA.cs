// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforA.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using AM;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Выдать заданное повторение поля – &uf('A
    // Вид функции: A.
    // Назначение: Выдать заданное повторение поля.
    // Формат (передаваемая строка):
    // AV<tag>^<delim>*<offset>.<length>#<occur>
    // где:
    // <tag> – метка поля;
    // <delim> – разделитель подполя;
    // <offset> – смещение;
    // <length> – длина;
    // <occur> – номер повторения.
    //
    // Примеры:
    //
    // &unifor('Av200#2')
    // &unifor('Av910^a#5')
    // &unifor('Av10^b*2.10#2')
    //

    static class UniforA
    {
        #region Public methods

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetFieldRepeat
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            try
            {
                var record = context.Record;
                if (!ReferenceEquals(record, null))
                {
                    var specification = new FieldSpecification();
                    if (specification.ParseUnifor(expression)
                        && specification.FieldRepeat.Kind != IndexKind.None)
                    {
                        if (specification.Tag != IrbisGuid.Tag)
                        {
                            // Поле GUID пропускается

                            var reference = new FieldReference();
                            reference.Apply(specification);

                            var result = reference.Format(record);
                            if (!string.IsNullOrEmpty(result))
                            {
                                context.WriteAndSetFlag(node, result);
                                context.VMonitor = true;
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "UniforA::GetFieldRepeat",
                        exception
                    );
            }
        }

        #endregion
    }
}
