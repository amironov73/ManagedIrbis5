// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* DoubleParser.cs -- разбор числа с плавающей точкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Text;

using Pidgin;

#endregion

namespace ParsingExperiments;

sealed class DoubleParser
    : Parser<char, double>
{
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out double result
        )
    {
        result = 0.0;
        var builder = new StringBuilder();

        if (!state.HasCurrent)
        {
            return false;
        }

        var haveDigits = false;
        var haveDotOrExponent = false;
        if (state.Current == '-')
        {
            builder.Append ('-');
            state.Advance ();
        }

        char current;
        while (state.HasCurrent)
        {
            current = state.Current;
            if (current < '0' || current > '9')
            {
                break;
            }

            builder.Append (current);
            haveDigits = true;
            state.Advance();
        }

        if (!state.HasCurrent)
        {
            return false;
        }

        if (state.Current == '.')
        {
            haveDotOrExponent = true;
            builder.Append ('.');
            state.Advance ();

            while (state.HasCurrent)
            {
                current = state.Current;
                if (current < '0' || current > '9')
                {
                    break;
                }

                builder.Append (current);
                haveDigits = true;
                state.Advance();
            }
        }

        if (state.HasCurrent)
        {
            current = state.Current;
            if (current == 'e' || current == 'E')
            {
                builder.Append (current);
                haveDotOrExponent = true;
                state.Advance ();
            }

            if (!state.HasCurrent)
            {
                return false;
            }

            current = state.Current;
            if (current == '+' || current == '-')
            {
                builder.Append (current);
                state.Advance();
            }

            if (!state.HasCurrent)
            {
                return false;
            }

            while (state.HasCurrent)
            {
                current = state.Current;
                if (current < '0' || current > '9')
                {
                    break;
                }

                builder.Append (current);
                state.Advance ();
            }
        }

        if (!haveDigits || !haveDotOrExponent)
        {
            return false;
        }

        return double.TryParse (builder.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
}
