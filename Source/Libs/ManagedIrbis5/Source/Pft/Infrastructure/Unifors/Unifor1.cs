// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Unifor1.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданный подэлемент – &uf('1')
    //
    // Формат (передаваемая строка):
    // 1NCXY?V<tag>^<delim>*<offset>.<length>#<occur>
    //
    // где:
    // N – номер повторения подэлемента;
    // если указана * – номер подэлемента совпадает
    // со счетчиком повторяющейся группы.
    // ХY – разделители между подэлементами.
    // С – принимает значения: R – разделители
    // справа от каждого подэлемента, кроме последнего;
    // L – разделители слева от каждого подэлемента;
    // D – каждый подэлемент заключен слева
    // разделителем Х и справа – Y.
    // ? – символ-разделитель.
    //
    // Остальные параметры аналогичны параметрам для функции
    // 'Выдать заданное повторение поля' – &uf('A')
    //
    // Пример:
    //
    // (/&unifor('1*R; ?v910^h#1'))
    //

    static class Unifor1
    {
        #region Public methods

        public static void GetElement
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            var navigator = new TextNavigator(expression);

            var index = context.Index;
            if (navigator.PeekChar() == '*')
            {
                navigator.ReadChar();
            }
            else
            {
                var indexText = navigator.ReadInteger().ToString();
                if (string.IsNullOrEmpty(indexText))
                {
                    return;
                }
                if (!Utility.TryParseInt32(indexText, out index))
                {
                    return;
                }
            }

            var mode = navigator.ReadChar();
            var left = navigator.ReadChar();
            var right = navigator.ReadChar();
            var question = navigator.ReadChar();
            if (question != '?')
            {
                return;
            }

            var command = navigator.ReadChar();
            if (command != 'v'
                && command != 'V')
            {
                return;
            }

            var tagText = navigator.ReadInteger().ToString();
            if (string.IsNullOrEmpty(tagText))
            {
                return;
            }
            var tag = tagText.SafeToInt32();
            if (tag == IrbisGuid.Tag)
            {
                // Поле GUID не выводится
                return;
            }

            var code = SubField.NoCode;
            if (navigator.PeekChar() == '^')
            {
                navigator.ReadChar();
                code = navigator.ReadChar();
                if (code == SubField.NoCode)
                {
                    return;
                }
            }

            int offset = 0, length = 0;
            if (navigator.PeekChar() == '*')
            {
                navigator.ReadChar();
                var offsetText = navigator.ReadInteger().ToString();
                if (!Utility.TryParseInt32(offsetText, out offset))
                {
                    return;
                }
            }
            if (navigator.PeekChar() == '.')
            {
                navigator.ReadChar();
                var lengthText = navigator.ReadInteger().ToString();
                if (!Utility.TryParseInt32(lengthText, out length))
                {
                    return;
                }
            }

            var repeat = 0;
            if (navigator.PeekChar() == '#')
            {
                navigator.ReadChar();
                if (navigator.PeekChar() == '*')
                {
                    repeat = context.Index;
                }
                else
                {
                    var repeatText = navigator.ReadInteger().ToString();
                    if (!Utility.TryParseInt32(repeatText, out repeat))
                    {
                        return;
                    }
                }
            }
            repeat = repeat - 1;

            var field = context.Record
                .Fields.GetField(tag, repeat);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            var text = code == SubField.NoCode
                ? field.ToText()
                : field.GetFirstSubFieldValue(code);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            if (offset != 0 || length != 0)
            {
                text = PftUtility.SafeSubString(text, offset, length);
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            string? output;

            var items = text.Split(left, right);
            if (mode == 'R' || mode == 'r')
            {
                output = items.GetOccurrence(index);
            }
            else if (mode == 'L' || mode == 'l')
            {
                output = items.GetOccurrence(index);
            }
            else
            {
                throw new NotImplementedException();
            }

            if (!string.IsNullOrEmpty(output))
            {
                context.WriteAndSetFlag(node, output);
                context.VMonitor = true;
            }
        }

        #endregion
    }
}
