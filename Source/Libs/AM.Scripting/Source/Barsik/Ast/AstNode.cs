// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AstNode.cs -- абстрактный узел AST
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Runtime.Mere;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Абстрактный узел AST.
/// </summary>
public abstract class AstNode
    : IMereSerializable

{
    #region IMereSerializable members

    /// <inheritdoc cref="IMereSerializable.MereSerialize"/>
    public virtual void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IMereSerializable.MereDeserialize"/>
    public virtual void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
