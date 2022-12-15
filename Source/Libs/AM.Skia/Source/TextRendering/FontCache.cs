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

using System.Collections.Generic;

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.TextRendering;

/// <summary>
///
/// </summary>
public class FontCache
{
    private static readonly Dictionary<int, FontCache> CacheStore = new ();

    private static int GetCacheKey (SKTypeface typeface, float fontSize)
    {
        return typeface.GetHashCode() + (int)(fontSize * 100f);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="typeface"></param>
    /// <param name="fontSize"></param>
    /// <returns></returns>
    public static FontCache GetCache (SKTypeface typeface, float fontSize)
    {
        var cacheKey = GetCacheKey (typeface, fontSize);
        if (CacheStore.TryGetValue (cacheKey, out var fontCache))
        {
            return fontCache;
        }

        var newFontCache = new FontCache (typeface, fontSize);
        CacheStore[cacheKey] = newFontCache;

        return newFontCache;
    }

    /// <summary>
    ///
    /// </summary>
    public float FontAscender { get; }

    private Dictionary<char, FontLetterDefinition> _LetterDefinitions = new Dictionary<char, FontLetterDefinition>();
    private SKPaint _TextPaint = new SKPaint();

    /// <summary>
    ///
    /// </summary>
    /// <param name="typeface"></param>
    /// <param name="fontSize"></param>
    public FontCache (SKTypeface typeface, float fontSize)
    {
        _TextPaint.Typeface = typeface;
        _TextPaint.TextSize = fontSize;

        FontAscender = -_TextPaint.FontMetrics.Ascent;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="character"></param>
    /// <param name="letterDefinition"></param>
    /// <returns></returns>
    public bool GetLetterDefinitionForChar (char character, out FontLetterDefinition? letterDefinition)
    {
        if (character == UnicodeCharacters.NoBreakSpace)
        {
            // change no-break space to regular space
            // reason: some fonts have issue with no-break space:
            //   * no letter definition
            //   * not normal big width
            character = UnicodeCharacters.Space;
        }

        return _LetterDefinitions.TryGetValue (character, out letterDefinition) && letterDefinition.ValidDefinition;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="character"></param>
    /// <param name="letterDefinition"></param>
    public void AddLetterDefinition (char character, FontLetterDefinition letterDefinition)
    {
        _LetterDefinitions[character] = letterDefinition;
    }

    public bool PrepareLetterDefinitions (string text)
    {
        List<char> newChars = new List<char>();
        FindNewCharacters (text, ref newChars);

        if (newChars.Count == 0)
        {
            return false;
        }

        var newCharString = new string (newChars.ToArray());

        var glyphs = _TextPaint.GetGlyphs (newCharString);
        var glyphWidths = _TextPaint.GetGlyphWidths (newCharString);

        for (int i = 0; i < glyphs.Length; i++)
        {
            FontLetterDefinition tempDef = new FontLetterDefinition();

            if (glyphs[i] == 0)
            {
                _LetterDefinitions[newCharString[i]] = tempDef;
            }
            else
            {
                tempDef.ValidDefinition = true;
                tempDef.AdvanceX = glyphWidths[i];

                _LetterDefinitions[newCharString[i]] = tempDef;
            }
        }

        return true;
    }

    private void FindNewCharacters (string text, ref List<char> chars)
    {
        if (_LetterDefinitions.Count == 0)
        {
            chars.AddRange (text);
        }
        else
        {
            foreach (var c in text)
            {
                if (!_LetterDefinitions.ContainsKey (c))
                {
                    chars.Add (c);
                }
            }
        }
    }
}
