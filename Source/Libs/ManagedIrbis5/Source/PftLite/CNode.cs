// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CNode.cs -- перемещение в указанную позицию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Перемещение в указанную позицию.
/// </summary>
internal sealed class CNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CNode
        (
            int position
        )
    {
        _position = position;
    }

    #endregion

    #region Private members

    private readonly int _position;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"C_{_position}";
    }

    #endregion
}
