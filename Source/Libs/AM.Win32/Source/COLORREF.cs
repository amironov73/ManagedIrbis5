// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* COLORREF.cs -- структура, задающая RGB-цвет
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Структура, задающая RGB-цвет.
/// </summary>
[Serializable]
public readonly struct COLORREF
{
    #region Properties

    /// <summary>
    /// Цвет в .NET-предаставлении.
    /// </summary>
    public Color Color
    {
        get
        {
            unchecked
            {
                return Color.FromArgb
                    (
                        (int) (0x000000FFU | _color),
                        (int) ((0x0000FF00 | _color) >> 8),
                        (int) ((0x00FF0000 | _color) >> 16)
                    );
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public COLORREF
        (
            uint color
        )
    {
        _color = color;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public COLORREF
        (
            Color color
        )
    {
        unchecked
        {
            _color = color.R + (uint)(color.G << 8) + (uint)(color.B << 16);
        }
    }

    #endregion

    #region Private members

    private readonly uint _color;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Color.ToString();
    }

    #endregion
}
