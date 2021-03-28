// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus1.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Устаревшие функции для работы с глобальными переменными
    //

    static class UniforPlus1
    {
        #region Private members

        // ================================================================

        private static string _GetGlobal
            (
                PftContext context,
                int index
            )
        {
            var fields = context.Globals.Get(index);
            var result = new StringBuilder();
            var first = true;
            foreach (var field in fields)
            {
                if (!first)
                {
                    result.AppendLine();
                }
                first = false;
                result.Append(field.ToText());
            }

            return result.ToString();
        }

        // ================================================================

        public static List<string> _GetGlobals
            (
                PftContext context,
                int index,
                int count
            )
        {
            if (count <= 0)
            {
                return new List<string>();
            }

            var result = new List<string>(count);
            while (count > 0)
            {
                if (context.Globals.HaveVariable(index))
                {
                    var item = _GetGlobal(context, index);
                    var lines = item.SplitLines();
                    result.AddRange(lines);
                }
                count--;
                index++;
            }

            return result;
        }

        // ================================================================

        /// <summary>
        /// Parse NNN,nnn
        /// </summary>
        private static int[]? _ParseOne
            (
                PftContext context,
                string? expression
            )
        {
            int[]? result = null;
            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            string[] parts = expression.Split
                (
                    CommonSeparators.Comma,
                    2
                );

            int index;
            var specification = parts[0];
            if (specification.StartsWith("*"))
            {
                specification = specification.Substring(1);
                if (specification.StartsWith("+"))
                {
                    specification = specification.Substring(1);
                }
                if (Utility.TryParseInt32(specification, out index))
                {
                    index += context.Index + 1;
                }
                else
                {
                    index = context.Index + 1;
                }
            }
            else
            {
                if (!Utility.TryParseInt32(specification, out index))
                {
                    goto DONE;
                }
            }

            var count = 1;
            if (parts.Length == 2)
            {
                count = parts[1].SafeToInt32(count);
            }

            result = new[]
            {
                index,
                count
            };

            DONE:
            return result;
        }

        // ================================================================

        /// <summary>
        /// Parse NNN,nnn#MMM,mmm
        /// </summary>
        private static int[]? _ParsePair
            (
                string? expression
            )
        {
            int[]? result = null;
            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            string[] parts = expression.Split
                (
                    CommonSeparators.NumberSign,
                    2
                );
            if (parts.Length != 2)
            {
                goto DONE;
            }

            string[] subs = parts[0].Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (!Utility.TryParseInt32(subs[0], out var firstIndex))
            {
                goto DONE;
            }

            var firstCount = 1;
            if (subs.Length == 2)
            {
                firstCount = subs[1].SafeToInt32(firstCount);
            }
            subs = parts[1].Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (!Utility.TryParseInt32(subs[0], out var secondIndex))
            {
                goto DONE;
            }
            var secondCount = 1;
            if (subs.Length == 2)
            {
                secondCount = subs[1].SafeToInt32(secondCount);
            }

            result = new[]
            {
                firstIndex,
                firstCount,
                secondIndex,
                secondCount
            };

            DONE:
            return result;
        }

        // ================================================================

        /// <summary>
        /// Удаляем пустые строки в конце списка.
        /// </summary>
        private static void _RemoveEmptyTailLines
            (
                List<string> list
            )
        {
            while (list.Count != 0)
            {
                // Удаляем пустые строки в конце
                var offset = list.Count - 1;
                if (string.IsNullOrEmpty(list[offset]))
                {
                    list.RemoveAt(offset);
                }
                else
                {
                    break;
                }
            }
        }

        #endregion

        #region Public methods

        // ================================================================

        //
        // Сложение двух списков (групп переменных) – &uf('+1A
        // Вид функции: +1A.
        // Назначение: Сложение двух списков(групп переменных).
        // Формат(передаваемая строка):
        // +1ANNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void AddGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            var first = _GetGlobals(context, pair[0], pair[1]);
            var second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0
                && second.Count == 0)
            {
                return;
            }

            var flag = true;
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            foreach (var item in second)
            {
                if (!flag)
                {
                    context.WriteLine(node);
                }
                context.Write(node, item);
                context.OutputFlag = true;
                flag = false;
            }

            foreach (var item in first)
            {
                if (!second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.WriteAndSetFlag(node, item);
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Очистить(опустошить) все глобальные переменные – &uf('+1
        // Вид функции: +1.
        // Назначение: Очистить (опустошить) все глобальные переменные.
        // Формат (передаваемая строка):
        // +1
        //
        // Пример:
        //
        // &unifor('+1')
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
        // Групповая мультираскодировка списка
        // Формат:
        // +1O<MNU>|SSSS
        // где:
        // <MNU> - имя справочника(с расширением);
        // SSSS - список строк(результат расформатирования
        //
        // Пример:
        //
        // &unifor(‘+1Omhr.mnu|’,(v910^m/))
        //

        public static void DecodeList
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            /*

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            string[] parts = expression.Split
                (
                    CommonSeparators.VerticalLine,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var menuName = parts[0];
            var specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = context.Provider.Database,
                    FileName = menuName
                };
            var menu = context.Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menu, null))
            {
                return;
            }
            if (menu.Entries.Count == 0)
            {
                return;
            }

            var lines = new List<string>(parts[1].SplitLines());
            _RemoveEmptyTailLines(lines);

            if (lines.Count == 0)
            {
                return;
            }

            var first = true;
            foreach (var line in lines)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                var value = menu.GetStringSensitive(line);
                context.WriteAndSetFlag(node, value);

                first = false;
            }

            */
        }

        // ================================================================

        //
        // Групповая мультираскодировка переменных
        // Формат:
        // +1K<MNU>|NNN,nnn
        // где:
        // <MNU> - имя справочника(с расширением);
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor(‘+1Kmhr.mnu|100,10’)
        //

        public static void DecodeGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            /*

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            string[] parts = expression.Split
                (
                    CommonSeparators.VerticalLine,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var menuName = parts[0];
            var specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = context.Provider.Database,
                    FileName = menuName
                };
            var menu = context.Provider.ReadMenuFile(specification);
            if (ReferenceEquals(menu, null))
            {
                return;
            }
            if (menu.Entries.Count == 0)
            {
                return;
            }

            var one = _ParseOne(context, parts[1]);
            if (ReferenceEquals(one, null))
            {
                return;
            }
            var index = one[0];
            var count = one[1];

            var lines = _GetGlobals(context, index, count);

            _RemoveEmptyTailLines(lines);
            if (lines.Count == 0)
            {
                return;
            }

            var first = true;
            foreach (var line in lines)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                var output = menu.GetStringSensitive(line);
                context.WriteAndSetFlag(node, output);

                first = false;
            }

            */
        }

        // ================================================================

        //
        // Исключение неоригинальных значений из группы переменных – &uf('+1G
        // Вид функции: +1G.
        // Назначение: Исключение неоригинальных значений из группы переменных.
        // Формат(передаваемая строка):
        // +1GNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной.
        // nnn – кол-во переменных(по умолчанию 1).
        //

        public static void DistinctGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }

            var index = one[0];
            var count = one[1];

            var list = new List<string>(count);
            var dictionary
                = new CaseInsensitiveDictionary<object>();
            while (count > 0)
            {
                if (context.Globals.HaveVariable(index))
                {
                    var item = _GetGlobal(context, index);
                    if (!dictionary.ContainsKey(item))
                    {
                        list.Add(item);
                        dictionary.Add(item, null);
                    }
                }
                count--;
                index++;
            }

            _RemoveEmptyTailLines(list);
            if (list.Count == 0)
            {
                return;
            }

            foreach (var line in list)
            {
                if (string.IsNullOrEmpty(line))
                {
                    context.WriteLine(node);
                }
                else
                {
                    context.WriteLine(node, line);
                }
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // +1I
        // Исключение неоригинальных значений из списка
        // Формат:
        // +1ISSSS
        //

        public static void DistinctList
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

            var original = expression.SplitLines();
            if (original.Length == 0)
            {
                return;
            }

            var dictionary
                = new CaseInsensitiveDictionary<object>();

            foreach (var line in original)
            {
                var copy = line ?? string.Empty;
                if (!dictionary.ContainsKey(copy))
                {
                    dictionary.Add(copy, null);
                    if (string.IsNullOrEmpty(line))
                    {
                        context.WriteLine(node);
                    }
                    else
                    {
                        context.WriteLine(node, line);
                    }
                    context.OutputFlag = true;
                }
            }
        }

        // ================================================================

        //
        // Перемножение двух списков (групп переменных) – &uf('+1M
        // Вид функции: +1M.
        // Назначение: Перемножение двух списков(групп переменных).
        // Формат(передаваемая строка):
        // +1MNNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void MultiplyGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            var first = _GetGlobals(context, pair[0], pair[1]);
            var second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0
                || second.Count == 0)
            {
                return;
            }

            var flag = true;
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            foreach (var item in first)
            {
                if (second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.WriteAndSetFlag(node, item);
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Чтение глобальных переменных – &uf('+1R
        // Вид функции: +1R.
        // Назначение: Чтение глобальных переменных.
        // Присутствует в версиях ИРБИС с 2004.1.
        // Формат (передаваемая строка):
        // +1RNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor('+1R100,2')
        //

        public static void ReadGlobal
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }

            var index = one[0];
            var count = one[1];
            var lines = _GetGlobals(context, index, count);
            _RemoveEmptyTailLines(lines);

            var flag = false;
            foreach (var line in lines)
            {
                if (flag)
                {
                    context.WriteLine(node);
                }
                context.WriteAndSetFlag(node, line);
                flag = true;
            }
        }

        // ================================================================

        //
        // Сортировка группы переменных – &uf('+1T
        // Вид функции: +1T.
        // Назначение: Сортировка группы переменных.
        // Формат (передаваемая строка):
        // +1TNNN,nnn
        // где:
        // NNN – номер первой или единственной переменной,
        // nnn – кол-во переменных (по умолчанию 1).
        //
        // Пример:
        //
        // &unifor('+1T100,4')
        //

        public static void SortGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var one = _ParseOne(context, expression);
            if (ReferenceEquals(one, null))
            {
                return;
            }

            var index = one[0];
            var count = one[1];
            var lines = _GetGlobals(context, index, count);
            _RemoveEmptyTailLines(lines);
            if (lines.Count == 0)
            {
                return;
            }

            lines.Sort(StringComparer.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                context.WriteLine(node, line);
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // +1V
        // Сортировка списка
        // Формат:
        // +1VSSSS
        //

        public static void SortList
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

            var lines = expression.SplitLines();
            Array.Sort
                (
                    lines,
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    context.WriteLine(node);
                }
                else
                {
                    context.WriteLine(node, line);
                }
                context.OutputFlag = true;
            }
        }

        // ================================================================

        //
        // Вычитание списков (групп переменных) – &uf('+1S
        // Вид функции: +1S.
        // Назначение: Вычитание списков(групп переменных).
        // Формат(передаваемая строка):
        // +1SNNN,nnn#MMM,mmm
        // где:
        // NNN,MMM – номер первой или единственной переменной.
        // nnn,mmm – кол-во переменных(по умолчанию 1).
        //

        public static void SubstractGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var pair = _ParsePair(expression);
            if (ReferenceEquals(pair, null))
            {
                return;
            }

            var first = _GetGlobals(context, pair[0], pair[1]);
            var second = _GetGlobals(context, pair[2], pair[3]);
            if (first.Count == 0)
            {
                return;
            }

            var flag = true;
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            foreach (var item in first)
            {
                if (!second.Contains(item, comparer))
                {
                    if (!flag)
                    {
                        context.WriteLine(node);
                    }
                    context.WriteAndSetFlag(node, item);
                    flag = false;
                }
            }
        }

        // ================================================================

        //
        // Запись в глобальные переменные – &uf('+1W
        // Вид функции: +1W.
        // Назначение: Запись в глобальные переменные.
        // Присутствует в версиях ИРБИС с 2004.1.
        // Формат (передаваемая строка):
        // +1WNNN,MMM#SSSS
        // где:
        // NNN – номер первой или единственной переменной,
        // возможна конструкция *+-<число>. * – номер текущего повторения
        // в повторяющейся группе.
        // MMM – номер переменной для сохранения кол-ва записанных переменных
        // (по умолчанию не используется).
        // SSSS – список строк(результат расформатирования).
        // Если задан MMM – каждая строка пишется в отдельную переменную,
        // в противном случае все пишется в одну переменную.
        //
        // Пример:
        //
        // &unifor('+1W100,0#',(v910/))
        //

        public static void WriteGlobal
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

            string[] parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );

            var indexText = parts[0];
            var valueText = string.Empty;
            if (parts.Length == 2)
            {
                valueText = parts[1] ?? string.Empty;
            }

            parts = indexText.Split
                (
                    CommonSeparators.Comma,
                    2
                );
            indexText = parts[0];

            var useCount = false;
            var countVariable = 0;
            if (parts.Length == 2)
            {
                if (!Utility.TryParseInt32
                    (
                        parts[1],
                        out countVariable
                    ))
                {
                    return;
                }
                useCount = true;
            }

            if (!Utility.TryParseInt32(indexText, out var index))
            {
                return;
            }

            if (useCount)
            {
                var lines = valueText.SplitLines();
                foreach (var line in lines)
                {
                    context.Globals.Add(index, line);
                    index++;
                }

                context.Globals.Add
                    (
                        countVariable,
                        lines.Length.ToInvariantString()
                    );
            }
            else
            {
                context.Globals.Add(index, valueText);
            }
        }

        // ================================================================

        #endregion
    }
}
