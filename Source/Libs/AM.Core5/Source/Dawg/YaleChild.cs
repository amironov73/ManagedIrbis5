// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* YaleChild.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal readonly struct YaleChild
{
    #region Pseudo-properties

    public readonly int Index;
    public readonly ushort CharIndex;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YaleChild
        (
            int index,
            ushort charIndex
        )
    {
        Index = index;
        CharIndex = charIndex;
    }

    #endregion
}
