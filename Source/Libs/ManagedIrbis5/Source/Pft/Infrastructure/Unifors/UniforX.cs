// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforX.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Удаление из заданной строки фрагментов,
    // выделенных угловыми скобками<> – &uf('X')
    // Вид функции: X.
    // Назначение: Удаление из заданной строки фрагментов,
    // выделенных угловыми скобками<>.
    // Формат (передаваемая строка):
    // X<строка>
    //
    // Пример:
    //
    // &unifor("X"v200)
    //

    static class UniforX
    {
        #region Public methods

        /// <summary>
        /// Remove text surrounded with angle brackets.
        /// </summary>
        public static void RemoveAngleBrackets
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var result = expression;

                if (expression.Contains("<"))
                {
                    var builder = new StringBuilder(expression.Length);
                    var navigator = new TextNavigator(expression);
                    while (!navigator.IsEOF)
                    {
                        var text = navigator.ReadUntil('<').ToString();
                        builder.Append(text);
                        var c = navigator.ReadChar();
                        if (c != '<')
                        {
                            break;
                        }
                        text = navigator.ReadUntil('>').ToString();
                        c = navigator.ReadChar();
                        if (c != '>')
                        {
                            builder.Append('<');
                            builder.Append(text);
                        }
                    }
                    builder.Append(navigator.GetRemainingText());
                    result = builder.ToString();
                }

                context.WriteAndSetFlag(node, result);
            }
        }

        #endregion
    }
}
