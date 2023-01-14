// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* ParseState.cs -- хранит состояние в процессе разбора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Хранит состояние в процессе разбора.
/// </summary>
public sealed class ParseState
{
    #region Properties

    /// <summary>
    /// Разрешение на продвижение (для <see cref="OptionalParser{TResult}"/>.
    /// </summary>
    public bool EnableAdvance { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ParseState
        (
            IEnumerable<Token> tokens,
            TextWriter? traceOutput = null
        )
    {
        Sure.NotNull (tokens);

        EnableAdvance = true; // по умолчанию продвижение разрешено
        _tokens = tokens.ToArray();
        _traceOutput = traceOutput;
    }

    #endregion

    #region Private members

    private readonly Token[] _tokens;
    private readonly TextWriter? _traceOutput;

    #endregion

    #region Public methods

    /// <summary>
    /// Возвращает общее количество обработанных токенов.
    /// Проще говоря, текущее абсолютное смещение от начала
    /// входного потока.
    /// </summary>
    public int Location { get; set; }

    /// <summary>
    /// Есть текущий токен или уже достигнут конец входного потока?
    /// </summary>
    public bool HasCurrent => Location < _tokens.Length;

    /// <summary>
    /// Текущий токен.
    /// </summary>
    public Token Current => HasCurrent
        ? _tokens[Location]
        : throw new IndexOutOfRangeException();

    /// <summary>
    /// Продвижение вперед во входном потоке на указанное количество токенов.
    /// </summary>
    public bool Advance
        (
            int count = 1
        )
    {
        if (EnableAdvance)
        {
            Location += count;
        }

        EnableAdvance = true;

        return Location < _tokens.Length;
    }

    /// <summary>
    /// Заглядывание вперед на указанное количество токенов.
    /// </summary>
    public ReadOnlySpan<Token> LookAhead
        (
            int count
        )
    {
        var position = Location + count;

        return position < _tokens.Length
            ? _tokens.AsSpan (position, 1)
            : default;
    }

    /// <summary>
    /// Вывод строки отладочного текста.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public void Trace
        (
            string line
        )
    {
        _traceOutput?.WriteLine (line);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (HasCurrent)
        {
            return Current.ToString();
        }

        return "EOT";
    }

    #endregion
}
