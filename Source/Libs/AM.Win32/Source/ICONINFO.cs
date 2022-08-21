// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ICONINFO.cs -- информация об иконке или курсоре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Информация об иконке или курсоре.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct ICONINFO
{
    /// <summary>
    /// Указывает, определяет ли эта структура значок или курсор.
    /// TRUE означает значок; FALSE означает курсор.
    /// </summary>
    public int fIcon;

    /// <summary>
    /// X-координата активной точки курсора. Если эта структура
    /// определяет значок, активная точка всегда находится
    /// в центре значка, и этот элемент игнорируется.
    /// </summary>
    public int xHotspot;

    /// <summary>
    /// Y-координата активной точки курсора. Если эта структура
    /// определяет значок, активная точка всегда находится в центре
    /// значка, и этот элемент игнорируется.
    /// </summary>
    public int yHotspot;

    /// <summary>
    /// Битовая маска значка. Если эта структура определяет
    /// черно-белый значок, эта битовая маска форматируется так,
    /// что верхняя половина представляет собой битовую маску
    /// значка И, а нижняя половина представляет собой битовую маску
    /// значка XOR. При этом условии высота должна быть четно кратна
    /// двум. Если эта структура определяет цветной значок,
    /// эта маска определяет только битовую маску И значка.
    /// </summary>
    public IntPtr hbmMask;

    /// <summary>
    /// Дескриптор растрового изображения цвета значка. Этот член
    /// может быть необязательным, если эта структура определяет
    /// черно-белый значок. Битовая маска И hbmMask применяется
    /// с флагом SRCAND к месту назначения; впоследствии цветовое
    /// растровое изображение применяется (используя XOR) к месту
    /// назначения с помощью флага SRCINVERT.
    /// </summary>
    public IntPtr hbmColor;
}
