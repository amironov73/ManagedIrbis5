// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo

/* GridLengthDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
///
/// </summary>
internal struct GridLengthDefinition
{
    #region Properties and fields

    public GridLength GridLength;

    public double? Min;

    public double? Max;

    #endregion
}
