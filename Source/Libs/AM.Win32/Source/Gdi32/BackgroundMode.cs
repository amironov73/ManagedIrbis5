// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BackgroundMode.cs -- background mix mode for text, hatch brush drawing on device context
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Background mix mode for text, hatch brush drawing on device context.
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
