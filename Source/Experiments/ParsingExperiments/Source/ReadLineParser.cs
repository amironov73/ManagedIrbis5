// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ReadLineParser.cs -- считывает строку целиком
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using Pidgin;

#endregion

namespace ParsingExperiments;

sealed class ReadLineParser
    : Parser<char, string>
{
    #region Constructor

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadLineParser
        (
            bool eatEol = false
        )
    {
        _eatEOL = eatEol;
    }

    #endregion

    #region Private members

    private readonly bool _eatEOL;

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
        result = null!;
        var builder = new StringBuilder();

        if (!state.HasCurrent)
        {
            // опаньки, текст закончился, мы этого не ждали
            return false;
        }

        while (state.HasCurrent)
        {
            // state.DumpChar();
            var chr = state.Current;
            if (chr is '\r' or '\n')
            {
                break;
            }

            builder.Append (chr);
            state.Advance ();
        }

        if (_eatEOL)
        {
            state.EatChar ('\r');
            state.EatChar ('\n');
        }

        // state.DumpChar();
        result = builder.ToString();

        return true;
    }

    #endregion
}
