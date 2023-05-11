// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SourcePositionParser.cs -- выдает текущую позицию в исходном коде скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Выдает текущую позицию в исходном коде скрипта.
/// </summary>
[PublicAPI]
public sealed class SourcePositionParser
    : Parser<SourcePosition>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out SourcePosition result
        )
    {
        // продвижения по исходному тексту не происходит!

        var line = -1;
        var column = -1;
        if (state.HasCurrent)
        {
            var current = state.Current;
            line = current.Line;
            column = 1;
        }

        result = new SourcePosition (line, column);

        return true;
    }

    #endregion
}
