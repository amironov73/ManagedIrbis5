﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Umarci.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using AM;
using AM.Collections;
using ManagedIrbis.Pft.Infrastructure.Unifors;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    //
    // Unofficial documentation
    //
    // Форматный выход &amp;umarci
    //
    // &umarci('1N1#i#N2') - выбирает N2-е
    // повторение подполя i поля N1.
    //
    // &umarci('2N1#S') - определяет количество
    // вхождений строки S в поле N1; длина S <= 10 симв.
    //
    // &umarci('3N1#N2#R) - из поля N1 выбирает
    // информацию между (N2-1)-ым и N2-ым разделителями R,
    // если N2<1 и до N2, если N2=1.
    //
    // &umarci('4N1/N2') - выдает содержимое
    // поля с меткой N2, встроенного в поле N1.
    //
    // &umarci('0a') - когда-то использовалась
    // для замены разделителей, но теперь замена происходит,
    // если имя fst импорта содержит 'marc' как часть.
    //


    /// <summary>
    /// Umarci.
    /// </summary>
    public sealed class Umarci
        : IFormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        public static CaseInsensitiveDictionary<Action<PftContext, PftNode?, string>> Registry { get; } = new();

        /// <summary>
        /// Throw exception on unknown key.
        /// </summary>
        public static bool ThrowOnUnknown { get; set; } = false;

        #endregion

        #region Construction

        static Umarci()
        {
            RegisterActions();
        }

        #endregion

        #region Private members

        private static void RegisterActions()
        {
            Registry.Add("0", Umarci0);
            Registry.Add("1", Umarci1);
            Registry.Add("2", Umarci2);
            Registry.Add("3", Umarci3);
            Registry.Add("4", Umarci4);
            Registry.Add("5", Umarci5);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find action for specified expression.
        /// </summary>
        public static Action<PftContext, PftNode?, string>? FindAction
            (
                ref string expression
            )
        {
            var keys = Registry.Keys;
            var bestMatch = 0;
            Action<PftContext, PftNode?, string>? result = null;

            foreach (var key in keys)
            {
                if (key.Length > bestMatch
                    && expression.StartsWith(key))
                {
                    bestMatch = key.Length;
                    result = Registry[key];
                }
            }

            if (bestMatch != 0)
            {
                expression = expression.Substring(bestMatch);
            }

            return result;
        }

        /// <summary>
        /// Handle command 0.
        /// </summary>
        public static void Umarci0
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            // Nothing to do actually
        }

        /// <summary>
        /// Handle command 1.
        /// </summary>
        public static void Umarci1
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            //
            // field#code#repeat
            //
            // e.g.: if &umarci('1101#a#2') <> '' then ...
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            var parts = expression.Split('#');
            if (parts.Length != 3)
            {
                return;
            }

            var tag = parts[0];
            var code = parts[1];
            if (string.IsNullOrEmpty(tag)
                || code.Length != 1)
            {
                return;
            }

            if (!int.TryParse(parts[2], out var repeat))
            {
                return;
            }

            repeat--;
            if (repeat < 0)
            {
                return;
            }

            var field = context.Record.Fields
                .GetField(tag.SafeToInt32(), context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            var text = field.GetSubFieldValue(code[0], repeat);
            if (!text.IsEmpty())
            {
                context.Write(node, text);
            }
        }

        /// <summary>
        /// Handle command 2.
        /// </summary>
        public static void Umarci2
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            //
            // field#substring
            //
            // e.g.: if val(&umarci('2998#^a'))>1 then ...
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                context.Write(node, "0");

                return;
            }

            var parts = expression.Split('#');
            if (parts.Length != 2)
            {
                context.Write(node, "0");

                return;
            }

            var tag = parts[0];
            var substring = parts[1];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(substring))
            {
                context.Write(node, "0");

                return;
            }

            var field = context.Record.Fields
                .GetField(tag.SafeToInt32(), context.Index);
            if (ReferenceEquals(field, null))
            {
                context.Write(node, "0");

                return;
            }

            var text = field.ToText();
            if (string.IsNullOrEmpty(text))
            {
                context.Write(node, "0");

                return;
            }

            var result = text.CountSubstrings(substring);
            context.Write(node, result.ToInvariantString());
        }

        /// <summary>
        /// Handle command 3.
        /// </summary>
        public static void Umarci3
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            //
            // field#index#separator
            //
            // e.g.: &umarci('391#2#+')
            //

            //
            // sample field: #1: 11|22|33|44
            //
            // &umarci('31#0#|') gives 11|22|33|44
            // &umarci('31#1#|') gives 11
            // &umarci('31#2#|') gives 22
            // &umarci('31#3#|') gives 33
            // &umarci('31#4#|') gives 44
            // &umarci('31#5#|') gives empty string
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            var parts = expression.Split('#');
            if (parts.Length != 3)
            {
                return;
            }

            var tag = parts[0];
            var indexText = parts[1];
            var separator = parts[2];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(indexText)
                || string.IsNullOrEmpty(separator)
                || separator.Length != 1)
            {
                return;
            }

            var field = context.Record.Fields
                .GetField(tag.SafeToInt32(), context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            var text = field.ToText();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (!Utility.TryParseInt32(indexText, out var index))
            {
                return;
            }

            if (index <= 0)
            {
                context.Write(node, text);
                return;
            }
            index--;

            var positions = UniforE.GetPositions(text, separator[0]);

            if (positions.Length == 0)
            {
                //if (index < 0)
                //{
                //    context.Write(node, text);
                //}
                return;
            }

            if (index == 0)
            {
                text = text.Substring(0, positions[0]);
                context.Write(node, text);
                return;
            }

            int start, end, length;

            if (index < positions.Length)
            {
                start = positions[index - 1] + 1;
                end = positions[index];
                length = end - start;
                text = text.Substring(start, length);
            }
            else if (index == positions.Length)
            {
                start = positions[index - 1] + 1;
                end = text.Length;
                length = end - start;
                text = text.Substring(start, length);
            }
            else
            {
                text = string.Empty;
            }

            context.Write(node, text);
        }

        /// <summary>
        /// Handle command 4.
        /// </summary>
        public static void Umarci4
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            //
            // tag/embed^code
            //
            // e.g.: &umarci('4461/011^а')
            //

            if (string.IsNullOrEmpty(expression)
                || ReferenceEquals(context.Record, null))
            {
                return;
            }

            var parts = expression.Split('/');
            if (parts.Length != 2)
            {
                return;
            }

            var tag = parts[0];
            var embed = parts[1];
            if (string.IsNullOrEmpty(tag)
                || string.IsNullOrEmpty(embed))
            {
                return;
            }

            parts = embed.Split('^');
            var code = '\0';
            if (parts.Length == 2)
            {
                embed = parts[0];
                code = parts[1].ToCharArray().GetOccurrence(0);
            }

            var field = context.Record.Fields
                .GetField(tag.SafeToInt32(), context.Index);
            if (ReferenceEquals(field, null))
            {
                return;
            }

            field = EmbeddedField.GetEmbeddedField
                (
                    field,
                    embed.SafeToInt32()
                )
                .FirstOrDefault();
            if (ReferenceEquals(field, null))
            {
                return;
            }

            var text = code == '\0'
                ? field.ToText()
                : code == '*'
                    ? field.Value
                    : field.GetValueOrFirstSubField();

            context.Write(node, text);
        }

        /// <summary>
        /// ibatrak Handle command 5.
        /// </summary>
        public static void Umarci5
            (
                PftContext context,
                PftNode? node,
                string expression
            )
        {
            //ibatrak простая очистка за один проход
            //первого повторения подполей d e f c от скобок и завершающих разделителей
            var record = context.Record;
            if (ReferenceEquals(record, null))
            {
                return;
            }

            var tag = expression;
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            var field = record.Fields.GetField
                (
                    tag.SafeToInt32(),
                    context.Index
                );
            if (ReferenceEquals(field, null))
            {
                return;
            }
            var text = field.ToText();
            string[] subfields = { "^d", "^e", "^f", "^c" };
            string[] substrings = { ":^", ";^", " : ^", " ; ^", ")^", ")" };

            for (var i = 0; i < subfields.Length; i++)
            {
                var index = text.IndexOf(subfields[i], StringComparison.InvariantCulture);
                if (index < 0 || text.Length == index + 2)
                {
                    continue;
                }
                index += 2;
                //открывающая скобка убирается только в начале подполя
                if (text[index] == '(')
                {
                    text = text.Substring(0, index)
                           + text.SafeSubstring(index + 1, text.Length);
                    if (index == text.Length)
                    {
                        continue;
                    }
                }
                //цикл по подстрокам в конце подполя, которые надо убрать
                for (var j = 0; j < substrings.Length; j++)
                {
                    var subIndex = text.IndexOf(substrings[j], index, StringComparison.InvariantCulture);
                    if (subIndex < 0)
                    {
                        continue;
                    }
                    //подстроки длиннее 1 символа оканчиваются на символ ^, его необходимо оставить,
                    //так как это разделитель подполей
                    if (substrings[j].Length > 1)
                    {
                        text = text.Substring(0, subIndex)
                               + text.Substring(subIndex + substrings[j].Length - 1);
                    }
                    else
                    {
                        //завершающая скобка убирается, если стоит на конце строки
                        //для ситуации со скобкой на конце подполя сделан шаблон )^
                        if (subIndex == text.Length - 1)
                            text = text.Substring(0, subIndex) + text.Substring(subIndex + 1);
                    }
                }
            }

            context.WriteAndSetFlag(node, text);
        }

        #endregion

        #region IFormatExit members

        /// <inheritdoc cref="IFormatExit.Name" />
        public string Name { get { return "umarci"; } }

        /// <inheritdoc cref="IFormatExit.Execute" />
        public void Execute
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                Magna.Trace
                    (
                        "Umarci::Execute: "
                        + "empty expression"
                    );

                return;
            }

            var action = FindAction(ref expression);
            if (ReferenceEquals(action, null))
            {
                Magna.Error
                    (
                        "Umarci::Execute: "
                        + "unknown action="
                        + expression.ToVisibleString()
                    );

                if (ThrowOnUnknown)
                {
                    throw new PftException
                        (
                            "Unknown unifor: "
                            + expression.ToVisibleString()
                        );
                }
            }
            else
            {
                action
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        #endregion
    }
}
