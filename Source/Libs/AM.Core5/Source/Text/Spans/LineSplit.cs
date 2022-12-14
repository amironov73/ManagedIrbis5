// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* LineSplit.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text.Spans;

/// <summary>
///
/// </summary>
public static class StringSpanLineSplitExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static LineSplitEnumerator SplitLines
        (
            this string str
        )
    {
        return new LineSplitEnumerator (str.AsSpan(), StringSplitOptions.None);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static LineSplitEnumerator SplitLines
        (
            this string str,
            StringSplitOptions options
        )
    {
        return new LineSplitEnumerator (str.AsSpan(), options);
    }

    /// <summary>
    ///
    /// </summary>
    // Must be a ref struct as it contains a ReadOnlySpan<char>
    public ref struct LineSplitEnumerator
    {
        private ReadOnlySpan<char> _str;
        private readonly StringSplitOptions _options;

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="options"></param>
        public LineSplitEnumerator
            (
                ReadOnlySpan<char> str,
                StringSplitOptions options
            )
        {
            _str = str;
            _options = options;
            Current = default;
        }

        /// <summary>
        /// foreach compatible
        /// </summary>
        public LineSplitEnumerator GetEnumerator() => this;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            var span = _str;
            if (span.Length == 0) // Reach the end of the string
                return false;

            var index = span.IndexOfAny ('\r', '\n');

            // "HIJKLMN"
            if (index == -1)
            {
                // single line
                _str = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
                Current = new LineSplitEntry (span, ReadOnlySpan<char>.Empty);
                return true;
            }

            // "ABCDEFG\r\nHIJKLMN"
            if (index < span.Length - 1 && span[index] == '\r')
            {
                // Try to consume the '\n' associated to the '\r'
                var next = span[index + 1];
                if (next == '\n')
                {
                    Current = new LineSplitEntry (span.Slice (0, index), span.Slice (index, 2));
                    _str = span.Slice (index + 2);
                    return true;
                }
            }

            if (_options == StringSplitOptions.RemoveEmptyEntries && index < span.Length - 1 && span[index] == '\n')
            {
                Current = new LineSplitEntry (span.Slice (1, index == 0 ? 0 : index - 1), span.Slice (index, 1));
                _str = span.Slice (index + 1);
                MoveNext();
                return true;
            }

            Current = new LineSplitEntry (span.Slice (0, index), span.Slice (index, 1));
            _str = span.Slice (index + 1);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public LineSplitEntry Current { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    public readonly ref struct LineSplitEntry
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        public LineSplitEntry (ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
        {
            Line = line;
            Separator = separator;
        }

        /// <summary>
        ///
        /// </summary>
        public ReadOnlySpan<char> Line { get; }

        /// <summary>
        ///
        /// </summary>
        public ReadOnlySpan<char> Separator { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        public void Deconstruct (out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
        {
            line = Line;
            separator = Separator;
        }

        // implicit cast to ReadOnlySpan<char>
        // foreach (ReadOnlySpan<char> entry in str.SplitLines())
        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public static implicit operator ReadOnlySpan<char> (LineSplitEntry entry) => entry.Line;
    }
}
