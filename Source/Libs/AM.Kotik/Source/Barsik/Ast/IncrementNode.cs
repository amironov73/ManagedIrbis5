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

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

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

    private static dynamic DoIncrement
        (
            dynamic value
        )
    {
        if (value is string text)
        {
            var number = new NumberText (text);
            number.Increment();

            return number.ToString();
        }

        return (value + 1);
    }

    private static dynamic DoDecrement
        (
            dynamic value
        )
    {
        if (value is string text)
        {
            var number = new NumberText (text);
            number.Increment();

            return number.ToString();
        }

        return (value - 1);
    }

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
            "++" => DoIncrement (target),
            "--" => DoDecrement (target),
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
