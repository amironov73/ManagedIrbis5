// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit
{
    /// <summary>
    /// Provides styling information for a run of text.
    /// </summary>
    public interface IStyle
    {
        /// <summary>
        /// The font family for text this text run.
        /// </summary>
        string FontFamily { get; }

        /// <summary>
        /// The font size for text in this run.
        /// </summary>
        float FontSize { get; }

        /// <summary>
        /// The font weight for text in this run.
        /// </summary>
        int FontWeight { get; }

        /// <summary>
        /// The font weight for text in this run.
        /// </summary>
        SKFontStyleWidth FontWidth { get; }

        /// <summary>
        /// True if the text in this run should be displayed in an italic
        /// font; otherwise False.
        /// </summary>
        bool FontItalic { get; }

        /// <summary>
        /// The underline style for text in this run.
        /// </summary>
        UnderlineStyle Underline { get; }

        /// <summary>
        /// The strike through style for the text in this run
        /// </summary>
        StrikeThroughStyle StrikeThrough { get; }

        /// <summary>
        /// The line height for text in this run as a multiplier (defaults to 1)
        /// </summary>
        float LineHeight { get; }

        /// <summary>
        /// The text color for text in this run.
        /// </summary>
        SKColor TextColor { get; }

        /// <summary>
        /// The background color of this run.
        /// </summary>
        SKColor BackgroundColor { get; }

        /// <summary>
        /// Color of the halo
        /// </summary>
        SKColor HaloColor { get; }

        /// <summary>
        /// Width of halo
        /// </summary>
        float HaloWidth { get; }

        /// <summary>
        /// Blur of halo
        /// </summary>
        float HaloBlur { get; }

        /// <summary>
        /// Extra spacing between each character
        /// </summary>
        float LetterSpacing { get; }

        /// <summary>
        /// The font variant (ie: super/sub-script) for text in this run.
        /// </summary>
        FontVariant FontVariant { get; }

        /// <summary>
        /// Text direction override for this span
        /// </summary>
        TextDirection TextDirection { get; }

        /// <summary>
        /// Specifies a replacement character to be displayed (password mode)
        /// </summary>
        char ReplacementCharacter { get; }
    }
}
