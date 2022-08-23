// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

/* DLLVERSIONINFO.cs -- используется в DllGetVesion
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Содержит информацию о версии конкретной DLL.
/// Используется в функции DllGetVersion.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct DLLVERSIONINFO
{
    #region Properties

    /// <summary>
    /// Заранее подсчитанный размер структуры в байтах.
    /// </summary>
    private const int Size = 20;

    /// <summary>
    /// Размер структуры в байтах. Этот член должен быть заполнен
    /// до вызова функции.
    /// </summary>
    public uint StructureSize;

    /// <summary>
    /// Основная версия DLL. Например, если версия DLL - 4.0.950,
    /// это поле будет равно 4.
    /// </summary>
    public uint MajorVersion;

    /// <summary>
    /// Минорная версия DLL. Например, если версия DLL - 4.0.950,
    /// это поле будет равно 0.
    /// </summary>
    public uint MinorVersion;

    /// <summary>
    /// Номер сборки DLL. Например, если версия DLL - 4.0.950,
    /// это поле будет равно 950.
    /// </summary>
    public uint BuildNumber;

    /// <summary>
    /// Идентифицирует платформу, для которой была создана
    /// библиотека DLL.
    /// </summary>
    public uint PlatformID;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{MajorVersion}.{MinorVersion}.{BuildNumber} ({PlatformID})";
    }

    #endregion
}
