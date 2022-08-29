// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* NMHDR.cs -- содержит информацию об уведомлении
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Содержит информацию об уведомлении.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct NMHDR
{
    /// <summary>
    /// Дескриптор окна (<c>HWND</c>) для элемента управления,
    /// отправляющего сообщение.
    /// </summary>
    public IntPtr ControlHandle;

    /// <summary>
    /// Идентификатор элемента управления, отправляющего сообщение.
    /// </summary>
    public uint ControlId;

    /// <summary>
    /// Код уведомления. Этот элемент может быть кодом уведомления
    /// для конкретного элемента управления или одним из общих
    /// кодов уведомления.
    /// </summary>
    public int Code;
}
