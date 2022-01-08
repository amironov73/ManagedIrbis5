// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatingNode.cs -- повторяющийся литерал
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
/// Повторяющийся литерал.
/// </summary>
internal sealed class RepeatingNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatingNode
        (
            string value,
            bool plus
        )
    {
        _value = value;
        _plus = plus;
    }

    #endregion

    #region Private members

    private readonly string _value;

    private readonly bool _plus;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"repeating: \"{_value}\" {_plus}";
    }

    #endregion

}
