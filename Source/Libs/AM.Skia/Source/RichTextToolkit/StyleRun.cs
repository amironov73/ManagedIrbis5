// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StyleRun.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit;

using Utils;

/// <summary>
/// Represets a style run - a logical run of characters all with the same
/// style.
/// </summary>
public class StyleRun
    : IRun
{
    /// <summary>
    /// Get the code points of this run.
    /// </summary>
    public Slice<int> CodePoints => CodePointBuffer!.SubSlice (Start, Length);

    /// <summary>
    /// Get the text of this style run
    /// </summary>
    /// <returns>A string</returns>
    public override string ToString()
    {
        return Utf32Utils.FromUtf32 (CodePoints);
    }

    /// <summary>
    /// The index of the first code point in this run (relative to the text block
    /// as a whole).
    /// </summary>
    public int Start { get; internal set; }

    /// <summary>
    /// The number of code points this run.
    /// </summary>
    public int Length { get; internal set; }

    /// <summary>
    /// The index of the first code point after this run.
    /// </summary>
    public int End => Start + Length;

    /// <summary>
    /// The style attributes to be applied to text in this run.
    /// </summary>
    public IStyle? Style { get; internal set; }

    int IRun.Offset => Start;
    int IRun.Length => Length;

    /// <summary>
    /// The global list of code points
    /// </summary>
    internal Buffer<int>? CodePointBuffer;

    internal static ThreadLocal<ObjectPool<StyleRun>> Pool = new (() =>
        new ObjectPool<StyleRun>()
        {
            Cleaner = (r) =>
            {
                r.CodePointBuffer = null;
                r.Style = null;
            }
        });
}
