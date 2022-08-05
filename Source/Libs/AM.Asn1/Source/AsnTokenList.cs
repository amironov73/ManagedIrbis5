// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsnTokenList.cs -- список токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
/// Список токенов.
/// </summary>
public sealed class AsnTokenList
{
    #region Properties

    /// <summary>
    /// Текущий токен.
    /// </summary>
    public AsnToken Current => _position < _tokens.Length
            ? _tokens[_position]
            : throw new AsnSyntaxException (this);

    /// <summary>
    /// Проверка, не достигнут ли конец списка.
    /// </summary>
    public bool IsEof => _position >= _tokens.Length;

    /// <summary>
    /// Общее количество токенов в списке.
    /// </summary>
    public int Length => _tokens.Length;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsnTokenList
        (
            IEnumerable<AsnToken> tokens
        )
    {
        Sure.NotNull ((object?) tokens);

        _tokens = tokens.ToArray();
        _position = 0;
    }

    #endregion

    #region Private members

    private int _position;
    private AsnToken[] _tokens;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление токена в конец списка.
    /// Крайне не рекомендуется так делать, но если очень нужно, то можно. :)
    /// </summary>
    public void Add
        (
            AsnTokenKind tokenKind
        )
    {
        Sure.Defined (tokenKind);

        var token = new AsnToken { Kind = tokenKind };
        var tokens = new List<AsnToken> (_tokens)
        {
            token
        };
        _tokens = tokens.ToArray();
    }

    /// <summary>
    /// Return number of remaining tokens.
    /// </summary>
    public int CountRemainingTokens()
    {
        var length = _tokens.Length;
        if (_position >= length)
        {
            return 0;
        }

        return _tokens.Length - _position - 1;
    }

    /// <summary>
    /// Dump token list.
    /// </summary>
    public void Dump
        (
            TextWriter writer
        )
    {
        writer.WriteLine
            (
                "Total tokens: {0}",
                _tokens.Length
            );

        foreach (var token in _tokens)
        {
            writer.WriteLine (token);
        }
    }

    /// <summary>
    /// Перемещение к следующему токену.
    /// </summary>
    public bool MoveNext()
    {
        _position++;

        var result = _position < _tokens.Length;

        if (!result)
        {
            Magna.Logger.LogTrace (nameof (AsnTokenList) + "::" + nameof (MoveNext) + ": end of list");
        }

        return result;
    }

    /// <summary>
    /// Подглядывание следующего токена.
    /// </summary>
    public AsnTokenKind Peek()
    {
        var newPosition = _position + 1;
        if (newPosition >= _tokens.Length)
        {
            Magna.Logger.LogTrace (nameof (AsnTokenList) + "::" + nameof (Peek) + "end of list");

            return AsnTokenKind.None;
        }

        return _tokens[newPosition].Kind;
    }

    /// <summary>
    /// Peek token at arbitrary position.
    /// </summary>
    public AsnTokenKind Peek
        (
            int delta
        )
    {
        var newPosition = _position + delta;
        if (newPosition < 0
            || newPosition >= _tokens.Length)
        {
            Magna.Logger.LogTrace
                (
                    "AsnTokenList::Peek: "
                    + "end of list"
                );

            return AsnTokenKind.None;
        }

        return _tokens[newPosition].Kind;
    }

    /// <summary>
    /// Require next token.
    /// </summary>
    public AsnTokenList RequireNext()
    {
        if (!MoveNext())
        {
            Magna.Logger.LogError
                (
                    "AsnTokenList::RequreNext: "
                    + "no next token"
                );

            throw new AsnSyntaxException (Current);
        }

        return this;
    }

    /// <summary>
    /// Require next token.
    /// </summary>
    public AsnTokenList RequireNext
        (
            AsnTokenKind kind
        )
    {
        RequireNext();
        if (Current.Kind != kind)
        {
            Magna.Logger.LogError
                (
                    nameof (AsnTokenList) + "::" + nameof (RequireNext)
                    + "expected {Expected}, got {Got}",
                    kind,
                    Current.Kind
                );

            throw new AsnSyntaxException (Current);
        }

        return this;
    }

    /// <summary>
    /// Move to begin of the list.
    /// </summary>
    public AsnTokenList Reset()
    {
        _position = 0;

        return this;
    }

    /// <summary>
    /// Restore position.
    /// </summary>
    public AsnTokenList RestorePosition
        (
            int position
        )
    {
        Sure.NonNegative (position, nameof (position));

        _position = position;

        return this;
    }

    /// <summary>
    /// Save position.
    /// </summary>
    public int SavePosition()
    {
        return _position;
    }

    /// <summary>
    /// Get segment (span) of the token list.
    /// </summary>
    public AsnTokenList? Segment
        (
            AsnTokenKind[] stop
        )
    {
        var savePosition = _position;
        var foundPosition = -1;

        while (!IsEof)
        {
            var current = Current.Kind;

            if (stop.Contains (current))
            {
                foundPosition = _position;
                break;
            }

            MoveNext();
        }

        if (foundPosition < 0)
        {
            _position = savePosition;

            return null;
        }

        var tokens = new List<AsnToken>();
        for (
                var position = savePosition;
                position < _position;
                position++
            )
        {
            tokens.Add (_tokens[position]);
        }

        var result = new AsnTokenList (tokens);

        return result;
    }

    /// <summary>
    /// Get segment (span) of the token list.
    /// </summary>
    internal AsnTokenList? Segment
        (
            TokenPair[] pairs,
            AsnTokenKind[] open,
            AsnTokenKind[] close,
            AsnTokenKind[] stop
        )
    {
        var savePosition = _position;
        var foundPosition = -1;

        var stack = new TokenStack (this, pairs);
        while (!IsEof)
        {
            var current = Current.Kind;

            if (open.Contains (current))
            {
                stack.Push (current);
            }
            else if (close.Contains (current))
            {
                if (stack.Count == 0)
                {
                    if (stop.Contains (current))
                    {
                        foundPosition = _position;
                        break;
                    }
                }

                stack.Pop (current);
            }
            else if (stop.Contains (current))
            {
                if (stack.Count == 0)
                {
                    foundPosition = _position;
                    break;
                }
            }

            MoveNext();
        }

        stack.Verify();
        if (foundPosition < 0)
        {
            Magna.Logger.LogDebug (nameof (AsnTokenList) + "::" + nameof (Segment) + ": not found");

            _position = savePosition;

            return null;
        }

        var tokens = new List<AsnToken>();
        for (
                var position = savePosition;
                position < _position;
                position++
            )
        {
            tokens.Add (_tokens[position]);
        }

        var result = new AsnTokenList (tokens);

        return result;
    }

    /// <summary>
    /// Get segment (span) of the token list.
    /// </summary>
    public AsnTokenList? Segment
        (
            AsnTokenKind[] open,
            AsnTokenKind[] close,
            AsnTokenKind[] stop
        )
    {
        var savePosition = _position;
        var foundPosition = -1;

        var level = 0;
        while (!IsEof)
        {
            var current = Current.Kind;

            if (open.Contains (current))
            {
                level++;
            }
            else if (close.Contains (current))
            {
                if (level == 0)
                {
                    if (stop.Contains (current))
                    {
                        foundPosition = _position;
                        break;
                    }
                }

                level--;
            }
            else if (stop.Contains (current))
            {
                if (level == 0)
                {
                    foundPosition = _position;
                    break;
                }
            }

            MoveNext();
        }

        if (level != 0)
        {
            Magna.Logger.LogError
                (
                    nameof (AsnTokenList) + "::" + nameof (Segment)
                    + ": unbalanced {Level}",
                    level
                );

            throw new AsnSyntaxException (this);
        }

        if (foundPosition < 0)
        {
            Magna.Logger.LogTrace (nameof (AsnTokenList) + "::" + nameof (Segment) + ": not found");
            _position = savePosition;

            return null;
        }

        var tokens = new List<AsnToken>();
        for (
                var position = savePosition;
                position < _position;
                position++
            )
        {
            tokens.Add (_tokens[position]);
        }

        var result = new AsnTokenList (tokens);

        return result;
    }

    /// <summary>
    /// Показ последних (по времени) токенов.
    /// </summary>
    public string ShowLastTokens
        (
            int howMany
        )
    {
        var builder = StringBuilderPool.Shared.Get();
        var index = _position - howMany;
        if (index < 0)
        {
            index = 0;
        }

        var first = true;
        while (index < Length)
        {
            if (!first)
            {
                builder.Append (' ');
            }

            builder.Append (_tokens[index]);

            index++;
            first = false;
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Выдача всех токенов в виде массива.
    /// </summary>
    public AsnToken[] ToArray()
    {
        return _tokens;
    }

    /// <summary>
    /// Convert token list to text.
    /// </summary>
    public string ToText()
    {
        var builder = StringBuilderPool.Shared.Get();

        foreach (var token in _tokens)
        {
            builder.Append (token.Text);
        }

        return builder.ReturnShared();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return IsEof
            ? "(EOF)"
            : $"{_position} of {_tokens.Length}: {Current}";
    }

    #endregion
}
