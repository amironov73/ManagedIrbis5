// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RemainderParser.cs -- парсер, выдающий остаток текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using Pidgin;

#endregion

namespace AM.Scripting;

/// <summary>
/// Парсер, выдающий нераспарсенный остаток текста.
/// После этого нераспарсенного текста не остается.
/// </summary>
internal sealed class RemainderParser
    : Parser<char, string>
{
    #region Parser<TToken, TResult> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<char> state,
            ref PooledList<Expected<char>> expecteds,
            out string result
        )
    {
        var builder = new StringBuilder();
        while (state.HasCurrent)
        {
            builder.Append (state.ReadChar());
        }

        result = builder.ToString();

        return true;
    }

    #endregion
}
