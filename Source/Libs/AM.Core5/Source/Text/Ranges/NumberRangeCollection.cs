// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* NumberRangeCollection.cs -- набор диапазонов чисел.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Text.Ranges;

/// <summary>
/// Набор диапазонов чисел.
/// </summary>
[DebuggerDisplay ("Count={Count}")]
public sealed class NumberRangeCollection
    : IEnumerable<NumberText>,
    IHandmadeSerializable
{
    #region Constants

    /// <summary>
    /// Разделитель по умолчанию.
    /// </summary>
    public const string DefaultDelimiter = ",";

    #endregion

    #region Properties

    /// <summary>
    /// Gets the collection item count.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Разделитель диапазонов.
    /// </summary>
    public string Delimiter { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NumberRangeCollection()
    {
        Delimiter = DefaultDelimiter;
        _items = new List<NumberRange>();
    }

    #endregion

    #region Private members

    private readonly List<NumberRange> _items;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление диапазона в набор.
    /// </summary>
    public NumberRangeCollection Add
        (
            NumberRange range
        )
    {
        _items.Add (range);

        return this;
    }

    /// <summary>
    /// Добавление диапазона в набор.
    /// </summary>
    public NumberRangeCollection Add
        (
            string start,
            string stop
        )
    {
        var result = Add
            (
                new NumberRange (start, stop)
            );

        return result;
    }

    /// <summary>
    /// Добавление диапазона в набор.
    /// </summary>
    public NumberRangeCollection Add
        (
            string startAndStop
        )
    {
        var result = Add
            (
                new NumberRange (startAndStop)
            );

        return result;
    }

    /// <summary>
    /// Проверка, содержит ли набор указанное число.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool Contains
        (
            NumberText number
        )
    {
        var result = _items.Any
            (
                item => item.Contains (number)
            );

        return result;
    }

    /// <summary>
    /// Parse the text representation
    /// </summary>
    public static NumberRangeCollection Parse
        (
            string text
        )
    {
        var navigator = new TextNavigator (text);
        navigator.SkipWhile (NumberRange.Delimiters);
        if (navigator.IsEOF)
        {
            Magna.Logger.LogError
                (
                    nameof (NumberRangeCollection) + "::" + nameof (Parse)
                    + ": unexpected end of stream"
                );

            throw new FormatException();
        }

        var result = new NumberRangeCollection();

        while (true)
        {
            navigator.SkipWhile (NumberRange.Delimiters);
            if (navigator.IsEOF)
            {
                break;
            }

            var start = navigator
                .ReadUntil (NumberRange.DelimitersOrMinus).ToString();
            NumberRange range;
            if (string.IsNullOrEmpty (start))
            {
                Magna.Logger.LogError
                    (
                        nameof (NumberRangeCollection) + "::" + nameof (Parse)
                        + ": empty Start clause"
                    );

                throw new FormatException();
            }

            navigator.SkipWhitespace();
            if (navigator.PeekChar() == '-')
            {
                navigator.ReadChar();
                navigator.SkipWhitespace();
                var stop = navigator
                    .ReadUntil (NumberRange.Delimiters).ToString();
                if (string.IsNullOrEmpty (stop))
                {
                    Magna.Logger.LogError
                        (
                            nameof (NumberRangeCollection) + "::" + nameof (Parse)
                            + ": empty Stop clause"
                        );

                    throw new FormatException();
                }

                range = new NumberRange (start, stop);
            }
            else
            {
                range = new NumberRange (start);
            }

            result.Add (range);
        }

        return result;
    }

    /// <summary>
    /// Кумуляция (сжатие).
    /// </summary>
    public static NumberRangeCollection Cumulate
        (
            List<NumberText> numbers
        )
    {
        var result = new NumberRangeCollection();

        if (numbers.Count != 0)
        {
            numbers.Sort();

            var previous = numbers[0];
            var last = previous.Clone();
            for (var i = 1; i < numbers.Count; i++)
            {
                var current = numbers[i];
                var next = last + 1;
                if (current != next)
                {
                    result.Add
                        (
                            new NumberRange
                                (
                                    previous,
                                    last
                                )
                        );
                    previous = current.Clone();
                }

                last = current;
            }

            result.Add
                (
                    new NumberRange
                        (
                            previous,
                            last
                        )
                );
        }

        return result;
    }

    /// <summary>
    /// Кумуляция (сжатие).
    /// </summary>
    public static NumberRangeCollection Cumulate
        (
            IEnumerable<string> texts
        )
    {
        var numbers = texts
            .Select (text => new NumberText (text))
            .ToList();

        return Cumulate (numbers);
    }

    /// <summary>
    /// Выполнение указанного действия
    /// на всех диапазонах набора.
    /// </summary>
    /// <param name="action"></param>
    public void For
        (
            Action<NumberText> action
        )
    {
        foreach (var range in _items)
        {
            foreach (var number in range)
            {
                action
                    (
                        number
                    );
            }
        }
    }

    #endregion

    #region IEnumerable<NumberText> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<NumberText> GetEnumerator()
    {
        foreach (var range in _items)
        {
            foreach (var number in range)
            {
                yield return number;
            }
        }
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        _items.Clear();
        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var item = new NumberRange();
            item.RestoreFromStream (reader);
            _items.Add (item);
        }
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WritePackedInt32 (_items.Count);
        for (var i = 0; i < _items.Count; i++)
        {
            _items[i].SaveToStream (writer);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        var first = true;
        foreach (var item in _items)
        {
            var text = item.ToString();
            if (!string.IsNullOrEmpty (text))
            {
                if (!first)
                {
                    builder.Append (Delimiter);
                }

                builder.Append (text);
                first = false;
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
