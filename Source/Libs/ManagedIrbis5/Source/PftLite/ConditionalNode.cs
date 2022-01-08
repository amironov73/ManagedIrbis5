// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConditionalNode.cs -- условный литерал
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
/// Условный литерал.
/// </summary>
internal sealed class ConditionalNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConditionalNode
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

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"conditional: \"{_value}\"";
    }

    #endregion
}
