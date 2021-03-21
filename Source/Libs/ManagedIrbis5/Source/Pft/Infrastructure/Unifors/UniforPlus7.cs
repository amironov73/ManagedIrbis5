// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus7.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    // Работа с индивидуальными повторяющимися глобальными переменными.

    static class UniforPlus7
    {
        #region Private members

        private static bool _Contains
            (
                IEnumerable<Field> fields,
                Field oneField
            )
        {
            string text = oneField.ToText();
            foreach (var field in fields)
            {
                if (field.ToText() == text)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public methods

        // ================================================================

        //
        // Добавление повторений глобальной переменной – &uf('+7U')
        // Вид функции: +7U.
        // Назначение: Добавление повторений глобальной переменной.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7UNNN#SSSS
        // где:
        // NNN – номер переменной;
        // SSSS – список строк(результат расформатирования);
        // каждая строка становится отдельным повторением.
        //

        public static void AppendGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );

                if (parts.Length == 2)
                {
                    if (Utility.TryParseInt32(parts[0], out var index))
                    {
                        context.Globals.Append(index, parts[1]);
                    }
                }
            }
        }

        // ================================================================

        //
        // Очистить(опустошить) все глобальные переменные – &uf('+7')
        // Вид функции: +7.
        // Назначение: Очистить (опустошить) все глобальные переменные.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7
        // Пример:
        // &unifor('+7')
        //

        public static void ClearGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            context.Globals.Clear();
        }

        // ================================================================

        //
        // Исключение неоригинальных повторений переменной – &uf('+7G')
        // Вид функции: +7G.
        // Назначение: Исключение неоригинальных повторений переменной.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7GNNN
        // Результат исключения записывается в ту же переменную.
        //

        public static void DistinctGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7G1')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                if (Utility.TryParseInt32(expression, out var index))
                {
                    Field[] fields = context.Globals.Get(index);
                    List<Field> result = new List<Field>();
                    foreach (Field field in fields)
                    {
                        if (!_Contains(result, field))
                        {
                            result.Add(field);
                        }
                    }
                    context.Globals.Set(index, result);
                }
            }
        }

        // ================================================================

        //
        // Логическое перемножение повторений двух переменных – &uf('+7M')
        // Вид функции: +7M.
        // Назначение: Логическое перемножение повторений двух переменных.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7MNNN#MMM
        // Результат перемножения записывается в первую переменную.
        //

        public static void MultiplyGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7M1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                var parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }

                if (!Utility.TryParseInt32(parts[0], out var firstIndex)
                    || !Utility.TryParseInt32(parts[1], out var secondIndex))
                {
                    return;
                }

                var first = context.Globals.Get(firstIndex);
                var second = context.Globals.Get(secondIndex);
                var result = first.Where
                    (
                        one => _Contains(second, one)
                    )
                    .ToArray();
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        //
        // Чтение глобальной переменной – &uf('+7R')
        // Вид функции: +7R.
        // Назначение: Чтение глобальной переменной.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7RNNN, nnn
        // где:
        // NNN – номер переменной.
        // nnn – номер повторения. По умолчанию – номер текущего
        // повторения в повторяющейся группе.
        // Эквивалентная конструкция языка форматирования GNNN.
        //
        // Пример:
        //
        // &unifor('+7R100')
        // эквивалентная конструкция языка форматирования:
        // G100
        //

        public static void ReadGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string[] parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );

                var indexText = parts[0];
                var haveRepeat = !ReferenceEquals(context.CurrentGroup, null);
                var repeat = context.Index;
                if (parts.Length == 2)
                {
                    var repeatText = parts[1];
                    if (!Utility.TryParseInt32(repeatText, out repeat))
                    {
                        return;
                    }
                    haveRepeat = true;
                    repeat--;
                }

                if (Utility.TryParseInt32(indexText, out var index))
                {
                    var fields = context.Globals.Get(index);

                    if (haveRepeat)
                    {
                        var field = fields.GetOccurrence(repeat);
                        if (!ReferenceEquals(field, null))
                        {
                            string output = field.ToText();
                            context.WriteAndSetFlag(node, output);
                        }
                    }
                    else
                    {
                        var output = new StringBuilder();
                        var first = true;
                        foreach (Field field in fields)
                        {
                            if (!first)
                            {
                                output.AppendLine();
                            }
                            first = false;
                            output.Append(field.ToText());
                        }
                        context.WriteAndSetFlag(node, output.ToString());
                    }
                }
            }
        }

        // ================================================================

        //
        // Сортировка повторений переменной – &uf('+7T')
        // Вид функции: +7T.
        // Назначение: Сортировка повторений переменной.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7TNNN
        // Результат сортировки записывается в ту же переменную.
        //

        public static void SortGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7T1')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                if (Utility.TryParseInt32(expression, out var index))
                {
                    Field[] fields = context.Globals.Get(index);
                    Array.Sort
                        (
                            fields,
                            (left, right) => string.Compare
                                (
                                    left.ToString(),
                                    right.ToString(),
                                    StringComparison.CurrentCulture
                                )
                        );
                    context.Globals.Set(index, fields);
                }
            }
        }

        // ================================================================

        //
        // Логическое вычитание повторений двух переменных – &uf('+7S')
        // Вид функции: +7S.
        // Назначение: Логическое вычитание повторений двух переменных.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7SNNN#MMM
        // Результат вычитания записывается в первую переменную.
        //

        public static void SubstractGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7S1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                var parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }

                if (!Utility.TryParseInt32(parts[0], out var firstIndex)
                    || !Utility.TryParseInt32(parts[1], out var secondIndex))
                {
                    return;
                }

                var first = context.Globals.Get(firstIndex);
                var second = context.Globals.Get(secondIndex);
                var result = first.Where
                    (
                        one => !_Contains(second, one)
                    )
                    .ToArray();
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        //
        // Логическое сложение повторений двух переменных – &uf('+7A')
        // Вид функции: +7A.
        // Назначение: Логическое сложение повторений двух переменных.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7ANNN#MMM
        // Результат сложения записывается в первую переменную.
        //

        public static void UnionGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            // &uf('+7')
            // &uf('+7W1#111'),&uf('+7U1#222'),&uf('+7U1#333'),&uf('+7U1#111')
            // &uf('+7W2#222'),&uf('+7U2#333'),&uf('+7U2#444')
            // &uf('+7S1#2')
            // &uf('+7R1')

            if (!string.IsNullOrEmpty(expression))
            {
                var parts = expression.Split('#');
                if (parts.Length != 2)
                {
                    return;
                }

                if (!Utility.TryParseInt32(parts[0], out var firstIndex)
                    || !Utility.TryParseInt32(parts[1], out var secondIndex))
                {
                    return;
                }

                var first = context.Globals.Get(firstIndex);
                var second = context.Globals.Get(secondIndex);
                var result = new List<Field>(second);
                foreach (Field field in first)
                {
                    if (!_Contains(result, field))
                    {
                        result.Add(field);
                    }
                }
                context.Globals.Set(firstIndex, result);
            }
        }

        // ================================================================

        //
        // Запись глобальной переменной – &uf('+7W')
        // Вид функции: +7W.
        // Назначение: Запись глобальной переменной.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +7WNNN#SSSS
        // где:
        // NNN – номер переменной;
        // SSSS – список строк(результат расформатирования);
        // каждая строка становится отдельным повторением.
        //
        // Пример:
        //
        // &unifor('+7W100#', (v910/))
        //

        public static void WriteGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {

                string[] parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );

                if (parts.Length == 2)
                {
                    if (Utility.TryParseInt32(parts[0], out var index))
                    {
                        context.Globals.Add(index, parts[1]);
                    }
                }
            }
        }

        // ================================================================

        #endregion
    }
}
