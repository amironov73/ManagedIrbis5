// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InternPoolExtensions.HtmlEncode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Text.Encodings.Web;

#endregion

#nullable enable

namespace AM.Collections.Intern;

/// <summary>
///
/// </summary>
public static partial class InternPoolExtensions
{
    // 32bit hex entity &#xffff0000;
    internal const int MaxCharExpansionSize = 10;

    public static string HtmlEncode (this IInternPool pool, string input)
        => HtmlEncode (pool, input.AsSpan());

#if NET5_0
        public static string HtmlEncode(this IInternPool pool, ReadOnlySpan<char> input)
        {
            // Need largest size, can't do multiple rounds of encoding due to https://github.com/dotnet/runtime/issues/45994
            if ((long)input.Length * MaxCharExpansionSize <= InternPool.StackAllocThresholdChars)
            {
                Span<char> output = stackalloc char[InternPool.StackAllocThresholdChars];
                var status =
 HtmlEncoder.Default.Encode(input, output, out int charsConsumed, out int charsWritten, isFinalBlock: true);

                if (status != OperationStatus.Done)
                    throw new InvalidOperationException("Invalid Data");

                output = output.Slice(0, charsWritten);
                return pool.Intern(output);
            }

            return HtmlEncodeSlower(pool, input);
        }


        public static string HtmlEncodeSlower(this IInternPool pool, ReadOnlySpan<char> input)
#else
    public static string HtmlEncode (this IInternPool pool, ReadOnlySpan<char> input)
#endif
    {
        // Need largest size, can't do multiple rounds of encoding due to https://github.com/dotnet/runtime/issues/45994
        var array = ArrayPool<char>.Shared.Rent (input.Length * MaxCharExpansionSize);
        Span<char> output = array;

        var status = HtmlEncoder.Default.Encode (input, output, out _, out int charsWritten, isFinalBlock: true);

        if (status != OperationStatus.Done)
            throw new InvalidOperationException ("Invalid Data");

        output = output.Slice (0, charsWritten);
        string result = pool.Intern (output);

        ArrayPool<char>.Shared.Return (array);

        return result;
    }
}
