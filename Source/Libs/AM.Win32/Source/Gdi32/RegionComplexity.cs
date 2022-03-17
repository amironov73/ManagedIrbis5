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

/* RegionComplexity.cs -- specifies the complexity of region
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Specifies the complexity of region.
/// </summary>
public enum RegionComplexity
{
    /// <summary>
    /// Error. No region created.
    /// </summary>
    ERROR = 0,

    /// <summary>
    /// The region is empty.
    /// </summary>
    NULLREGION = 1,

    /// <summary>
    /// The region is a single rectangle.
    /// </summary>
    SIMPLEREGION = 2,

    /// <summary>
    /// The region is more than a single rectangle.
    /// </summary>
    COMPLEXREGION = 3
}
