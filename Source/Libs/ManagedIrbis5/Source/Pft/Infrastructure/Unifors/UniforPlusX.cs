// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusX.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+X')
    // Параметры: А#Б.
    // Ищет в базе термин из первого параметра, перемещается по инверсному файлу.
    // Пока найденны термин не встанет дальше указанного текста.
    // Модифицирует параметр Б той же функцией, что unifor('+W').
    // Контекст не закрывает, можно продолжить поиск через NextTerm.
    //

    static class UniforPlusX
    {
        #region Public methods

        /// <summary>
        /// Инкремент строки.
        /// </summary>
        public static void SearchIncrement
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            //
            // TODO Implement context non-closing
            // TODO NextTerm in provider
            //

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            // если в строке больше, чем 1 символ #, лишние части отбрасываются
            string[] parts = expression.Split
                (
                    CommonSeparators.NumberSign,
                    StringSplitOptions.None
                );
            if (parts.Length < 2)
            {
                return;
            }

            var term = parts[0];
            if (string.IsNullOrEmpty(term))
            {
                return;
            }
            var result = 0;
            while (true)
            {
                var provider = context.Provider;

                // имитация NextTerm, поиск, начиная с указанного термина
                // с подхватом следующего
                // ищем по 10 терминов, чтобы меньше дергать базу
                var parameters = new TermParameters
                {
                    Database = provider.Database,
                    StartTerm = term,
                    NumberOfTerms = 10
                };
                TermInfo[] terms = provider.ReadTerms(parameters);
                if (terms.Length == 0)
                {
                    break;
                }
                for (var i = 0; i < terms.Length; i++)
                {
                    result = string.CompareOrdinal(terms[i].Text, parts[0]);
                    term = terms[i].Text;
                    if (result == 1)
                    {
                        break;
                    }
                }

                if (result == 1)
                {
                    break;
                }
            }

            // TODO Implement
            // contextIrbis64.UniforPlusXTerm = term;

            if (!string.IsNullOrEmpty(parts[1]))
            {
                var output = UniforPlusW.Increment(parts[1]);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
