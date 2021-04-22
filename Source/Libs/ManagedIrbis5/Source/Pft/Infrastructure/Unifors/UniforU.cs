// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforU.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;

using AM;
using AM.Text;
using AM.Text.Ranges;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Кумуляция номеров журналов – &uf('U')
    // Вид функции: U.
    // Назначение: Кумуляция номеров журналов.
    // Формат (передаваемая строка):
    // U<strbase>,<stradd>
    // где:
    // <strbase> – исходная кумулированная строка.
    // <stradd> – кумулируемые номера.
    //
    // Пример:
    //
    // &unifor("U"v909^h",12")
    //

    static class UniforU
    {
        #region Public methods

        /// <summary>
        /// Check whether the issue is present
        /// in cumulated collection.
        /// </summary>
        public static void Check
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var parts = expression.Split
                    (
                        CommonSeparators.Comma,
                        2
                    );
                if (parts.Length == 2)
                {
                    var issue = parts[0];
                    var cumulated = parts[1];

                    var result = Check(issue, cumulated);
                    var output = result ? "1" : "0";
                    context.Write(node, output);
                }
            }
        }

        /// <summary>
        /// Check whether the issue is present
        /// in cumulated collection.
        /// </summary>
        public static bool Check
            (
                string issue,
                string cumulated
            )
        {
            var collection = NumberRangeCollection.Parse(cumulated);
            var number = new NumberText(issue);
            var result = collection.Contains(number);

            return result;
        }

        /// <summary>
        /// Cumulate the magazine issues.
        /// </summary>
        public static string Cumulate
            (
                string issues
            )
        {
            try
            {
                var collection = NumberRangeCollection.Parse(issues);
                var numbers = collection
                    .Distinct()
                    .ToList();
                var result = NumberRangeCollection.Cumulate(numbers);

                return result.ToString();
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "UniforU::Cumulate",
                        exception
                    );

                return issues;
            }
        }

        /// <summary>
        /// Cumulate the magazine issues.
        /// </summary>
        public static void Cumulate
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Cumulate(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        /// <summary>
        /// Decumulate the magazine issues.
        /// </summary>
        public static string Decumulate
            (
                string issues
            )
        {
            var collection = NumberRangeCollection.Parse(issues);
            var result = new StringBuilder();
            var first = true;
            foreach (var number in collection)
            {
                if (!first)
                {
                    result.Append(',');
                }
                first = false;
                result.Append(number);
            }

            return result.ToString();
        }

        /// <summary>
        /// Decumulate the magazine issues.
        /// </summary>
        public static void Decumulate
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Decumulate(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
