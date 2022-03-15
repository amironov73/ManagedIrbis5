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

/* DocInfoFlags.cs -- specifies additional information about the print job
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Specifies additional information about the print job.
/// </summary>
[Flags]
public enum DocInfoFlags
{
    /// <summary>
    /// Applications that use banding should set this flag for optimal
    /// performance during printing.
    /// </summary>
    DI_APPBANDING = 0x00000001,

    /// <summary>
    /// The application will use raster operations that involve reading
    /// from the destination surface.
    /// </summary>
    DI_ROPS_READ_DESTINATION = 0x00000002
}
