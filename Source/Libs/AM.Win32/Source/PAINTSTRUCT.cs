// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PAINTSTRUCT.cs -- used to paint client area of a window
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
/// Структура PAINTSTRUCT содержит информацию для приложения.
/// Эта информация может использоваться для рисования клиентской
/// области окна, принадлежащего этому приложению.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct PAINTSTRUCT
{
    /// <summary>
    /// Дескриптор DC устройства, который будет использоваться для рисования.
    /// </summary>
    public IntPtr hdc;

    /// <summary>
    /// Указывает, нужно ли стирать фон. Это значение не равно нулю,
    /// если приложение должно стереть фон. Приложение отвечает
    /// за стирание фона, если класс окна создается без фоновой кисти.
    /// </summary>
    public int fErase;

    /// <summary>
    /// Структура RECT, указывающая верхний левый и нижний правый углы
    /// прямоугольника, в котором запрашивается рисование, в единицах
    /// устройства относительно левого верхнего угла клиентской области.
    /// </summary>
    public Rectangle rcPaint;

    /// <summary>
    /// Зарезервировано для системных нужд.
    /// </summary>
    public int fRestore;

    /// <summary>
    /// Зарезервировано для системных нужд.
    /// </summary>
    public int fIncUpdate;

    /// <summary>
    /// Зарезервировано для системных нужд.
    /// </summary>
    [MarshalAs (UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] rgbReserved;
}
