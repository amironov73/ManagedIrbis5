// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IncrementNode.cs -- оператор инкремента/декремента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Оператор инкремента/декремента (префиксного/постфиксного).
/// </summary>
internal sealed class IncrementNode
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
        Sure.NotNull (target);

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
        var result = target;

        target = _operation switch
        {
            "++" => target + 1,
            "--" => target - 1,
            _ => throw new InvalidOperationException()
        };

        _target.Assign (context, "=", target);
        if (_prefix)
        {
            result = target;
        }

        return result;
    }

    #endregion
}
