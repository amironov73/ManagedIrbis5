// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ACCEL.cs -- клавиша-акселератор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Клавиша-акселератор.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct ACCEL
{
    /// <summary>
    /// The accelerator behavior.
    /// </summary>
    public byte fVirt;

    /// <summary>
    /// The accelerator key.
    /// </summary>
    public short key;

    /// <summary>
    /// The accelerator identifier.
    /// </summary>
    public short cmd;
}
