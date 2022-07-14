// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* StdHandles.cs -- стандартные файловые дескрипторы
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Стандартные файловые дескрипторы.
/// </summary>
public enum StdHandles
    : uint
{
    /// <summary>
    /// Дескриптор устройства стандартного ввода.
    /// Изначально указывает на консольный буфер ввода, <c>CONIN$</c>.
    /// </summary>
    STD_INPUT_HANDLE = unchecked ((uint) -10),

    /// <summary>
    /// Дескриптор устройства стандартного вывода.
    /// Изначально указывает на консольный буфер вывода, <c>CONOUT$</c>.
    /// </summary>
    STD_OUTPUT_HANDLE = unchecked ((uint) -11),

    /// <summary>
    /// Дескриптор устройства стандартного потока ошибок.
    /// Изначально указывает на консольный буфер вывода, <c>CONOUT$</c>.
    /// </summary>
    STD_ERROR_HANDLE = unchecked ((uint) -12)
}
