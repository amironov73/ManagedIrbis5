// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ThrowNode.cs -- оператор throw
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Оператор throw.
/// </summary>
sealed class ThrowNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ThrowNode
        (
            AtomNode operand
        )
    {
        Sure.NotNull (operand);

        _operand = operand;
    }

    #endregion

    #region Private members

    private readonly AtomNode _operand;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var value = _operand.Compute (context);

        throw value switch
        {
            null => new ApplicationException(),
            string message => new ApplicationException (message),
            Exception exception => exception,
            _ => new ApplicationException (((object)value).ToString())
        };
    }

    #endregion
}
