// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* SIZEL.cs -- ширина и высота прямоугольника
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Высота и ширина прямоугольника.
/// Единицы измерения зависят от используемой функции.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct SIZEL
{
    #region Properties

    /// <summary>
    /// Ширина прямоугольника. Единица измерения зависит от используемой функции.
    /// </summary>
    public int cx;

    /// <summary>
    /// Высота прямоугольника. Единица измерения зависит от используемой функции.
    /// </summary>
    public int cy;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{cx} x {cy}";
    }

    #endregion
}
