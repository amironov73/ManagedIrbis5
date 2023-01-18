// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
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
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public AtomNode? Target { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IncrementNode
        (
            bool decrement,
            bool prefix
        )
    {
        _decrement = decrement;
        _prefix = prefix;
    }

    #endregion

    #region Private members

    private readonly bool _decrement;
    private readonly bool _prefix;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var target = Target!.Compute (context);

        Console.WriteLine ($"Increment: {_decrement} {_prefix}");

        return target;
    }

    #endregion
}
