// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* COPYDATASTRUCT.cs -- содержит данные для сообщения WM_COPYDATA
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Содержит данные, которые будут переданы другому приложению
/// с помощью сообщения WM_COPYDATA.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct COPYDATASTRUCT
{
    /// <summary>
    /// Данные, которые необходимо передать принимающему приложению.
    /// Вообще говоря, это могут быть любые данные по выбору
    /// отправляющей стороны, например, дескриптор главного окна.
    /// </summary>
    public IntPtr dwData;

    /// <summary>
    /// Размер в байтах данных, на которые указывает элемент lpData.
    /// </summary>
    public int cbData;

    /// <summary>
    /// Данные, которые необходимо передать принимающему приложению.
    /// Этот элемент может быть NULL.
    /// </summary>
    public IntPtr lpData;
}
