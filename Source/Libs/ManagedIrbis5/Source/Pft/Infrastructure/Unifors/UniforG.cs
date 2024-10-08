﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforG.cs -- вернуть часть строки до или начиная с заданного символа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors;

//
// Вернуть часть строки до или начиная с заданного символа – &uf('G')
// Вид функции: G.
// Назначение: Вернуть часть строки до или начиная с заданного символа.
// Формат (передаваемая строка):
// GNA<строка>
// где:
// N может принимать значения:
// 0 (или A) – до заданного символа не включая его;
// 1 (или B) – начиная с заданного символа;
// 2 (или C) – после заданного символа;
// 3 (или D) – после последнего вхождения заданного символа;
// 4 (или E) – до последнего вхождения заданного символа (включая его);
// 5 – до последнего вхождения заданного символа (не включая его).
// А – заданный символ.Символ обозначает самого себя,
// кроме # (обозначает любую цифру) и $ (обозначает любую букву).
// Примечание: функция G5 присутствует в версиях ИРБИС с 2015.1.
//
// Примеры:
//
// &unifor("G0#"v700)
// &unifor("G1-"v700^a)
// &unifor("G2-"v700^a)
//
// Пример получения ссылки на файл из подполя 952^U
// полнотекстовой БД для файлов, добавленных с разбиением и без
// &uf('G0:',&uf('G4:',&uf('G2:',&uf('G2:', v952^U))))
//

internal static class UniforG
{
    #region Public methods

    /// <summary>
    /// Вернуть часть строки до или начиная с заданного символа.
    /// </summary>
    public static void GetPart
        (
            PftContext context,
            PftNode? node,
            string? expression
        )
    {
        Sure.NotNull (context);

        if (!string.IsNullOrEmpty (expression))
        {
            var navigator = new TextNavigator (expression);
            var code = navigator.ReadChar();
            var symbol = navigator.ReadChar();
            var text = navigator.GetRemainingText().ToString();
            if (!string.IsNullOrEmpty (text))
            {
                int firstOffset, lastOffset;
                string? output = null;

                switch (symbol)
                {
                    case '#':
                        firstOffset = text.IndexOfAny (PftUtility.Digits);
                        lastOffset = text.LastIndexOfAny (PftUtility.Digits);
                        break;

                    case '$':
                        firstOffset = text.IndexOfAny (PftUtility.Letters);
                        lastOffset = text.LastIndexOfAny (PftUtility.Letters);
                        break;

                    default:
                        firstOffset = text.IndexOf (symbol);
                        lastOffset = text.LastIndexOf (symbol);
                        break;
                }

                switch (code)
                {
                    case '0':
                    case 'A':
                    case 'a':
                        output = firstOffset < 0
                            ? text
                            : text.Substring (0, firstOffset);
                        break;

                    case '1':
                    case 'B':
                    case 'b':
                        output = firstOffset < 0
                            ? text
                            : text.Substring (firstOffset);
                        break;

                    case '2':
                    case 'C':
                    case 'c':
                        output = firstOffset < 0
                            ? text
                            : text.Substring (firstOffset + 1);
                        break;

                    case '3':
                    case 'D':
                    case 'd':

                        // для тройки ИРБИС почему-то буквально
                        // трактует решетку и доллар
                        if (symbol == '#' || symbol == '$')
                        {
                            lastOffset = text.LastIndexOf (symbol);
                        }

                        output = lastOffset < 0
                            ? text
                            : text.Substring (lastOffset + 1);
                        break;

                    case '4':
                    case 'E':
                    case 'e':

                        // для четверки ИРБИС почему-то буквально
                        // трактует решетку и доллар
                        if (symbol == '#' || symbol == '$')
                        {
                            lastOffset = text.LastIndexOf (symbol);
                            output = lastOffset < 0
                                ? string.Empty
                                : text.Substring (0, lastOffset + 1);
                        }
                        else
                        {
                            output = lastOffset < 0
                                ? text
                                : text.Substring (0, lastOffset + 1);
                        }

                        break;

                    case '5':
                    case 'F':
                    case 'f':

                        // для пятерки ИРБИС почему-то буквально
                        // трактует решетку и доллар
                        if (symbol == '#' || symbol == '$')
                        {
                            lastOffset = text.LastIndexOf (symbol);
                            output = lastOffset < 0
                                ? string.Empty
                                : text.Substring (0, lastOffset + 1);
                        }
                        else
                        {
                            output = lastOffset < 0
                                ? text
                                : text.Substring (0, lastOffset);
                        }

                        break;

                    default:
                        Magna.Logger.LogError
                            (
                                nameof (UniforG) + "::" + nameof (GetPart)
                                + ": unexpected code {Code}",
                                code
                            );
                        break;
                }

                context.WriteAndSetFlag (node, output);
            }
        }
    }

    #endregion
}
