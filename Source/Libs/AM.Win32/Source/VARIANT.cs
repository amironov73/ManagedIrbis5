// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* VARIANT.cs -- вариантная структура Win32
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Вариантная структура Win32.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct VARIANT
{
    /// <summary>
    /// Тип данных, хранимых в структуре в данный момент.
    /// </summary>
    [MarshalAs (UnmanagedType.I2)]
    public short vt;

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    [MarshalAs (UnmanagedType.I2)]
    public short reserved1;

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    [MarshalAs (UnmanagedType.I2)]
    public short reserved2;

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    [MarshalAs (UnmanagedType.I2)]
    public short reserved3;

    /// <summary>
    /// Первая порция данных.
    /// </summary>
    public IntPtr data1;

    /// <summary>
    /// Вторая порция данных.
    /// </summary>
    public IntPtr data2;
}
