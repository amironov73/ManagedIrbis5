// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Unifor9.cs -- удаление двойных кавычек из заданной строки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Удалить двойные кавычки из заданной строки – &uf('9')
    // Вид функции: 9.
    // Назначение: Удалить двойные кавычки из заданной строки.
    // Формат (передаваемая строка):
    // 9<исх.строка>
    //
    // Пример:
    //
    // &unifor("9"v200^a)
    //

    static class Unifor9
    {
        #region Public methods

        /// <summary>
        /// Remove double quotes from the string.
        /// </summary>
        public static void RemoveDoubleQuotes
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = expression.Replace("\"", string.Empty);
                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
