// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* IdentifierParser.cs -- парсер специально для идентификаторов Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using Pidgin;

#endregion

namespace AM.Scripting.Barsik;

/// <summary>
/// Парсер специально для идентификаторов Барсика.
/// </summary>
internal sealed class IdentifierParser
    : Parser<char, string>
{
    #region Private members

    private static readonly char[] _firstLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
          + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
          + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
          + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
          + "_$"
        ).ToCharArray();

    private static readonly char[] _secondLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
          + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
          + "0123456789"
          + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
          + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
          + "_$"
        ).ToCharArray();

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

        if (!state.HasCurrent)
        {
            // опаньки, текст закончился, а мы этого не ждали
            return false;
        }

        var chr = state.Current;
        if (Array.IndexOf (_firstLetter, chr) < 0)
        {
            // похоже, это вообще не идентификатор
            return false;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.Append (chr);

        state.Advance();
        while (state.HasCurrent)
        {
            chr = state.Current;
            if (Array.IndexOf (_secondLetter, chr) < 0)
            {
                break;
            }

            builder.Append (chr);
            state.Advance();
        }

        result = builder.ReturnShared();

        return Array.IndexOf (BarsikUtility.Keywords, result) < 0
            || Array.IndexOf (BarsikUtility.ShortTypeNames, result) >= 0;
    }

    #endregion
}
