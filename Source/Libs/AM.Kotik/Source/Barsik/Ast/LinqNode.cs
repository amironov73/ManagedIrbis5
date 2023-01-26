// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LinqNode.cs -- жалкое подобие LINQ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/*

  from score in scores //required
  where score > 80 // optional
  orderby score // optional
  select score; //must end with select or group

 */

/// <summary>
/// Жалкое подобие LINQ.
/// </summary>
public sealed class LinqNode
    : AtomNode
{
    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
