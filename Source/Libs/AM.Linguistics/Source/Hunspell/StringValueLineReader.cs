// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StringValueLineReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Threading.Tasks;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class StringValueLineReader
    : IHunspellLineReader
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    public StringValueLineReader (string text)
    {
        content = text;
    }

    #endregion

    private readonly string content;

    private int position;

    /// <summary>
    ///
    /// </summary>
    public Encoding CurrentEncoding => Encoding.Unicode;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string? ReadLine()
    {
        if (content == null! || position >= content.Length)
        {
            return null;
        }

        var startPosition = position;

        for (; position < content.Length && !content[position].IsLineBreakChar(); ++position)
        {
            // пустое тело цикла
        }

        var result = content.Substring (startPosition, position - startPosition);

        for (; position < content.Length && content[position].IsLineBreakChar(); position++)
        {
            // пустое тело цикла
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task<string?> ReadLineAsync()
    {
        return Task.FromResult (ReadLine());
    }
}
