// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus4.cs -- выдача метки, порядкового номера и значения поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors;

//
// Выдача метки, порядкового номера и значения поля
// в соответствии с индексом (номером повторения)
// повторяющейся группы – &uf('+4')
// Вид функции: +4.
// Назначение: Выдача метки, порядкового номера и значения поля
// в соответствии с индексом (номером повторения)
// повторяющейся группы.
// Присутствует в версиях ИРБИС с 2005.2.
// Формат (передаваемая строка):
// +4XY
// где:
// Х принимает три значения: T – выдать метку;
// F – выдать значение поле; N – выдать порядковый номер поля
// в записи (отличается от индекса повторения,
// если Y принимает значение 1);
// Y принимает значения: 0 – поля выдаются в порядке
// расположения в записи; 1 – поля выдаются в порядке
// возрастания меток (по умолчанию 0).
//
// Пример:
//
// (&unifor('+4T1'),'_' &unifor('+4N1'),': ', &unifor('+4F1'),)
//

internal static class UniforPlus4
{
    #region Public methods

    /// <summary>
    /// Получение поля по выражению.
    /// </summary>
    public static void GetField
        (
            PftContext context,
            PftNode? node,
            string? expression
        )
    {
        Sure.NotNull (context);

        if (string.IsNullOrEmpty (expression))
        {
            return;
        }

        var record = context.Record;
        if (!ReferenceEquals (record, null))
        {
            var navigator = new TextNavigator (expression);
            var command = char.ToUpperInvariant (navigator.ReadChar());
            var order = char.ToUpperInvariant (navigator.ReadChar());
            if (command == TextNavigator.EOF
                || order == TextNavigator.EOF)
            {
                return;
            }

            // Поле GUID пропускается
            var workingFields = record.Fields
                .Where (field => field.Tag != IrbisGuid.Tag)
                .ToArray();
            if (order != '0')
            {
                Array.Sort (workingFields, FieldComparer.ByTag());
            }

            var index = context.Index;
            var currentField = workingFields.GetOccurrence (index);
            if (ReferenceEquals (currentField, null))
            {
                return;
            }

            string? output = null;

            switch (command)
            {
                case 'T':
                    output = currentField.Tag.ToInvariantString();
                    break;

                case 'F':
                    output = currentField.ToText();
                    break;

                case 'N':
                    var fieldIndex = record.Fields.IndexOf (currentField) + 1;
                    output = fieldIndex.ToInvariantString();
                    break;

                default:
                    Magna.Logger.LogError
                        (
                            nameof (UniforPlus4) + "::" + nameof(GetField)
                            + ": unknown command {Command}",
                            command
                        );
                    break;
            }

            context.WriteAndSetFlag (node, output);
        }
    }

    #endregion
}
