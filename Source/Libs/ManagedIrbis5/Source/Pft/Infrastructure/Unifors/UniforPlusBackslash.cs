﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusBackslash.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Преобразование строки, удваивающее обратный слэш, или обратное – &uf('+\')
    // Вид функции: +\.
    // Назначение: Преобразование строки, удваивающее обратный слэш, или обратное.
    // Формат (передаваемая строка):
    // +\N<строка>
    // где:
    // N может принимать значения
    // 0 - удвоение знаков обратного слэш;
    // 1 - преобразование удвоенных знаков слэш в одинарные.
    //
    // Примеры:
    //
    // Результатом формата
    // &uf('+\0c:\example.txt')
    // будет строка
    // c:\\example.txt
    //
    // Результатом формата
    // &uf('+\1c:\\example.txt')
    // будет строка
    // c:\example.txt
    //

    static class UniforPlusBackslash
    {
        #region Public methods

        /// <summary>
        /// Convert backslashes.
        /// </summary>
        public static void ConvertBackslashes
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                var command = navigator.ReadChar();
                var text = navigator.GetRemainingText().ToString();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }
                if (!text.Contains("\\"))
                {
                    context.WriteAndSetFlag(node, text);

                    return;
                }

                var ok = command == '1';
                navigator = new TextNavigator(text);
                while (!navigator.IsEOF)
                {
                    navigator.SkipTo('\\');
                    if (navigator.PeekChar() != '\\')
                    {
                        break;
                    }

                    var chunk = navigator.ReadWhile('\\').ToString();
                    var length = chunk.Length;
                    if (command == '1')
                    {
                        // схлапывание удвоенных
                        if (length % 2 != 0)
                        {
                            ok = false;
                            break;
                        }
                    }
                    else
                    {
                        // удвоение
                        if (length % 2 != 0)
                        {
                            ok = true;
                            break;
                        }
                    }
                }

                if (!ok)
                {
                    context.WriteAndSetFlag(node, text);

                    return;
                }

                var result = new StringBuilder(text.Length);
                navigator = new TextNavigator(text);
                while (!navigator.IsEOF)
                {
                    var chunk = navigator.ReadUntil('\\').ToString();
                    result.Append(chunk);
                    if (navigator.PeekChar() != '\\')
                    {
                        break;
                    }

                    chunk = navigator.ReadWhile('\\').ToString();
                    var length = chunk.Length;
                    if (command == '1')
                    {
                        // схлапывание удвоенных
                        length = Math.Max(length / 2, 1);
                        for (var i = 0; i < length; i++)
                        {
                            result.Append('\\');
                        }
                    }
                    else
                    {
                        // удвоение
                        for (var i = 0; i < length; i++)
                        {
                            result.Append('\\');
                            result.Append('\\');
                        }
                    }
                }

                context.WriteAndSetFlag(node, result.ToString());
            }
        }

        #endregion
    }
}
