// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IdealHashObjectBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
///
/// </summary>
public abstract class IdealHashObjectBase
{
    internal int IdealHashCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => IdealHashCode;
}
