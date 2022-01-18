// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Threading.Tasks;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class StringValueLineReader : IHunspellLineReader
{
    public StringValueLineReader (string text)
    {
        content = text;
    }

    private readonly string content;

    private int position = 0;

    public Encoding CurrentEncoding => Encoding.Unicode;

    public string ReadLine()
    {
        if (content == null || position >= content.Length) return null;

        var startPosition = position;

        for (; position < content.Length && !content[position].IsLineBreakChar(); ++position) ;

        var result = content.Substring (startPosition, position - startPosition);

        for (; position < content.Length && content[position].IsLineBreakChar(); position++) ;

        return result;
    }

    public Task<string> ReadLineAsync()
    {
        return Task.FromResult (ReadLine());
    }
}
