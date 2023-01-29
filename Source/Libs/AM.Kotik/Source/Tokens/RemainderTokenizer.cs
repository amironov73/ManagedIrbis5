// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RemainderTokenizer.cs -- выдает оставшуюся часть текста
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Выдает оставшуюся часть текста
/// (либо пустую строку, если достигнут конец текста).
/// </summary>
public sealed class RemainderTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var text = _navigator.GetRemainingText().ToString();

        return new Token ("remainder", text, line, column);
    }

    #endregion
}
