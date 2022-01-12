// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LineNumberFormatting.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Fctb;

/// <summary>
/// Customize how we display the line numbers
/// </summary>
public abstract class LineNumberFormatting
{
    /// <summary>
    /// Defines how line number is displayed
    /// </summary>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public abstract string FromLineNumberToString (int lineNumber);

    /// <summary>
    /// Recover the line number from the formatted string
    /// </summary>
    /// <param name="lineNumber"></param>
    /// <param name="parsedLineNumber"></param>
    /// <returns></returns>
    public abstract bool TryParseStringIntoLineNumber (string lineNumber, out int parsedLineNumber);
}
