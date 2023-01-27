// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CommaNode.cs -- узел, соответствующий запятой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace MicroPft.Ast;

/// <summary>
/// Узел, соответствующий запятой.
/// </summary>
internal sealed class CommaNode
    : PftNode
{
    #region PftNode members

    public override void Execute
        (
            PftContext context
        )
    {
        // пустое тело метода
    }

    #endregion

    #region MereSerializer members

    /// <inheritdoc cref="PftNode.MereSerialize"/>
    public override void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => ",";

    #endregion
}
