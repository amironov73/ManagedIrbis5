// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ColorHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

#pragma warning disable 649

namespace PdfSharpCore.Internal;

/// <summary>
///
/// </summary>
struct SColor
{
    public byte a;
    public byte r;
    public byte g;
    public byte b;
}

/// <summary>
///
/// </summary>
struct SCColor
{
    public float a;
    public float r;
    public float g;
    public float b;
}

/// <summary>
///
/// </summary>
static class ColorHelper
{
    public static float sRgbToScRgb (byte bval)
    {
        var num = bval / 255f;

        return num switch
        {
            <= 0.0f => 0f,
            <= 0.04045f => num / 12.92f,
            < 1f => MathF.Pow ((num + 0.055f) / 1.055f, 2.4f),
            _ => 1f
        };
    }

    public static byte ScRgbTosRgb (float val)
    {
        return val switch
        {
            <= 0.0f => 0,
            <= 0.0031308f => (byte)(255f * val * 12.92f + 0.5f),
            <= 1.0f => (byte)(255f * (1.055f * MathF.Pow (val, 0.41666666666666669f) - 0.055f) + 0.5f),
            _ => 0xff
        };
    }
}
