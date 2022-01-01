// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* NumberRange.cs --range of numbers containing non-numeric fragments
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM.Runtime;

#endregion

#nullable enable

namespace AM.Text.Ranges;

/// <summary>
/// Range of numbers containing non-numeric fragments.
/// </summary>
[DebuggerDisplay ("{" + nameof (Start) + "} - {" + nameof (Stop) + "}")]
public sealed class NumberRange
    : IEnumerable<NumberText>,
    IHandmadeSerializable,
    IEquatable<NumberRange>,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Delimiters.
    /// </summary>
    public static char[] Delimiters { get; } = { ' ', '\t', '\r', '\n', ',', ';' };

    /// <summary>
    /// Delimiters or minus sign.
    /// </summary>
    public static char[] DelimitersOrMinus { get; } = { ' ', '\t', '\r', '\n', ',', ';', '-' };

    /// <summary>
    /// Start value.
    /// </summary>
    public NumberText? Start { get; set; }

    /// <summary>
    /// Stop value. Can coincide
    /// with <see cref="Start"/> value.
    /// </summary>
    public NumberText? Stop { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Default constructor.
    /// </summary>
    public NumberRange()
    {
    }

    /// <summary>
    /// Конструктор для диапазона, состоящего
    /// из одного числа.
    /// </summary>
    public NumberRange
        (
            NumberText startAndStop
        )
    {
        Start = startAndStop;
        Stop = startAndStop;
    }

    /// <summary>
    /// Конструктор для произвольного диапазона.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="stop"></param>
    public NumberRange
        (
            NumberText start,
            NumberText stop
        )
    {
        Start = start;
        Stop = stop;
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, содержит ли диапазон указанное значение.
    /// </summary>
    public bool Contains
        (
            NumberText number
        )
    {
        Start = Start.ThrowIfNull();
        Stop = Stop.ThrowIfNull();

        return Start.CompareTo (number) <= 0
               && number.CompareTo (Stop) <= 0;
    }

    /// <summary>
    /// Parse text representation.
    /// </summary>
    public static NumberRange Parse
        (
            string text
        )
    {
        var navigator = new ValueTextNavigator (text);

        navigator.SkipWhile (Delimiters);
        if (navigator.IsEOF)
        {
            Magna.Error
                (
                    nameof (NumberRange) + "::" + nameof (Parse)
                    + ": unexpected end of text"
                );

            throw new FormatException();
        }

        NumberRange result;
        var start = navigator.ReadUntil (DelimitersOrMinus).ToString();
        if (string.IsNullOrEmpty (start))
        {
            Magna.Error
                (
                    nameof (NumberRange) + "::" + nameof (Parse)
                    + ": start sequence not found"
                );

            throw new FormatException();
        }

        navigator.SkipWhitespace();
        if (navigator.PeekChar() == '-')
        {
            navigator.ReadChar();
            navigator.SkipWhitespace();
            string stop = navigator.ReadUntil (DelimitersOrMinus).ToString();
            if (string.IsNullOrEmpty (stop))
            {
                Magna.Error
                    (
                        nameof (NumberRange) + "::" + nameof (Parse)
                        + ": stop sequence not found"
                    );

                throw new FormatException();
            }

            result = new NumberRange (start, stop);
        } // if

        else
        {
            result = new NumberRange (start);
        }

        navigator.SkipWhile (Delimiters);

        if (!navigator.IsEOF)
        {
            Magna.Error
                (
                    nameof (NumberRange) + "::" + nameof (Parse)
                    + ": garbage behind the range"
                );

            throw new FormatException();
        }

        return result;
    }

    /// <summary>
    /// Выполнение указанного действия на всём диапазоне.
    /// </summary>
    public void For
        (
            Action<NumberText> action
        )
    {
        Start = Start.ThrowIfNull();
        Stop = Stop.ThrowIfNull();

        for (
                NumberText current = Start;
                current.CompareTo (Stop) <= 0;
                current = current.Increment()
            )
        {
            action
                (
                    current
                );
        }
    }

    /// <summary>
    /// Пересечение двух диапазонов.
    /// </summary>
    public NumberRange Intersect
        (
            NumberRange other
        )
    {
        // coverity[SWAPPED_ARGUMENTS]
        return new NumberRange
            (
                Stop.ThrowIfNull(),
                other.Start.ThrowIfNull()
            );
    }

    /// <summary>
    /// Проверка, не пустой ли диапазон.
    /// </summary>
    public bool IsEmpty() => Start > Stop;

    /// <summary>
    /// Объединение двух диапазонов.
    /// </summary>
    public NumberRange Union
        (
            NumberRange other
        )
    {
        return new NumberRange
            (
                NumberText.Min
                    (
                        Start.ThrowIfNull(),
                        other.Start.ThrowIfNull()
                    ),
                NumberText.Max
                    (
                        Stop.ThrowIfNull(),
                        other.Stop.ThrowIfNull()
                    )
            );
    }

    #endregion

    #region IEnumerable<NumberText> members

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through
    /// the collection.
    /// </summary>
    public IEnumerator<NumberText> GetEnumerator()
    {
        Start = Start.ThrowIfNull();
        Stop = Stop.ThrowIfNull();

        for (
                NumberText current = Start!;
                current.CompareTo (Stop) <= 0;
                current = current.Clone().Increment()
            )
        {
            yield return current;
        }
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Start = reader.RestoreNullable<NumberText>();
        Stop = reader.RestoreNullable<NumberText>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Start)
            .WriteNullable (Stop);
    }

    #endregion

    #region IEquatable members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals
        (
            NumberRange? other
        )
    {
        if (ReferenceEquals (Start, null)
            || ReferenceEquals (Stop, null))
        {
            return false;
        }

        other = other.ThrowIfNull();

        bool result = Start.Equals (other.Start)
                      && Stop.Equals (other.Stop);

        return result;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<NumberRange> (this, throwOnError);

        verifier
            .NotNull (Start)
            .NotNull (Stop);

        if (verifier.Result)
        {
            verifier
                .VerifySubObject (Start!)
                .VerifySubObject (Stop!)
                .Assert (Start!.CompareTo (Stop) <= 0);
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        var result = 0;

        // ReSharper disable NonReadonlyMemberInGetHashCode
        if (!ReferenceEquals (Start, null))
        {
            result = Start.GetHashCode();
        }

        if (!ReferenceEquals (Stop, null))
        {
            result = result * 137 + Stop.GetHashCode();
        }

        // ReSharper restore NonReadonlyMemberInGetHashCode

        return result;
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (Start is null && Stop is null)
        {
            return string.Empty;
        }

        if (Start is null)
        {
            return Stop!.ToString();
        }

        if (Stop is null)
        {
            return Start!.ToString();
        }

        if (Start.CompareTo (Stop) == 0)
        {
            return Start.ToString();
        }

        return $"{Start}-{Stop}";
    }

    #endregion
}
