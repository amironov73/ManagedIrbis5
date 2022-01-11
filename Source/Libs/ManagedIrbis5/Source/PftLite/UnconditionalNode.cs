// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* UnconditionalNode.cs -- безусловный литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Безусловный литерал.
/// </summary>
internal sealed class UnconditionalNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UnconditionalNode
        (
            string value
        )
    {
        _value = value;
    }

    #endregion

    #region Private members

    private readonly string _value;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        context.Write (_value);
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
    public override string ToString()
    {
        return $"unconditional: \"{_value}\"";
    }

    #endregion
}
