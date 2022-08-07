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

using AM.Text;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Разбор числа с плавающей точкой.
/// Отличается от стандартного <see cref="Parser.Real"/> тем,
/// что принципиально не воспринимает целые числа.
/// Это сделано намерено для явного распознавания констант
/// типа <see cref="double"/>.
/// </summary>
internal sealed class DoubleParser
    : Parser<char, double>
{
    #region Parser members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out double result
        )
    {
        result = 0.0;

        if (!state.HasCurrent)
        {
            return false;
        }

        var builder = StringBuilderPool.Shared.Get();
        var haveDigits = false;
        var haveDotOrExponent = false;
        if (state.Current is '-' or '+')
        {
            builder.Append (state.Current);
            state.Advance ();
        }

        char current;
        while (state.HasCurrent)
        {
            current = state.Current;
            if (current is < '0' or > '9')
            {
                break;
            }

            builder.Append (current);
            haveDigits = true;
            state.Advance();
        }

        if (!state.HasCurrent)
        {
            builder.DismissShared();
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
                if (current is < '0' or > '9')
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
            if (current is 'e' or 'E')
            {
                builder.Append (current);
                haveDotOrExponent = true;
                state.Advance ();
            }

            if (!state.HasCurrent)
            {
                builder.DismissShared();
                return false;
            }

            current = state.Current;
            if (current is '+' or '-')
            {
                builder.Append (current);
                state.Advance();
            }

            if (!state.HasCurrent)
            {
                 builder.DismissShared();
                return false;
            }

            while (state.HasCurrent)
            {
                current = state.Current;
                if (current is < '0' or > '9')
                {
                    break;
                }

                builder.Append (current);
                state.Advance ();
            }
        }

        if (!haveDigits || !haveDotOrExponent)
        {
            builder.DismissShared();
            return false;
        }

        return double.TryParse
            (
                builder.ReturnShared(),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    #endregion
}
