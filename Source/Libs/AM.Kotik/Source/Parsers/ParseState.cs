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
    /// Поток для отладочной печати.
    /// </summary>
    public TextWriter? DebugOutput { get; set; }

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
    /// Продвижение вперед во входном потоке на указанное количество токенов.
    /// </summary>
    public bool Advance
        (
            int count = 1
        )
    {
        Location += count;

        return Location < _tokens.Length;
    }

    /// <summary>
    /// Отладочный вывод текущей позиции в исходном коде скрипта.
    /// </summary>
    public void DebugCurrentPosition
        (
            object parser
        )
    {
        if (DebugOutput is not null)
        {
            var current = HasCurrent ? Current.ToString() : "EOT";

            DebugOutput.WriteLine ($"{parser}: {current} <<");
        }
    }

    /// <summary>
    /// Отладочный вывод признака успешности выполнения парсинга.
    /// </summary>
    public bool DebugSuccess
        (
            object parser,
            bool success
        )
    {
        if (DebugOutput is not null)
        {
            var current = HasCurrent ? Current.ToString() : "EOT";

            DebugOutput.WriteLine ($"{parser}: {current} >> {success}");
        }

        return success;
    }

    /// <summary>
    /// Заглядывание вперед на указанное количество токенов.
    /// </summary>
    public Token? LookAhead
        (
            int count
        )
    {
        var position = Location + count;

        return position < _tokens.Length
            ? _tokens[position]
            : null;
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
    public override string ToString() => HasCurrent ? Current.ToString() : "EOT";

    #endregion
}
