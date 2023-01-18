// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* IncrementNode.cs -- оператор инкремента/декремента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Оператор инкремента/декремента (префиксного/постфиксного).
/// </summary>
public sealed class IncrementNode
    : UnaryNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncrementNode
        (
            AtomNode target,
            string operation,
            bool prefix
        )
    {
        _target = target;
        _operation = operation;
        _prefix = prefix;
    }

    #endregion

    #region Private members

    private readonly AtomNode _target;
    private readonly string _operation;
    private readonly bool _prefix;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var target = _target.Compute (context);

        Console.WriteLine ($"Increment: {_operation} {_prefix}");

        return target;
    }

    #endregion
}
