﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniforC.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Identifiers;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Контроль ISSN/ISBN – &uf('C')
    // Вид функции: C.
    // Назначение: Контроль ISSN/ISBN.Возвращаемое значение:
    // 0 - при положительном результате, 1 - при отрицательном.
    // Формат (передаваемая строка):
    // С<ISSN/ISBN>
    //
    // Пример:
    //
    // &unifor("C"v10^a)
    //

    static class UniforC
    {
        #region Public methods

        /// <summary>
        /// Контроль ISSN/ISBN.
        /// Возвращаемое значение: 0 – при положительном
        /// результате, 1 – при отрицательном.
        /// </summary>
        public static void CheckIsbn
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expresion
            )
        {
            Code.NotNull(context, "context");

            // Пустой ISBN считается правильным
            string output = "1";

            if (!string.IsNullOrEmpty(expresion))
            {
                List<char> digits = new List<char>(expresion.Length);
                foreach (char c in expresion)
                {
                    if (PftUtility.DigitsX.Contains(c))
                    {
                        digits.Add(c);
                    }
                }
                if (digits.Count == 8)
                {
                    if (Issn.CheckControlDigit(expresion))
                    {
                        output = "0";
                    }
                }
                else if (digits.Count == 10 || digits.Count == 13)
                {
                    if (Isbn.Validate(expresion, false))
                    {
                        output = "0";
                    }
                }
            }

            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
