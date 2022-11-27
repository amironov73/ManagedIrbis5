// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BackgroundMode.cs -- режим смешивания фона для текста, рисования штриховки кистью в контексте устройства
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Режим смешивания фона для текста, рисования штриховки кистью
/// в контексте устройства.
/// </summary>
[Flags]
public enum BackgroundMode
{
    /// <summary>
    /// Error.
    /// </summary>
    Error = 0,

    /// <summary>
    /// Transparent.
    /// </summary>
    Transparent = 1,

    /// <summary>
    /// Opaque.
    /// </summary>
    Opaque = 2
}
