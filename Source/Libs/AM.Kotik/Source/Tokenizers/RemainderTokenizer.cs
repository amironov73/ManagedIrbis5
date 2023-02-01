// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* RemainderTokenizer.cs -- выдает оставшуюся часть текста
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Выдает оставшуюся часть текста
/// (либо пустую строку, если достигнут конец текста).
/// </summary>
public sealed class RemainderTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        var text = navigator.GetRemainingText().ToString();

        return new Token ("remainder", text, line, column, offset);
    }

    #endregion
}
