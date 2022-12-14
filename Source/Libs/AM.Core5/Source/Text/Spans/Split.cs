// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* Split.cs --
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
public static class StringSpanSplitExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static SplitEnumerator SplitSpan (this string str, char separator)
    {
        return new SplitEnumerator (str.AsSpan(), separator);
    }

    // Must be a ref struct as it contains a ReadOnlySpan<char>
    /// <summary>
    ///
    /// </summary>
    public ref struct SplitEnumerator
    {
        private ReadOnlySpan<char> _str;
        private readonly char _separator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        public SplitEnumerator
            (
                ReadOnlySpan<char> str,
                char separator
            )
        {
            _str = str;
            _separator = separator;
            Current = default;
        }

        /// <summary>
        /// foreach compatible
        /// </summary>
        public SplitEnumerator GetEnumerator() => this;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            var span = _str;
            if (span.Length == 0) // Reach the end of the string
            {
                return false;
            }

            var index = span.IndexOf (_separator);
            if (index == -1)
            {
                // not exists
                _str = ReadOnlySpan<char>.Empty;
                Current = new SplitEntry (span, ReadOnlySpan<char>.Empty);
                return true;
            }

            Current = new SplitEntry (span.Slice (0, index), span.Slice (index, 1));
            _str = span.Slice (index + 1);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        public SplitEntry Current { get; private set; }
    }

    /// <summary>
    ///
    /// </summary>
    public readonly ref struct SplitEntry
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        public SplitEntry
            (
                ReadOnlySpan<char> line,
                ReadOnlySpan<char> separator
            )
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
        public void Deconstruct
            (
                out ReadOnlySpan<char> line,
                out ReadOnlySpan<char> separator
            )
        {
            line = Line;
            separator = Separator;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
        // foreach (ReadOnlySpan<char> entry in str.SplitLines())
        public static implicit operator ReadOnlySpan<char> (SplitEntry entry) => entry.Line;
    }
}
