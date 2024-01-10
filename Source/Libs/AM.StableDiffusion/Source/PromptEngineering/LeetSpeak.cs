// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LeetSpeak.cs -- транслитерация в leet
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;
using AM.Text.Searching;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/*
    https://ru.wikipedia.org/wiki/Leet

    Leet (стилизуется как 1337, а также означает «elite») — распространившийся
    в Интернете стиль применения английского языка. Основные отличия — замена
    латинских букв на похожие цифры и символы, имитация и пародия на ошибки,
    свойственные для быстрого набора текста, имитация жаргона хакеров и геймеров,
    добавление окончания z0r к словам. Используется в основном в письменном виде,
    некоторые слова не имеют определённого произношения, например, слово pwn.

 */

/// <summary>
/// Транслитерация в leet.
/// Полезна для именования LORA и embedding.
/// </summary>
[PublicAPI]
public static class LeetSpeak
{
    #region Private members

    // подставляемые вместо определенных букв цифры
    private static Dictionary<char, char> _substitutes
        = new (CharComparer.InvariantCultureIgnoreCase)
    {
        ['a'] = '4',
        ['b'] = '8',
        ['d'] = '2',
        ['e'] = '3',
        ['f'] = '7',
        ['g'] = '9',
        ['h'] = '6',
        ['i'] = '1',
        ['l'] = '1',
        ['o'] = '0',
        ['s'] = '5',
        ['t'] = '7',
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Транслитерация текста в leet.
    /// </summary>
    public static string Translate
        (
            IEnumerable<char> text
        )
    {
        Sure.NotNull (text);

        var first = true;
        var result = StringBuilderPool.Shared.Get();
        foreach (var chr in text)
        {
            if (chr is ' ')
            {
                // пробелы удаляем
                continue;
            }

            if (first)
            {
                // первый символ всегда оставляем как есть
                result.Append (chr);
                first = false;
                continue;
            }

            result.Append
                (
                    _substitutes.GetValueOrDefault (chr, chr)
                );
        }

        return result.ReturnShared();
    }

    #endregion
}
