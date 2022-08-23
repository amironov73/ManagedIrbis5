// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* HELPINFO.cs -- информация об элементе, для которого затребована подсказка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Информация об элементе, для которого была затребована контекстно-
/// чувствительная подсказка.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct HELPINFO
{
    /// <summary>
    /// Размер структуры в байтах. Зависит от разрядности платформы.
    /// </summary>
    public int Size;

    /// <summary>
    ///Тип контекста, для которого запрашивается справка.
    /// Этот элемент может быть одним из следующих значений.
    /// <list type="table">
    /// <listheader>
    ///     <term>Значение</term>
    ///     <description>Описание</description>
    /// </listheader>
    /// <item>
    ///     <term>HELPINFO_MENUITEM</term>
    ///     <description>Запрошена помощь по пункту меню.</description>
    /// </item>
    /// <item>
    ///     <term>HELPINFO_WINDOW</term>
    ///     <description>Запрошена помощь для элемента управления
    ///     или окна.</description>
    /// </item>
    /// </list>
    /// </summary>
    public int ContextType;

    /// <summary>
    /// Идентификатор окна или элемента управления,
    /// если iContextType равен HELPINFO_WINDOW,
    /// или идентификатор пункта меню, если iContextType
    /// равен HELPINFO_MENUITEM.
    /// </summary>
    public int ControlID;

    /// <summary>
    /// Идентификатор дочернего окна или элемента управления,
    /// если iContextType равен HELPINFО_WINDOW, или идентификатор
    /// связанного меню, если iContextType равен HELPINFO_MENUITEM.
    /// </summary>
    public IntPtr ItemHandle;

    /// <summary>
    /// Идентификатор контекста справки окна или элемента управления.
    /// </summary>
    public int ContextId;

    /// <summary>
    /// Структура POINT, содержащая экранные координаты курсора мыши.
    /// Это полезно для предоставления справки в зависимости
    /// от положения курсора мыши.
    /// </summary>
    public Point MousePos;
}
