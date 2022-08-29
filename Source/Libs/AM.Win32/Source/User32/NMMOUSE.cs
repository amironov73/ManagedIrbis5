// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* NMMOUSE.cs -- уведомление о событиях, связанных с мышью
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Содержит информацию, используемую с сообщениями уведомлений
/// о событиях, связанных с мышью.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct NMMOUSE
{
    /// <summary>
    /// Структура <see cref="NMHDR" />, содержащая дополнительную
    /// информацию об этом уведомлении.
    /// </summary>
    public NMHDR hdr;

    /// <summary>
    /// Идентификатор элемента управления.
    /// </summary>
    public IntPtr dwItemSpec;

    /// <summary>
    /// Данные элемента управления.
    /// </summary>
    public IntPtr dwItemData;

    /// <summary>
    /// Структура <see cref="Point" />, содержащая экранные координаты
    /// мыши в момент щелчка.
    /// </summary>
    public Point pt;

    /// <summary>
    /// Содержит информацию о том, куда на элементе или элементе
    /// управления указывает курсор.
    /// </summary>
    public IntPtr dwHitInfo;
}
