// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RememberParser.cs -- парсер, запоминающий факт удачного выполнения вложенного парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер, запоминающий в <see cref="ParseState"/> факт удачного
/// выполнения вложенного парсера.
/// </summary>
[PublicAPI]
public sealed class RememberParser<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RememberParser
        (
            string key,
            IParser<TResult> inner
        )
    {
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (inner);

        _key = key;
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly string _key;
    private readonly IParser<TResult> _inner;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        // предполагается, что все необходимые
        // телодвижения насчет продвижения/сохранения
        // состояния и отладки проделывает
        // вложенный парсер

        if (_inner.TryParse (state, out result))
        {
            state.UserData[_key] = result;
            return true;
        }

        return false;
    }

    #endregion
}
