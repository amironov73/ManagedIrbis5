// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RememberParser.cs -- парсер, запоминающий факт удачного выполнения вложенного парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik.Parsers;

/// <summary>
/// Парсер, запоминающий в <see cref="ParseState"/> факт удачного
/// выполнения вложенного парсера.
/// </summary>
public sealed class RememberParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RememberParser
        (
            string key, 
            Parser<TResult> inner
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
    private readonly Parser<TResult> _inner;

    #endregion

    #region Parset<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse 
        (
            ParseState state, 
            [MaybeNullWhen (false)] out TResult result
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
