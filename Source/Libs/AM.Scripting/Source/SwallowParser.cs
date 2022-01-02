// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SwallowParser.cs -- парсер, съедающий комментарии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Pidgin;

#endregion

namespace AM.Scripting;

/// <summary>
/// Парсер, съедающий комментарии в стиле C/C++.
/// Заодно съедаются пробелы и обнаруживаются
/// разделители стейтментов.
/// Короче, парсер строго для Барсика :)
/// </summary>
internal sealed class SwallowParser
    : Parser<char, Unit>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SwallowParser
        (
            params char[] delimiters
        )
    {
        _delimiters = delimiters;
    }

    #endregion

    #region Private members

    private readonly char[] _delimiters;

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out Unit result
        )
    {
        result = Unit.Value;

        if (!state.HasCurrent)
        {
            // опаньки, текст закончился, а мы этого не ждали
            return false;
        }

        while (state.HasCurrent)
        {
            // state.DumpChar();
            var chr = state.Current;

            // сначала пропускаем все пробелы
            if (char.IsWhiteSpace (chr))
            {
                state.Advance ();
                continue;
            }

            if (Array.IndexOf (_delimiters, chr) >= 0)
            {
                // мы нарвались на разделитель стейтментов
                state.Advance ();
                return true;
            }

            if (chr == '/')
            {
                // Возможно, это начало комментария

                state.PushBookmark();
                state.ReadChar();
                chr = state.ReadChar();
                if (chr == '/')
                {
                    // это однострочный комментарий
                    state.PopBookmark();

                    while (state.HasCurrent)
                    {
                        if (state.Current is '\r' or '\n')
                        {
                            break;
                        }

                        state.Advance();
                    }

                    state.EatChar ('\r');
                    state.EatChar ('\n');
                }
                else if (chr == '*')
                {
                    // это многострочный комментарий
                    state.PopBookmark();

                    var found = false;
                    while (state.HasCurrent)
                    {
                        chr = state.ReadChar();
                        if (chr == '*')
                        {
                            // возможно, это конец комментария
                            chr = state.ReadChar();
                            if (chr == '/')
                            {
                                // точно, это конец комментария
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        // комментарий без конца!
                        return false;
                    }

                    // не надо делать break, т. к. может быть еще один комментарий
                }
                else
                {
                    // это просто слэш, выходим из цикла
                    state.Rewind();
                    break;
                }
            }
            else
            {
                // это какой-то непредвиденный символ,
                // пусть с ним разбираются другие парсеры
                break;
            }
        }

        return true;
    }

    #endregion
}
