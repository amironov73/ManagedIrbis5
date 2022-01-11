// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* XNode.cs -- вставка указанного количества пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Вставка указанного количества пробелов.
/// </summary>
internal sealed class XNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public XNode
        (
            int length
        )
    {
        _length = length;
    }

    #endregion

    #region Private members

    private readonly int _length;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (context.CurrentRepeat == 0)
        {
            context.Write (new string (' ', _length));
        }
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
        return $"X_{_length}";
    }

    #endregion
}
