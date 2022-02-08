// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StrictParser.cs -- парсер, парсящий строго токены
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Парсер, парсящий строго токены в понимании Барсика.
/// Очередная попытка. :)
/// </summary>
public sealed class StrictParser
    : Parser<char, string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="expected">Ожидаемый токен.</param>
    /// <param name="unexpected">Неожиданные символы (от токена,
    /// начинающегося точно также).
    /// Пример: "+" и "++".
    /// </param>
    public StrictParser
        (
            string expected,
            string? unexpected = null
        )
    {
        Sure.NotNullNorEmpty (expected);

        _expected = expected;
        _unexpected = unexpected;
    }

    #endregion

    #region Private members

    private readonly string _expected;
    private readonly string? _unexpected;

    private static bool IsIdentifier
        (
            char chr
        )
    {
        return chr is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')
        or (>= 'А' and <= 'Я') or (>= 'а' and <= 'я') or (>= '0' and <= '9')
        or '_';
    }

    #endregion

    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out string result
        )
    {
        result = default!;

        var lenght = _expected.Length;
        for (var i = 0; i < lenght; i++)
        {
            if (!state.HasCurrent)
            {
                // опаньки, текст закончился, а мы этого не ждали
                return false;
            }

            var chr = state.Current;
            if (chr != _expected[i])
            {
                return false;
            }

            state.Advance();
        }

        // мы имеем точное совпадение с искомым
        // надо бы проверить, что нет ничего лишнего

        if (!state.HasCurrent)
        {
            result = _expected;
            return true;
        }

        if (IsIdentifier (_expected[0]))
        {
            // если мы разбираем нечто вроде идентификатора,
            // то дальше не должно быть ничего похожего на идентификикатор
            if (!IsIdentifier (state.Current))
            {
                result = _expected;
                return true;
            }
        }
        else
        {
            // иначе проверяем на неожиданные символы
            if (string.IsNullOrEmpty (_unexpected)
                || !_unexpected.Contains (state.Current))
            {
                result = _expected;
                return true;
            }
        }

        return false;
    }

    #endregion
}
