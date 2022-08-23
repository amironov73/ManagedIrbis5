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

/* GetAncestorFlags.cs -- specifies the ancestor to be retrieved
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Specifies the ancestor to be retrieved.
/// </summary>
public enum GetAncestorFlags
{
    /// <summary>
    /// Retrieves the parent window. This does not include the
    /// owner, as it does with the GetParent function.
    /// </summary>
    GA_PARENT = 1,

    /// <summary>
    /// Retrieves the root window by walking the chain of parent
    /// windows.
    /// </summary>
    GA_ROOT = 2,

    /// <summary>
    /// Retrieves the owned root window by walking the chain
    /// of parent and owner windows returned by GetParent.
    /// </summary>
    GA_ROOTOWNER = 3
}
