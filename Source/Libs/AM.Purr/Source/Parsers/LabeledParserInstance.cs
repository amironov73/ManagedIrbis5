// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LabeledParserInstance.cs -- помеченный экземпляр парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Помеченный экземпляр парсера.
/// Предназначен для облегчения отладки.
/// </summary>
[PublicAPI]
public sealed class LabeledParserInstance<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LabeledParserInstance
        (
            Parser<TResult> inner,
            string label
        )
    {
        Sure.NotNull (inner);
        Sure.NotNullNorEmpty (label);

        _inner = inner;
        Label = label;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _inner;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        return _inner.TryParse (state, out result);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() => Label!;

    #endregion
}
